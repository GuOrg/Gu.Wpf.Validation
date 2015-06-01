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
        protected static readonly PropertyPath DecimalDigitsPath = new PropertyPath(Input.DecimalDigitsProperty);
        protected static readonly PropertyPath MinPath = new PropertyPath(Input.MinProperty);
        protected static readonly PropertyPath MaxPath = new PropertyPath(Input.MaxProperty);
        protected static readonly PropertyPath PatternPath = new PropertyPath(Input.PatternProperty);
        protected static readonly PropertyPath IsRequiredPath = new PropertyPath(Input.IsRequiredProperty);
        protected static readonly PropertyPath IsKeyboardFocusedPath = new PropertyPath(UIElement.IsKeyboardFocusedProperty);
        protected static readonly PropertyPath TextPath = new PropertyPath(TextBox.TextProperty);
        protected static readonly PropertyPath IsDirtyPath = new PropertyPath(TextBoxExt.IsDirtyProperty);
        protected static readonly PropertyPath HasErrorPath = new PropertyPath(Validation.HasErrorProperty);
        protected static readonly StringFormatConverter StringFormatConverter = new StringFormatConverter();
        protected static readonly ResetValueOnErrorConverter ResetOnErrorConverter = new ResetValueOnErrorConverter();
        protected static readonly UpdateConverter UpdateConverter = new UpdateConverter();

        /// <summary>
        /// Proxy property used for binding HasError to enable resetting Value on error
        /// </summary>
        protected static readonly DependencyProperty HasErrorProxyProperty = DependencyProperty.RegisterAttached(
            "HasErrorProxy",
            typeof(bool),
            typeof(DefaultValidator),
            new PropertyMetadata(false));

        protected static readonly DependencyProperty IsKeyboardFocusedProxyProperty = DependencyProperty.RegisterAttached(
            "IsKeyboardFocusedProxy",
            typeof(bool),
            typeof(DefaultValidator),
            new PropertyMetadata(false));

        public virtual void Bind(TextBox textBox)
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
            BindingOperations.SetBinding(textBox, TextBox.TextProperty, textBinding);

            // Using a binding to reset Value on validation error, nonstandard
            var hasErrorBinding = CreateBinding(textBox, BindingMode.OneWay, HasErrorPath, ResetOnErrorConverter);
            BindingOperations.SetBinding(textBox, HasErrorProxyProperty, hasErrorBinding);

            // Using a binding to update formatting
            var updateFormattingBinding = new MultiBinding { Delay = 10 };
            updateFormattingBinding.Bindings.Add(CreateBinding(textBox, BindingMode.OneWay, IsKeyboardFocusedPath));
            updateFormattingBinding.Bindings.Add(CreateBinding(textBox, BindingMode.OneWay, CulturePath));
            updateFormattingBinding.Bindings.Add(CreateBinding(textBox, BindingMode.OneWay, NumberStylesPath));
            updateFormattingBinding.Bindings.Add(CreateBinding(textBox, BindingMode.OneWay, DecimalDigitsPath));
            updateFormattingBinding.Bindings.Add(CreateBinding(textBox, BindingMode.OneWay, PatternPath));
            updateFormattingBinding.Bindings.Add(CreateBinding(textBox, BindingMode.OneWay, IsRequiredPath));
            updateFormattingBinding.Bindings.Add(CreateBinding(textBox, BindingMode.OneWay, MinPath));
            updateFormattingBinding.Bindings.Add(CreateBinding(textBox, BindingMode.OneWay, MaxPath));
            updateFormattingBinding.ConverterParameter = textBox;
            updateFormattingBinding.Converter = UpdateConverter;
            BindingOperations.SetBinding(textBox, IsKeyboardFocusedProxyProperty, updateFormattingBinding);
        }

        protected static void SetHasErrorProxy(DependencyObject element, bool value)
        {
            element.SetValue(HasErrorProxyProperty, value);
        }

        protected static bool GetHasErrorProxy(DependencyObject element)
        {
            return (bool)element.GetValue(HasErrorProxyProperty);
        }

        protected static void SetIsKeyboardFocusedProxy(DependencyObject element, bool value)
        {
            element.SetValue(IsKeyboardFocusedProxyProperty, value);
        }

        protected static bool GetIsKeyboardFocusedProxy(DependencyObject element)
        {
            return (bool)element.GetValue(IsKeyboardFocusedProxyProperty);
        }

        private Binding CreateBinding(
            TextBox source,
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

        private Binding CreateBinding(
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

        protected static Binding CreateBinding(TextBox source, BindingMode mode, PropertyPath path)
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
