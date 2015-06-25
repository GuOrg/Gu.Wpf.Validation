namespace Gu.Wpf.Validation.Internals
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    public class FlagsConverter : IMultiValueConverter
    {
        private readonly DependencyProperty _property;

        public FlagsConverter(DependencyProperty property)
        {
            _property = property;
        }

        public virtual object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            var textBox = (TextBox)parameter;
            var flags = textBox.GetValue(_property) as Flags;
            if (flags == null)
            {
                //Debug.WriteLine(@"{0}.Convert({1}) -> UpdateSource(textBox) (flags == null) textBox.Text: {2}", GetType().PrettyName(), value.ToDebugString(), textBox.Text.ToDebugString());
                return new Flags(value);
            }

            var newFlags = flags.Update(value);
            if (!Equals(flags, newFlags))
            {
                //Debug.WriteLine(@"{0}.Convert({1}) -> UpdateSource(textBox) (!Equals(flags, newFlags)) textBox.Text: {2}", GetType().PrettyName(), value.ToDebugString(), textBox.Text.ToDebugString());
            }
            return newFlags;
        }

        public virtual object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException();
        }
    }
}