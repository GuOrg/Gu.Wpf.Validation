namespace Gu.Wpf.Validation
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    using Gu.Wpf.Validation.Internals;
    using Gu.Wpf.Validation.StringConverters;

    public class DefaultFormatter : IFormatter
    {
        private static readonly RoutedEventHandler OnLoadedHandler = OnLoaded;
        private static readonly RoutedEventHandler OnFormattingDirtyHandler = OnFormattingDirty;

        public virtual void Bind(TextBox textBox)
        {
            if (textBox == null)
            {
                return;
            }
            if (textBox.IsLoaded)
            {
                AddHandlers(textBox);
            }
            else
            {
                textBox.UpdateHandler(FrameworkElement.LoadedEvent, OnLoadedHandler);
            }
        }

        /// <summary>
        /// Using a binding with converter to update formatting
        /// </summary>
        /// <param name="textBox"></param>
        protected virtual void AddHandlers(TextBox textBox)
        {
            textBox.UpdateHandler(Input.FormattingDirtyEvent, OnFormattingDirtyHandler);
            textBox.UpdateHandler(UIElement.LostKeyboardFocusEvent, OnFormattingDirtyHandler);
        }

        protected virtual void RemoveHandlers(TextBox textBox)
        {
            textBox.RemoveHandler(Input.FormattingDirtyEvent, OnFormattingDirtyHandler);
            textBox.RemoveHandler(UIElement.LostKeyboardFocusEvent, OnFormattingDirtyHandler);
        }

        protected virtual void UpdateFormatting(TextBox textBox)
        {
            if (textBox.IsKeyboardFocused)
            {
                return;
            }

            if (textBox.GetIsReceivingUserInput())
            {
                return;
            }

            if (textBox.GetIsUpdating())
            {
                return;
            }

            var converter = textBox.GetStringConverter();
            if (converter == null)
            {
                return;
            }
            object rawValue;
            if (TryGetRawValue(textBox, converter, out rawValue))
            {
                TryUpdateTextBox(textBox, converter, rawValue);
            }
        }

        protected virtual bool TryUpdateTextBox(TextBox textBox, IStringConverter converter, object rawValue)
        {
            string formatted = converter.ToFormattedString(rawValue, textBox) ?? String.Empty;
            if (formatted != textBox.Text)
            {
                textBox.SetIsUpdating(true);
                textBox.SetTextUndoable(formatted);
                textBox.SetIsUpdating(false);
                return true;
            }
            return false;
        }

        protected virtual bool TryGetRawValue(TextBox textBox, IStringConverter converter, out object rawValue)
        {
            rawValue = textBox.GetRawValue();
            if (rawValue != RawValueTracker.Unset)
            {
                return true;
            }
            return false;
        }

        private static void OnLoaded(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            textBox.RemoveHandler(FrameworkElement.LoadedEvent, OnLoadedHandler);
            var formatter = textBox.GetFormatter() as DefaultFormatter;
            if (formatter != null)
            {
                formatter.AddHandlers(textBox);
            }
        }

        private static void OnFormattingDirty(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            var defaultValidator = textBox.GetFormatter() as DefaultFormatter;
            if (defaultValidator != null)
            {
                defaultValidator.UpdateFormatting(textBox);
            }
        }
    }
}
