namespace Gu.Wpf.Validation
{
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    using Gu.Wpf.Validation.Internals;

    public class DefaultValidator : IValidator
    {
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
        protected static readonly PropertyPath UpdateValidationFlagsPath = new PropertyPath(UpdateValidationFlagsProperty);
        protected static readonly TextToValueConverter TextToValueConverter = new TextToValueConverter();
        protected static readonly UpdateValidationConverter UpdateValidationConverter = new UpdateValidationConverter();

        private static readonly RoutedEventHandler OnLoadedHandler = new RoutedEventHandler(OnLoaded);
        private static readonly RoutedEventHandler OnValidationDirtyHandler = new RoutedEventHandler(OnValidationDirty);

        internal static readonly RoutedEventArgs ValidationDirtyArgs = new RoutedEventArgs(Input.ValidationDirtyEvent);

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
            textBox.UpdateHandler(Input.ValidationDirtyEvent, OnValidationDirtyHandler);
            BindTextToValue(textBox);
            BindUpdateValidation(textBox);
        }

        protected virtual void ClearBindings(TextBox textBox)
        {
            BindingOperations.ClearBinding(textBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(textBox, UpdateValidationFlagsProperty);
        }

        protected virtual void BindTextToValue(TextBox textBox)
        {
            var binding = CreateBinding(
                textBox,
                BindingMode.TwoWay,
                textBox.GetValidationTrigger(),
                ValuePath,
                TextToValueConverter);
            var rules = textBox.GetValidationRules();
            foreach (var rule in rules)
            {
                binding.ValidationRules.Add(rule);
            }
            BindingOperations.SetBinding(textBox, TextBox.TextProperty, binding);
        }

        protected virtual void BindUpdateValidation(TextBox textBox)
        {
            var binding = new MultiBinding();
            binding.Bindings.Add(CreateBinding(textBox, BindingMode.OneWay, RawTextPath));
            binding.Bindings.Add(CreateBinding(textBox, BindingMode.OneWay, RawValuePath));
            //binding.Bindings.Add(CreateBinding(textBox, BindingMode.OneWay, CulturePath));
            //binding.Bindings.Add(CreateBinding(textBox, BindingMode.OneWay, NumberStylesPath));
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

        protected virtual void UpdateValidation(TextBox textBox)
        {
            Debug.WriteLine(string.Format(@"DefaultValidator.UpdateValidation() textBox.Text: {0}", textBox.Text.ToDebugString()));
            var expression = BindingOperations.GetBindingExpression(textBox, TextBox.TextProperty);
            if (expression == null || textBox.GetIsUpdating())
            {
                return;
            }
            var before = expression.HasError;
            if (!expression.HasError)
            {
                expression.SetNeedsValidation(true);
            }
            expression.ValidateWithoutUpdate();
            OnPostValidation(textBox, new Errors(before, expression.HasError));
        }

        protected virtual void OnPostValidation(TextBox textBox, Errors errors)
        {
            var strategy = textBox.GetOnValidationErrorStrategy();

            if ((strategy & OnValidationErrorStrategy.ResetValueOnError) != 0 && errors.After)
            {
                var expression = BindingOperations.GetBindingExpression(textBox, Input.ValueProperty);
                if (expression != null)
                {
                    Debug.WriteLine(
                        @"{0}.OnHasErrorChanged({1}) -> UpdateTarget() textBox.Text: {2}",
                        GetType()
                            .PrettyName(),
                        errors.ToArgumentsString(),
                        textBox.Text.ToDebugString());
                    textBox.SetIsUpdating(true);
                    expression.UpdateTarget(); // Update Input.Value with value from binding
                    textBox.SetIsUpdating(false);
                }
            }

            if ((strategy & OnValidationErrorStrategy.UpdateSourceOnError) != 0 && errors.After)
            {
                var rawValue = textBox.GetRawValue();
                var value = textBox.GetValue(Input.ValueProperty);
                if (rawValue != RawValueTracker.Unset && !Equals(rawValue, value))
                {
                    Debug.WriteLine(
                        @"{0}.OnHasErrorChanged({1}) -> textBox.SetCurrentValue(Input.ValueProperty, {2}) textBox.Text: {3}",
                        GetType()
                            .PrettyName(),
                        errors.ToArgumentsString(),
                        rawValue.ToDebugString(),
                        textBox.Text.ToDebugString());
                    textBox.SetIsUpdating(true);
                    textBox.SetCurrentValue(Input.ValueProperty, rawValue); // think the binding prevents transfer when has errors
                    textBox.SetIsUpdating(false);
                }
            }

            if ((strategy & OnValidationErrorStrategy.UpdateSourceOnSuccess) != 0 && !errors.After)
            {
                var rawValue = textBox.GetRawValue();
                var value = textBox.GetValue(Input.ValueProperty);
                if (rawValue != RawValueTracker.Unset && !Equals(rawValue, value))
                {
                    Debug.WriteLine(@"{0}.OnHasErrorChanged({1}) -> UpdateSource() textBox.Text: {2}", GetType().PrettyName(), errors.ToArgumentsString(), textBox.Text.ToDebugString());
                    textBox.SetIsUpdating(true);
                    textBox.SetCurrentValue(Input.ValueProperty, rawValue);
                    var valueExpression = BindingOperations.GetBindingExpression(textBox, Input.ValueProperty);
                    if (valueExpression != null)
                    {
                        if (valueExpression.ParentBinding.UpdateSourceTrigger != UpdateSourceTrigger.PropertyChanged && !textBox.IsKeyboardFocused)
                        {
                            valueExpression.UpdateSource();
                        }
                    }
                    textBox.SetIsUpdating(false);
                }
            }
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
            if (textBox.GetIsUpdating())
            {
                return;
            }
            textBox.RaiseEvent(ValidationDirtyArgs);
        }

        private static void OnValidationDirty(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            var defaultValidator = textBox.GetValidator() as DefaultValidator;
            if (defaultValidator != null)
            {
                defaultValidator.UpdateValidation(textBox);
            }
        }

        protected struct Errors
        {
            internal readonly bool Before;

            internal readonly bool After;

            public Errors(bool before, bool after)
            {
                Before = before;
                After = after;
            }

            public override string ToString()
            {
                return string.Format("Before: {0}, After: {1}", Before, After);
            }

            public string ToArgumentsString()
            {
                return string.Format("{0}, {1}", Before, After);
            }
        }
    }
}
