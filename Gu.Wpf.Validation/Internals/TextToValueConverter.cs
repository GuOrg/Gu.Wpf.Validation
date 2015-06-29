namespace Gu.Wpf.Validation.Internals
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    public class TextToValueConverter : IValueConverter
    {
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == Input.Unset)
            {
                return DependencyProperty.UnsetValue;
            }
            Debug.Write(string.Format(@"{0}.Convert({1}) -> ", GetType().PrettyName(), value.ToDebugString()));
            var textBox = (TextBox)parameter;
            if (textBox.GetIsUpdating())
            {
                Debug.WriteLine(string.Format(@"Binding.DoNothing (textBox.GetIsUpdating()) textBox.Text: {0}", textBox.Text.ToDebugString()));
                return Binding.DoNothing;
            }
            textBox.SetRawValue(value);
            if (textBox.IsKeyboardFocused)
            {
                Debug.WriteLine(string.Format(@"Binding.DoNothing (textBox.IsKeyboardFocused) textBox.Text: {0}", textBox.Text.ToDebugString()));
                return Binding.DoNothing;
            }

            var converter = textBox.GetStringConverter();
            var formatted = converter.ToFormattedString(value, textBox);
            if (formatted == textBox.Text)
            {
                Debug.WriteLine(string.Format(@"Binding.DoNothing (formatted == textBox.Text) textBox.Text: {0}", textBox.Text.ToDebugString()));
                return Binding.DoNothing;
            }
            Debug.WriteLine(@"{0} textBox.Text: {1}", formatted.ToDebugString(), textBox.Text.ToDebugString());
            return formatted;
        }

        public virtual object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            Debug.Write(string.Format(@"{0}.ConvertBack({1}) -> ", GetType().PrettyName(), value.ToDebugString()));
            var textBox = (TextBox)parameter;
            if (textBox.GetIsUpdating())
            {
                var convertBack = textBox.GetValue(Input.ValueProperty);
                Debug.WriteLine(@"{0} (textBox.GetIsUpdating()) textBox.Text: {1}",
                    convertBack.ToDebugString(),
                    textBox.Text.ToDebugString());
                return convertBack;
            }
            var converter = textBox.GetStringConverter();
            object result;
            if (converter.TryParse(value, textBox, out result))
            {
                Debug.WriteLine(@"{0} (converter.TryParse(value, textBox, out result)) textBox.Text: {1}",
                    result ?? "null",
                    textBox.Text.ToDebugString());
                return result;
            }
            Debug.WriteLine(string.Format(@"Binding.DoNothing textBox.Text: {0}) Reset", textBox.Text.ToDebugString()));
            return Binding.DoNothing;
        }
    }
}
