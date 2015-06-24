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
            var textBox = parameter as TextBox;
            if (textBox == null)
            {
                //Debug.WriteLine(@"{0} (textBox == null)", value.ToDebugString());
                return value;
            }
            if (Equals(value, true))
            {
                Debug.WriteLine(@"{0}.Convert({1}) ->  ResetValue() textBox.Text: ""{2}""", GetType().PrettyName(), value.ToDebugString(), textBox.Text.ToDebugString());
                textBox.SetIsUpdating(true);
                textBox.ResetValue();
                textBox.SetIsUpdating(false);
            }
            else if (!textBox.IsKeyboardFocused)
            {
                var rawValue = textBox.GetRawValue();
                if (rawValue != RawValueTracker.Unset && rawValue != textBox.GetValue(Input.ValueProperty))
                {
                    Debug.WriteLine(@"{0}.Convert({1}) textBox.SetCurrentValue(Input.ValueProperty, {2}) (textBox.Text: ""{3}"")", GetType().PrettyName(), value.ToDebugString(), rawValue.ToDebugString(), textBox.Text.ToDebugString());
                    textBox.SetIsUpdating(true);
                    textBox.SetCurrentValue(Input.ValueProperty, rawValue);
                    textBox.RaiseEvent(new RoutedEventArgs(UIElement.LostFocusEvent)); // dunno if this is a good idea.
                    textBox.SetIsUpdating(false);
                }
            }
            //Debug.WriteLine("");
            return value;
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
