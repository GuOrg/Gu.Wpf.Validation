namespace Gu.Wpf.Validation.Demo
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class NullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "null";
            }
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = value as string;
            if (string.IsNullOrEmpty(s) || string.Equals(s, "null", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }
            return System.Convert.ChangeType(value, targetType, culture);
        }
    }
}
