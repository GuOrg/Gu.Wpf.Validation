namespace Gu.Wpf.Validation.Internals
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Windows.Controls;
    using System.Windows.Data;

    public class UpdateValidationConverter : IMultiValueConverter
    {
        public virtual object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            Debug.WriteLine(string.Format(@"UpdateValidationConverter.Convert({0}) ", value));
            var textBox = (TextBox)parameter;
            if (textBox == null)
            {
                return value;
            }
            if (textBox.GetIsUpdating())
            {
                return value;
            }
            var expression = BindingOperations.GetBindingExpressionBase(textBox, TextBox.TextProperty);
            if (expression != null)
            {
                textBox.SetIsUpdating(true);
                expression.UpdateSource();
                textBox.SetIsUpdating(false);
            }
            return value;
        }

        public virtual object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException();
        }
    }
}