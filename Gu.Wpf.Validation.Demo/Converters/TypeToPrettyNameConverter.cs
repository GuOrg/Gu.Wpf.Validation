namespace Gu.Wpf.Validation.Demo.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    using Gu.Wpf.Validation.Internals;

    public class TypeToPrettyNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = value as Type;
            if (type == null)
            {
                return "null";
            }
            return type.PrettyName();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
