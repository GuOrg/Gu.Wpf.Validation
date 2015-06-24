namespace Gu.Wpf.Validation
{
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    using Gu.Wpf.Validation.Rules;

    using Internals;

    public class DefaultValidator : IValidator
    {
        /// <summary>
        /// Proxy property used for binding HasError to enable resetting Value on error
        /// </summary>
        protected static readonly DependencyProperty HasErrorProxyProperty = DependencyProperty.RegisterAttached(
            "HasErrorProxy",
            typeof(bool),
            typeof(DefaultValidator),
            new PropertyMetadata(false));

        internal static readonly DependencyProperty UpdateValidationFlagsProperty = DependencyProperty.RegisterAttached(
            "UpdateValidationFlags",
            typeof(UpdateValidationFlags),
            typeof(DefaultValidator),
            new PropertyMetadata(null, OnUpdateValidationFlagsChanged));

        protected static readonly PropertyPath ValuePath = new PropertyPath(Input.ValueProperty);
        protected static readonly PropertyPath CulturePath = new PropertyPath(Input.CultureProperty);
        protected static readonly PropertyPath NumberStylesPath = new PropertyPath(Input.NumberStylesProperty);
        protected static readonly PropertyPath MinPath = new PropertyPath(Input.MinProperty);
        protected static readonly PropertyPath MaxPath = new PropertyPath(Input.MaxProperty);
        protected static readonly PropertyPath PatternPath = new PropertyPath(Input.PatternProperty);
        protected static readonly PropertyPath IsRequiredPath = new PropertyPath(Input.IsRequiredProperty);
        protected static readonly PropertyPath TextPath = new PropertyPath(TextBox.TextProperty);
        protected static readonly PropertyPath RawValuePath = new PropertyPath(RawValueTracker.RawValueProperty);
        protected static readonly PropertyPath RawTextPath = new PropertyPath(RawValueTracker.RawTextProperty);
        protected static readonly PropertyPath HasErrorPath = new PropertyPath(Validation.HasErrorProperty);
        protected static readonly PropertyPath UpdateValidationFlagsPath = new PropertyPath(UpdateValidationFlagsProperty);
        protected static readonly TextToValueConverter TextToValueConverter = new TextToValueConverter();
        protected static readonly OnErrorConverter ResetOnErrorConverter = new OnErrorConverter();
        protected static readonly UpdateValidationConverter UpdateValidationConverter = new UpdateValidationConverter();

        public virtual void Bind(TextBox textBox)
        {
            if (textBox == null)
            {
                return;
            }

            BindingOperations.ClearBinding(textBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(textBox, HasErrorProxyProperty);
            BindingOperations.ClearBinding(textBox, UpdateValidationFlagsProperty);

            RawValueTracker.TrackUserInput(textBox);

            var valueBinding = CreateBinding(textBox, BindingMode.TwoWay, textBox.GetValidationTrigger(), ValuePath, TextToValueConverter);
            var rules = textBox.GetValidationRules();
            foreach (var rule in rules)
            {
                valueBinding.ValidationRules.Add(rule);
            }
            BindingOperations.SetBinding(textBox, TextBox.TextProperty, valueBinding);

            // Using a binding to reset Value on validation error, nonstandard
            var hasErrorBinding = CreateBinding(textBox, BindingMode.OneWay, HasErrorPath, ResetOnErrorConverter);
            BindingOperations.SetBinding(textBox, HasErrorProxyProperty, hasErrorBinding);

            var validationDirtyBinding = new MultiBinding();
            validationDirtyBinding.Bindings.Add(CreateBinding(textBox, BindingMode.OneWay, RawTextPath));
            validationDirtyBinding.Bindings.Add(CreateBinding(textBox, BindingMode.OneWay, RawValuePath));
            validationDirtyBinding.Bindings.Add(CreateBinding(textBox, BindingMode.OneWay, CulturePath));
            validationDirtyBinding.Bindings.Add(CreateBinding(textBox, BindingMode.OneWay, NumberStylesPath));
            validationDirtyBinding.Bindings.Add(CreateBinding(textBox, BindingMode.OneWay, PatternPath));
            validationDirtyBinding.Bindings.Add(CreateBinding(textBox, BindingMode.OneWay, IsRequiredPath));
            validationDirtyBinding.Bindings.Add(CreateBinding(textBox, BindingMode.OneWay, MinPath));
            validationDirtyBinding.Bindings.Add(CreateBinding(textBox, BindingMode.OneWay, MaxPath));
            validationDirtyBinding.ConverterParameter = textBox;
            validationDirtyBinding.Converter = UpdateValidationConverter;
            BindingOperations.SetBinding(textBox, UpdateValidationFlagsProperty, validationDirtyBinding);
        }

        protected virtual Binding CreateBinding(
            TextBox source,
            BindingMode mode,
            PropertyPath path,
            IValueConverter converter)
        {
            return CreateBinding(source, mode, UpdateSourceTrigger.PropertyChanged, path, converter);
        }

        protected virtual Binding CreateBinding(
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

        protected virtual Binding CreateBinding(
            TextBox source,
            BindingMode mode,
            PropertyPath path)
        {
            return CreateBinding(source, mode, UpdateSourceTrigger.PropertyChanged, path);
        }

        protected virtual Binding CreateBinding(
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

        private static void OnUpdateValidationFlagsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = (TextBox)d;
            UpdateValidation(textBox);
        }

        private static void UpdateValidation(TextBox textBox)
        {
            Debug.WriteLine(string.Format(@"DefaultValidator.UpdateValidation() textBox.Text: {0}", textBox.Text.ToDebugString()));
            var expression = BindingOperations.GetBindingExpression(textBox, TextBox.TextProperty);
            if (expression == null || textBox.GetIsUpdating())
            {
                return;
            }
            textBox.SetIsUpdating(true);
            expression.UpdateSource();
            expression.ValidateWithoutUpdate();
            textBox.SetIsUpdating(false);
        }
    }
}
