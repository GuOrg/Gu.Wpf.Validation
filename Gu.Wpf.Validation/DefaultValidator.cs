namespace Gu.Wpf.Validation
{
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    using Gu.Wpf.Validation.Internals;

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

        private static readonly RoutedEventHandler OnLoadedHandler = new RoutedEventHandler(OnLoaded);

        public virtual void Bind(TextBox textBox)
        {
            if (textBox == null)
            {
                return;
            }

            if (textBox.IsLoaded)
            {
                ClearBindings(textBox);
                AddBindings(textBox);
            }
            else
            {
                textBox.UpdateHandler(FrameworkElement.LoadedEvent, OnLoadedHandler);
            }
        }

        private void AddBindings(TextBox textBox)
        {
            RawValueTracker.TrackUserInput(textBox);

            BindTextToValue(textBox);

            BindResetOnErrors(textBox);

            BindUpdateValidation(textBox);
        }

        protected virtual void ClearBindings(TextBox textBox)
        {
            BindingOperations.ClearBinding(textBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(textBox, HasErrorProxyProperty);
            BindingOperations.ClearBinding(textBox, UpdateValidationFlagsProperty);
        }

        protected virtual void BindTextToValue(TextBox textBox)
        {
            var valueBinding = CreateBinding(
                textBox,
                BindingMode.TwoWay,
                textBox.GetValidationTrigger(),
                ValuePath,
                TextToValueConverter);
            var rules = textBox.GetValidationRules();
            foreach (var rule in rules)
            {
                valueBinding.ValidationRules.Add(rule);
            }
            BindingOperations.SetBinding(textBox, TextBox.TextProperty, valueBinding);
        }

        /// <summary>
        ///  Using a binding to reset Value on validation error, nonstandard
        /// </summary>
        /// <param name="textBox"></param>
        protected virtual void BindResetOnErrors(TextBox textBox)
        {
            var binding = CreateBinding(textBox, BindingMode.OneWay, HasErrorPath, ResetOnErrorConverter);
            BindingOperations.SetBinding(textBox, HasErrorProxyProperty, binding);
        }

        protected virtual void BindUpdateValidation(TextBox textBox)
        {
            var binding = new MultiBinding();
            //binding.Bindings.Add(CreateBinding(textBox, BindingMode.OneWay, RawTextPath));
            binding.Bindings.Add(CreateBinding(textBox, BindingMode.OneWay, RawValuePath));
            binding.Bindings.Add(CreateBinding(textBox, BindingMode.OneWay, CulturePath));
            binding.Bindings.Add(CreateBinding(textBox, BindingMode.OneWay, NumberStylesPath));
            binding.Bindings.Add(CreateBinding(textBox, BindingMode.OneWay, PatternPath));
            binding.Bindings.Add(CreateBinding(textBox, BindingMode.OneWay, IsRequiredPath));
            binding.Bindings.Add(CreateBinding(textBox, BindingMode.OneWay, MinPath));
            binding.Bindings.Add(CreateBinding(textBox, BindingMode.OneWay, MaxPath));
            binding.ConverterParameter = textBox;
            binding.Converter = UpdateValidationConverter;
            BindingOperations.SetBinding(textBox, UpdateValidationFlagsProperty, binding);
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

        private static void OnLoaded(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            textBox.RemoveHandler(FrameworkElement.LoadedEvent, OnLoadedHandler);
            var validator = textBox.GetValidator() as DefaultValidator;
            if (validator != null)
            {
                validator.AddBindings(textBox);
            }
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
            var hadError = expression.HasError;
            expression.SetNeedsValidation(true);
            expression.ValidateWithoutUpdate();
            if (hadError)
            {
                if (!expression.HasError)
                {
                    expression.UpdateSource();
                }
            }
        }
    }
}
