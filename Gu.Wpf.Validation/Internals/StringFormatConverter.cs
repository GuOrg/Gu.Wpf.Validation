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
            Debug.Write(string.Format(@"StringFormatConverter.Convert({0}) ", value));
            var textBox = (TextBox)parameter;
            if (textBox.GetIsUpdating())
            {
                Debug.WriteLine("IsUpdating DoNothing");
                return Binding.DoNothing;
            }

            if (textBox.IsKeyboardFocused)
            {
                Debug.WriteLine("IsKeyboardFocused DoNothing");
                return Binding.DoNothing;
            }

            var converter = textBox.GetStringConverter();
            var formatted = converter.ToFormattedString(value, textBox);
            Debug.WriteLine(string.Format("formatted: {0}", formatted ?? "null"));
            Debug.WriteLine("SetRawText: " + converter.ToRawString(value, textBox));
            textBox.SetRawText(converter.ToRawString(value, textBox));
            return formatted;
        }

        public virtual object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            Debug.Write(string.Format(@"StringFormatConverter.ConvertBack(""{0}"") ", value));
            var textBox = (TextBox)parameter;
            if (textBox.GetIsUpdating())
            {
                Debug.WriteLine("IsUpdating");
                return textBox.GetValue(Input.ValueProperty);
            }
            var converter = textBox.GetStringConverter();
            object result;
            if (converter.TryParse(value, textBox, out result))
            {
                Debug.WriteLine(@"(result:{0}, Value: {1}, Text: ""{2}"")",
                    textBox.GetValue(),
                    result,
                    textBox.Text);
                return result;
            }
            Debug.WriteLine(string.Format(@"(Text:""{0}"") Reset", textBox.Text));
            return Binding.DoNothing;
        }
    }
}
