namespace Gu.Wpf.Validation.Demo.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    using Gu.Wpf.Validation.Demo.Misc;

    public class SampleStructToNullableDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var sampleStruct = value as SampleStruct?;
            if (sampleStruct == null || sampleStruct.Value.Value == 0)
            {
                return null;
            }
            return sampleStruct.Value.Value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var d = value as double?;
            if (d == null)
            {
                return new SampleStruct(0);
            }
            return new SampleStruct(d.Value);
        }
    }
}
