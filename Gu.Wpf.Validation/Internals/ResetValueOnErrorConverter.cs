namespace Gu.Wpf.Validation.Internals
{
    using System;
    using System.Globalization;
    using System.Windows.Controls;
    using System.Windows.Data;

    public class ResetValueOnErrorConverter : IValueConverter
    {
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var textBox = (TextBox)parameter;
            if (Equals(value, true))
            {
                textBox.ResetValue();
            }
            return value;
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
