namespace Gu.Wpf.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    [DebuggerDisplay("Element {Element} HasEror: {HasError}")]
    public sealed class ValidationProxy : DependencyObject, IDisposable
    {
        private static readonly PropertyPath ControlHasErrorPath = new PropertyPath(Validation.HasErrorProperty);

        private static readonly DependencyProperty HasErrorProxyProperty = DependencyProperty.Register(
            "HasErrorProxy",
            typeof(bool),
            typeof(ValidationProxy),
            new PropertyMetadata(
                false,
                OnHasErrorChanged));

        private readonly Binding _controlHasErrorBinding;
        private static readonly IEnumerable<ValidationError> EmptyErrors = Enumerable.Empty<ValidationError>();

        public ValidationProxy(FrameworkElement child)
        {
            _controlHasErrorBinding = new Binding
            {
                Path = ControlHasErrorPath,
                Source = child,
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            BindingOperations.SetBinding(this, HasErrorProxyProperty, _controlHasErrorBinding);
        }

        public FrameworkElement Element
        {
            get { return _controlHasErrorBinding.Source as FrameworkElement; }
        }

        /// <summary>
        /// Gets the errors for the element
        /// </summary>
        public IEnumerable<ValidationError> Errors
        {
            get
            {
                var source = Element;
                if (source == null)
                {
                    return EmptyErrors;
                }
                var hasError = Validation.GetHasError(source);
                if (!hasError)
                {
                    return EmptyErrors;
                }
                return Validation.GetErrors(source);
            }
        }

        /// <summary>
        /// Gets if the element or the scope has any error
        /// </summary>
        public bool HasError
        {
            get
            {
                var source = Element;
                if (source == null)
                {
                    return false;
                }
                return Validation.GetHasError(source);
            }
        }

        public void Dispose()
        {
            BindingOperations.ClearBinding(this, HasErrorProxyProperty);
        }

        private static void OnHasErrorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var validationProxy = (ValidationProxy)d;
            var element = validationProxy.Element;
            if (element != null && element.GetIsValidationScope() == true)
            {
                element.RaiseEvent(new RoutedEventArgs(Scope.ErrorEvent));
            }
        }
    }
}