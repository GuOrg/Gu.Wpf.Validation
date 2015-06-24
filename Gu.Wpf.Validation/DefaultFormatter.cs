namespace Gu.Wpf.Validation
{
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Data;

    using Gu.Wpf.Validation.Internals;

    public class DefaultFormatter : IFormatter
    {
        protected static readonly PropertyPath IsKeyboardFocusedPath = new PropertyPath(UIElement.IsKeyboardFocusedProperty);
        protected static readonly PropertyPath DecimalDigitsPath = new PropertyPath(Input.DecimalDigitsProperty);
        protected static readonly PropertyPath CulturePath = new PropertyPath(Input.CultureProperty);
        //protected static readonly PropertyPath ValuePath = new PropertyPath(Input.ValueProperty);
        protected static readonly PropertyPath RawTextPath = new PropertyPath(RawValueTracker.RawTextProperty);

        protected static readonly UpdateFormattingConverter UpdateFormattingConverter = new UpdateFormattingConverter();

        protected static readonly DependencyProperty UpdateFormattingProxyProperty = DependencyProperty.RegisterAttached(
            "UpdateFormattingProxy",
            typeof(object),
            typeof(DefaultFormatter),
            new PropertyMetadata(null));

        public virtual void Bind(System.Windows.Controls.TextBox textBox)
        {
            if (textBox == null)
            {
                return;
            }
            BindingOperations.ClearBinding(textBox, UpdateFormattingProxyProperty);

            // Using a binding to update formatting
            var binding = new MultiBinding
                              {
                                  Converter = UpdateFormattingConverter,
                                  ConverterParameter = textBox
                              };
            binding.Bindings.Add(CreateBinding(textBox, IsKeyboardFocusedPath));
            binding.Bindings.Add(CreateBinding(textBox, DecimalDigitsPath));
            binding.Bindings.Add(CreateBinding(textBox, CulturePath));
            binding.Bindings.Add(CreateBinding(textBox, RawTextPath));
            // Using a binding with converter to update formatting
            BindingOperations.SetBinding(textBox, UpdateFormattingProxyProperty, binding);
        }

        protected virtual Binding CreateBinding(
            System.Windows.Controls.TextBox source,
            PropertyPath path)
        {
            return new Binding
            {
                Path = path,
                Source = source,
                Mode = BindingMode.OneWay,
                ConverterParameter = source,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
        }
    }
}
