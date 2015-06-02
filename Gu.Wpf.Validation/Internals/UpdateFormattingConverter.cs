namespace Gu.Wpf.Validation.Internals
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    using Gu.Wpf.Validation.StringConverters;

    public class UpdateFormattingConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            Debug.WriteLine(string.Format(@"UpdateFormattingConverter.Convert({0}) ", value));
            var textBox = (TextBox)parameter;
            if (textBox.IsKeyboardFocused || textBox.GetIsUpdating())
            {
                return value;
            }

            var converter = textBox.GetStringConverter();
            if (converter == null)
            {
                return value;
            }

            var hasError = (bool)textBox.GetValue(Validation.HasErrorProperty);
            if (hasError)
            {
                if (!Equals(textBox.GetOldCulture(), textBox.GetCulture()))
                {
                    UpdateCulture(textBox, converter);
                }
                return value;
            }

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
            var rawText = textBox.GetRawText();
            if (converter.TryParse(rawText, textBox, out rawValue))
            {
                return true;
            }
            var format = textBox.GetCulture();
            var oldCulture = textBox.GetOldCulture();
            textBox.Update(Input.CultureProperty, oldCulture);
            var result = converter.TryParse(rawText, textBox, out rawValue);
            textBox.Update(Input.CultureProperty, format);
            return result;
        }

        protected virtual void UpdateCulture(TextBox textBox, IStringConverter converter)
        {
            TryUpdateTextFromRaw(textBox, converter);
            textBox.SetValue(TextBoxExt.OldCultureProperty, textBox.GetCulture());
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
