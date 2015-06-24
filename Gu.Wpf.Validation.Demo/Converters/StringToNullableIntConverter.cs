namespace Gu.Wpf.Validation.Demo.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class StringToNullableIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = value as string;
            if (string.IsNullOrEmpty(s))
            {
                return null;
            }
            return int.Parse(s);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            return value.ToString();
        }
    }
}