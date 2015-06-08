namespace Gu.Wpf.Validation
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    using Gu.Wpf.Validation.Internals;

    public class DefaultValidator : IValidator
    {
        protected static readonly PropertyPath ValuePath = new PropertyPath(Input.ValueProperty);
        protected static readonly PropertyPath CulturePath = new PropertyPath(Input.CultureProperty);
        protected static readonly PropertyPath NumberStylesPath = new PropertyPath(Input.NumberStylesProperty);
        protected static readonly PropertyPath MinPath = new PropertyPath(Input.MinProperty);
        protected static readonly PropertyPath MaxPath = new PropertyPath(Input.MaxProperty);
        protected static readonly PropertyPath PatternPath = new PropertyPath(Input.PatternProperty);
        protected static readonly PropertyPath IsRequiredPath = new PropertyPath(Input.IsRequiredProperty);
        protected static readonly PropertyPath TextPath = new PropertyPath(System.Windows.Controls.TextBox.TextProperty);
        protected static readonly PropertyPath HasErrorPath = new PropertyPath(Validation.HasErrorProperty);
        protected static readonly StringFormatConverter StringFormatConverter = new StringFormatConverter();
        protected static readonly OnErrorConverter ResetOnErrorConverter = new OnErrorConverter();
        protected static readonly UpdateValidationConverter UpdateValidationConverter = new UpdateValidationConverter();

        /// <summary>
        /// Proxy property used for binding HasError to enable resetting Value on error
        /// </summary>
        protected static readonly DependencyProperty HasErrorProxyProperty = DependencyProperty.RegisterAttached(
            "HasErrorProxy",
            typeof(bool),
            typeof(DefaultValidator),
            new PropertyMetadata(false));

        protected static readonly DependencyProperty UpdateValidationProxyProperty = DependencyProperty.RegisterAttached(
            "UpdateValidationProxy",
            typeof(object[]),
            typeof(DefaultValidator),
            new PropertyMetadata(null));

        public virtual void Bind(System.Windows.Controls.TextBox textBox)
        {
            if (textBox == null)
            {
                return;
            }

            var textBinding = CreateBinding(textBox, BindingMode.TwoWay, textBox.GetValidationTrigger(), ValuePath, StringFormatConverter);
            var rules = textBox.GetValidationRules();
            foreach (var rule in rules)
            {
                textBinding.ValidationRules.Add(rule);
            }
            BindingOperations.SetBinding(textBox, System.Windows.Controls.TextBox.TextProperty, textBinding);

            // Using a binding to reset Value on validation error, nonstandard
            var hasErrorBinding = CreateBinding(textBox, BindingMode.OneWay, HasErrorPath, ResetOnErrorConverter);
            BindingOperations.SetBinding(textBox, HasErrorProxyProperty, hasErrorBinding);

            var updateValidationBinding = new MultiBinding { Delay = 10 };
            updateValidationBinding.Bindings.Add(CreateBinding(textBox, BindingMode.OneWay, TextPath));
            updateValidationBinding.Bindings.Add(CreateBinding(textBox, BindingMode.OneWay, CulturePath));
            updateValidationBinding.Bindings.Add(CreateBinding(textBox, BindingMode.OneWay, NumberStylesPath));
            updateValidationBinding.Bindings.Add(CreateBinding(textBox, BindingMode.OneWay, PatternPath));
            updateValidationBinding.Bindings.Add(CreateBinding(textBox, BindingMode.OneWay, IsRequiredPath));
            updateValidationBinding.Bindings.Add(CreateBinding(textBox, BindingMode.OneWay, MinPath));
            updateValidationBinding.Bindings.Add(CreateBinding(textBox, BindingMode.OneWay, MaxPath));
            updateValidationBinding.ConverterParameter = textBox;
            updateValidationBinding.Converter = UpdateValidationConverter;
            BindingOperations.SetBinding(textBox, UpdateValidationProxyProperty, updateValidationBinding);
        }

        protected virtual Binding CreateBinding(
            System.Windows.Controls.TextBox source,
            BindingMode mode,
            PropertyPath path,
            IValueConverter converter)
        {
            return new Binding
            {
                Path = path,
                Source = source,
                Mode = mode,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                Converter = converter,
                ConverterParameter = source
            };
        }

        protected virtual Binding CreateBinding(
            System.Windows.Controls.TextBox source,
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

        protected virtual Binding CreateBinding(
            System.Windows.Controls.TextBox source, 
            BindingMode mode, 
            PropertyPath path)
        {
            return new Binding
                    {
                        Path = path,
                        Source = source,
                        Mode = mode,
                        ConverterParameter = source,
                        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                    };
        }
    }
}
