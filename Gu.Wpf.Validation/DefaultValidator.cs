namespace Gu.Wpf.Validation
{
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    using Gu.Wpf.Validation.Internals;

    public class DefaultValidator : IValidator
    {
        protected static readonly TextToValueConverter TextToValueConverter = new TextToValueConverter();

        protected static readonly RoutedEventHandler OnLoadedHandler = OnLoaded;
        protected static readonly RoutedEventHandler OnValidationDirtyHandler = OnValidationDirty;

        public virtual void Bind(TextBox textBox)
        {
            if (textBox == null)
            {
                return;
            }

            if (textBox.IsLoaded)
            {
                AddHandlers(textBox);
                UpdateValidation(textBox);
            }
            else
            {
                textBox.UpdateHandler(FrameworkElement.LoadedEvent, OnLoadedHandler);
            }
        }

        protected virtual void AddHandlers(TextBox textBox)
        {
            RawValueTracker.TrackUserInput(textBox);
            textBox.UpdateHandler(Input.ValidationDirtyEvent, OnValidationDirtyHandler);
            textBox.UpdateHandler(FrameworkElement.LoadedEvent, OnValidationDirtyHandler);
            BindTextToValue(textBox);
        }

        protected virtual void RemoveHandlers(TextBox textBox)
        {
            BindingOperations.ClearBinding(textBox, TextBox.TextProperty);
        }

        protected virtual void BindTextToValue(TextBox textBox)
        {
            var binding = BindingHelper.CreateBinding(
                textBox,
                BindingMode.TwoWay,
                textBox.GetValidationTrigger(),
                Input.ValueProperty,
                TextToValueConverter);
            var rules = textBox.GetValidationRules();
            foreach (var rule in rules)
            {
                binding.ValidationRules.Add(rule);
            }
            BindingOperations.SetBinding(textBox, TextBox.TextProperty, binding);
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
                validator.AddHandlers(textBox);
                validator.UpdateValidation(textBox);
            }
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
