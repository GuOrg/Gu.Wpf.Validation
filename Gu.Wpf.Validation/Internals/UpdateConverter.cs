namespace Gu.Wpf.Validation.Internals
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    public class UpdateConverter : IMultiValueConverter
    {
        public virtual object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            var textBox = (TextBox)parameter;
            if (!textBox.IsKeyboardFocused)
            {
                var hasError = (bool)textBox.GetValue(Validation.HasErrorProperty);
                textBox.SetIsUpdating(true);
                var expression = BindingOperations.GetBindingExpressionBase(textBox, TextBox.TextProperty);
                expression.UpdateSource();
                textBox.SetIsUpdating(false);
                var hasErrorAfter = (bool)textBox.GetValue(Validation.HasErrorProperty);
                if (hasError && !hasErrorAfter)
                {
                    expression.UpdateSource();
                }
                if (!hasErrorAfter)
                {
                    var converter = textBox.GetStringConverter();
                    if (converter != null)
                    {
                        var formatted = converter.ToString(textBox.GetValue(Input.ValueProperty), textBox);
                        if (formatted != textBox.Text)
                        {
                            textBox.SetIsUpdating(true);
                            textBox.SetTextUndoable(formatted);
                            textBox.SetIsUpdating(false);
                        }
                    }
                }
            }
            return value[0];
        }

        public virtual object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException();
        }
    }
}