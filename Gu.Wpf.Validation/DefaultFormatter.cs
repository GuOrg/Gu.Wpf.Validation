namespace Gu.Wpf.Validation
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    using Gu.Wpf.Validation.Internals;
    using Gu.Wpf.Validation.StringConverters;

    public class DefaultFormatter : IFormatter
    {
        protected static readonly PropertyPath IsKeyboardFocusedPath = new PropertyPath(UIElement.IsKeyboardFocusedProperty);
        protected static readonly PropertyPath DecimalDigitsPath = new PropertyPath(Input.DecimalDigitsProperty);
        protected static readonly PropertyPath CulturePath = new PropertyPath(Input.CultureProperty);
        protected static readonly PropertyPath RawTextPath = new PropertyPath(RawValueTracker.RawTextProperty);

        protected static readonly DependencyProperty UpdateFormattingFlagsProperty = DependencyProperty.RegisterAttached(
                "UpdateFormattingFlags",
                typeof(Flags),
                typeof(DefaultFormatter),
                new PropertyMetadata(null, OnFlagsChanged));

        protected static readonly FlagsConverter UpdateFormattingConverter = new FlagsConverter(DefaultFormatter.UpdateFormattingFlagsProperty);

        private static readonly RoutedEventHandler OnLoadedHandler = new RoutedEventHandler(OnLoaded);
        private static readonly RoutedEventHandler OnFormattingDirtyHandler = new RoutedEventHandler(OnFormattingDirty);

        internal static readonly RoutedEventArgs FormattingDirtyArgs = new RoutedEventArgs(Input.FormattingDirtyEvent);

        public virtual void Bind(TextBox textBox)
        {
            if (textBox == null)
            {
                return;
            }
            if (textBox.IsLoaded)
            {
                ClearBindings(textBox);
                AddBinding(textBox);
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
        protected virtual void AddBinding(TextBox textBox)
        {
            textBox.UpdateHandler(Input.FormattingDirtyEvent, OnFormattingDirtyHandler);
            var binding = new MultiBinding { Converter = UpdateFormattingConverter, ConverterParameter = textBox };
            binding.Bindings.Add(CreateBinding(textBox, IsKeyboardFocusedPath));
            binding.Bindings.Add(CreateBinding(textBox, DecimalDigitsPath));
            binding.Bindings.Add(CreateBinding(textBox, CulturePath));
            binding.Bindings.Add(CreateBinding(textBox, RawTextPath));
            BindingOperations.SetBinding(textBox, UpdateFormattingFlagsProperty, binding);
        }

        protected virtual void ClearBindings(TextBox textBox)
        {
            BindingOperations.ClearBinding(textBox, UpdateFormattingFlagsProperty);
        }

        protected virtual Binding CreateBinding(
            TextBox source,
            PropertyPath path)
        {
            return new Binding
            {
                Path = path,
                Source = source,
                Mode = BindingMode.OneWay,
                ConverterParameter = source,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
        }


        protected virtual void UpdateFormatting(TextBox textBox)
        {
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
                formatter.AddBinding(textBox);
            }
        }

        private static void OnFlagsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = (TextBox)d;
            if (textBox.GetIsUpdating() || textBox.IsKeyboardFocused || textBox.GetIsReceivingUserInput())
            {
                return;
            }

            var converter = textBox.GetStringConverter();
            if (converter == null)
            {
                return;
            }

            textBox.RaiseEvent(FormattingDirtyArgs);
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
