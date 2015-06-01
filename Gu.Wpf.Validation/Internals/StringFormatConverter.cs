namespace Gu.Wpf.Validation.Internals
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Windows.Controls;
    using System.Windows.Data;

    public class StringFormatConverter : IValueConverter
    {
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var textBox = (TextBox)parameter;
            if (textBox.IsKeyboardFocused || textBox.GetIsUpdating())
            {
                Debug.WriteLine("Convert: {0} IsKeyboardFocused DoNothing", value);
                return Binding.DoNothing;
            }

            var converter = textBox.GetStringConverter();

            Debug.WriteLine("Convert: {0}", value);
            var convert = converter.ToString(value, textBox);
            return convert;
        }

        public virtual object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            var textBox = (TextBox)parameter;
            if (textBox.GetIsUpdating())
            {
                return textBox.GetValue(Input.ValueProperty);
            }
            var converter = textBox.GetStringConverter();
            object result;
            if (converter.TryParse(value, textBox, out result))
            {
                Debug.WriteLine(@"ConvertBack: value: ""{0}"" (result:{1}, Value: {2}, Text: ""{3}"")",
                    value,
                    textBox.GetValue(),
                    result,
                    textBox.Text);
                return result;
            }
            Debug.WriteLine(@"ConvertBack: value: ""{0}"" (Text:""{1}"") Reset", value, textBox.Text);
            return Binding.DoNothing;
        }
    }
}
