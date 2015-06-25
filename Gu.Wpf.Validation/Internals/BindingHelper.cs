namespace Gu.Wpf.Validation.Internals
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    public class BindingHelper
    {
        public static Binding CreateBinding(
            TextBox source,
            BindingMode mode,
            PropertyPath path,
            IValueConverter converter)
        {
            return CreateBinding(source, mode, UpdateSourceTrigger.PropertyChanged, path, converter);
        }

        public static Binding CreateBinding(
            TextBox source,
            BindingMode mode,
            UpdateSourceTrigger trigger,
            PropertyPath path,
            IValueConverter converter)
        {
            return new Binding
            {
                Path = path,
                Source = source,
                Mode = mode,
                UpdateSourceTrigger = trigger,
                Converter = converter,
                ConverterParameter = source
            };
        }

        public static Binding CreateBinding(
            TextBox source,
            BindingMode mode,
            PropertyPath path)
        {
            return CreateBinding(source, mode, UpdateSourceTrigger.PropertyChanged, path);
        }

        public static Binding CreateBinding(
            TextBox source,
            BindingMode mode,
            UpdateSourceTrigger updateSourceTrigger,
            PropertyPath path)
        {
            return new Binding
            {
                Path = path,
                Source = source,
                Mode = mode,
                ConverterParameter = source,
                UpdateSourceTrigger = updateSourceTrigger
            };
        }
    }
}
