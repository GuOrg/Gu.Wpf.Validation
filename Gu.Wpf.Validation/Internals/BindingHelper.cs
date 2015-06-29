namespace Gu.Wpf.Validation.Internals
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    internal class BindingHelper
    {
        private static readonly Dictionary<DependencyProperty, PropertyPath> PropertyPaths =
            new Dictionary<DependencyProperty, PropertyPath>();


        internal static BindingExpression Bind(
            DependencyObject target,
            DependencyProperty targetProperty,
            object source,
            DependencyProperty sourceProperty)
        {
            return Bind(target, targetProperty, source, GetPath(sourceProperty));
        }


        internal static BindingExpression Bind(
            DependencyObject target,
            DependencyProperty targetProperty,
            object source,
            PropertyPath path)
        {
            var binding = new Binding
                              {
                                  Path = path,
                                  Source = source,
                                  Mode = BindingMode.OneWay,
                                  UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                              };
            return (BindingExpression)BindingOperations.SetBinding(target, targetProperty, binding);
        }

        internal static Binding CreateBinding(
            TextBox source,
            BindingMode mode,
            DependencyProperty property,
            IValueConverter converter)
        {
            return CreateBinding(source, mode, UpdateSourceTrigger.PropertyChanged, property, converter);
        }

        internal static Binding CreateBinding(
            TextBox source,
            BindingMode mode,
            UpdateSourceTrigger trigger,
            DependencyProperty property,
            IValueConverter converter)
        {
            return new Binding
                       {
                           Path = GetPath(property),
                           Source = source,
                           Mode = mode,
                           UpdateSourceTrigger = trigger,
                           Converter = converter,
                           ConverterParameter = source
                       };
        }

        internal static Binding CreateBinding(TextBox source, BindingMode mode, DependencyProperty property)
        {
            return CreateBinding(source, mode, UpdateSourceTrigger.PropertyChanged, property);
        }

        internal static Binding CreateBinding(
            TextBox source,
            BindingMode mode,
            UpdateSourceTrigger updateSourceTrigger,
            DependencyProperty property)
        {
            return new Binding
                       {
                           Path = GetPath(property),
                           Source = source,
                           Mode = mode,
                           ConverterParameter = source,
                           UpdateSourceTrigger = updateSourceTrigger
                       };
        }

        internal static PropertyPath GetPath(DependencyProperty property)
        {
            PropertyPath path;
            if (!PropertyPaths.TryGetValue(property, out path))
            {
                path = new PropertyPath(property);
                PropertyPaths[property] = path;
            }
            return path;
        }
    }
}
