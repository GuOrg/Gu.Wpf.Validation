namespace Gu.Wpf.Validation.Internals
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    public class OnErrorConverter : IValueConverter
    {
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Debug.Write(string.Format(@"OnErrorConverter.Convert({0}) ", value));
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
            else if (!textBox.IsKeyboardFocused)
            {
                var rawValue = textBox.GetRawValue();
                if (rawValue != RawValueTracker.Unset)
                {
                    textBox.SetIsUpdating(true);
                    Debug.WriteLine("UpdateSource");
                    textBox.SetCurrentValue(Input.ValueProperty, rawValue);
                    textBox.RaiseEvent(new RoutedEventArgs(UIElement.LostFocusEvent)); // dunno if this is a good idea.
                    textBox.SetIsUpdating(false);
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
