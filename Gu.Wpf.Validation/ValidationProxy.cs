namespace Gu.Wpf.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    public sealed class ValidationProxy : DependencyObject, IDisposable
    {
        private static readonly PropertyPath ControlHasErrorPath = new PropertyPath(System.Windows.Controls.Validation.HasErrorProperty);
        private static readonly PropertyPath ScopeHasErrorPath = new PropertyPath(ValidationExt.HasErrorProperty);
        private static readonly HasErrorsConverter Converter = new HasErrorsConverter();

        public static readonly DependencyProperty HasErrorProxyProperty = DependencyProperty.Register(
            "HasErrorProxy",
            typeof(bool),
            typeof(ValidationProxy),
            new PropertyMetadata(
                false,
                OnHasErrorChanged));

        public static readonly DependencyProperty HasErrorProperty = ValidationExt.HasErrorProperty.AddOwner(typeof(ValidationProxy));

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

            var scopeHasErrorBinding = new Binding
            {
                Path = ScopeHasErrorPath,
                Source = child,
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            var multiBinding = new MultiBinding();
            multiBinding.Bindings.Add(_controlHasErrorBinding);
            multiBinding.Bindings.Add(scopeHasErrorBinding);
            multiBinding.Converter = Converter;
            BindingOperations.SetBinding(this, HasErrorProxyProperty, multiBinding);
        }

        public event EventHandler HasErrorChanged;

        public FrameworkElement Element
        {
            get { return _controlHasErrorBinding.Source as FrameworkElement; }
        }

        public bool HasError
        {
            get { return (bool)GetValue(HasErrorProperty); }
            set { SetValue(ValidationExt.HasErrorPropertyKey, value); }
        }

        public IEnumerable<ValidationError> Errors
        {
            get
            {
                var source = _controlHasErrorBinding.Source as DependencyObject;
                if (source == null)
                {
                    return EmptyErrors;
                }
                return System.Windows.Controls.Validation.GetErrors(source);
            }
        }

        public void Dispose()
        {
            BindingOperations.ClearBinding(this, HasErrorProperty);
        }

        private static void OnHasErrorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var validationProxy = (ValidationProxy)d;
            validationProxy.HasError = (bool)e.NewValue;
            validationProxy.OnHasErrorChanged();
        }

        private void OnHasErrorChanged()
        {
            var handler = HasErrorChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        private class HasErrorsConverter : IMultiValueConverter
        {
            public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
            {
                return Equals(values[0], true) || Equals(values[1], true);
            }

            public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
    }
}