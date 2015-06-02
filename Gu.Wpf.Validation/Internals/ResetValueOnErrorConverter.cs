namespace Gu.Wpf.Validation.Internals
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Windows.Controls;
    using System.Windows.Data;

    public class ResetValueOnErrorConverter : IValueConverter
    {
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Debug.Write(string.Format(@"ResetValueOnErrorConverter.Convert({0}) ", value));
            var textBox = parameter as TextBox;
            if (textBox == null)
            {
                Debug.WriteLine("");
                return value;
            }
            if (Equals(value, true))
            {
                Debug.WriteLine("ResetValue");
                textBox.SetIsUpdating(true);
                textBox.ResetValue();
                textBox.SetIsUpdating(false);
            }
            else
            {
                var expression = BindingOperations.GetBindingExpressionBase(textBox, TextBox.TextProperty);
                if (expression != null)
                {
                    Debug.WriteLine("UpdateTarget");
                    expression.UpdateTarget();
                }
            }
            Debug.WriteLine("");
            return value;
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
