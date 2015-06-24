namespace Gu.Wpf.Validation.Internals
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Windows.Controls;
    using System.Windows.Data;

    using Gu.Wpf.Validation.StringConverters;

    public class UpdateFormattingConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            var textBox = (TextBox)parameter;
            if (textBox.IsKeyboardFocused || textBox.GetIsUpdating() || textBox.GetIsReceivingUserInput())
            {
                return value;
            }

            var converter = textBox.GetStringConverter();
            if (converter == null)
            {
                return value;
            }

            Debug.WriteLine(@"{0}.Convert({1}) -> TryUpdateTextFromRaw(textBox, converter) textBox.Text: ", value.ToDebugString(), textBox.Text.ToDebugString());
            TryUpdateTextFromRaw(textBox, converter);

            return value;
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("Formatting bindings should be oneway");
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

        private void TryUpdateTextFromRaw(TextBox textBox, IStringConverter converter)
        {
            object rawValue;
            if (TryGetRawValue(textBox, converter, out rawValue))
            {
                TryUpdateTextBox(textBox, converter, rawValue);
            }
        }
    }
}
