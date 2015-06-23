namespace Gu.Wpf.Validation.Demo.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Controls;
    using System.Windows.Data;

    public class TextBoxChildConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var groupBox = value as GroupBox;
            if (groupBox == null)
            {
                return null;
            }
            var stackPanel = groupBox.Content as StackPanel;
            if (stackPanel != null)
            {
                return stackPanel.Children[0] as TextBox;
            }
            return groupBox.Content as TextBox;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
