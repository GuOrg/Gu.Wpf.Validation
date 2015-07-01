namespace Gu.Wpf.Validation
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    public static class Scope
    {
        public static readonly RoutedEvent ErrorEvent = EventManager.RegisterRoutedEvent(
            "Error",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(Scope));

        public static readonly DependencyProperty IsValidationScopeProperty = DependencyProperty.RegisterAttached(
            "IsValidationScope",
            typeof(bool?),
            typeof(Scope),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.Inherits,
                OnIsValidationScopeChanged));

        private static readonly DependencyPropertyKey ErrorProxiesPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "ErrorProxies",
            typeof(ErrorProxyCollection),
            typeof(Scope),
            new PropertyMetadata(null));

        public static readonly DependencyProperty ErrorProxiesProperty = ErrorProxiesPropertyKey.DependencyProperty;

        private static readonly DependencyProperty ValidationProxyProperty = DependencyProperty.RegisterAttached(
            "ValidationProxy",
            typeof(ValidationProxy),
            typeof(Scope),
            new PropertyMetadata(null));

        internal static readonly DependencyPropertyKey HasErrorPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "HasError",
            typeof(bool),
            typeof(Scope),
            new PropertyMetadata(false, null, OnHasErrorCoerce));

        public static readonly DependencyProperty HasErrorProperty = HasErrorPropertyKey.DependencyProperty;

        private static readonly ValidationError[] EmptyErrorCollection = new ValidationError[0];
        private static readonly DependencyPropertyKey ErrorsPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "Errors",
            typeof(IEnumerable<ValidationError>),
            typeof(Scope),
            new PropertyMetadata(EmptyErrorCollection, null, OnErrorsCoerce));

        public static readonly DependencyProperty ErrorsProperty = ErrorsPropertyKey.DependencyProperty;

        public static void AddErrorHandler(UIElement element, RoutedEventHandler handler)
        {
            element.AddHandler(ErrorEvent, handler);
        }

        public static void RemoveErrorHandler(UIElement element, RoutedEventHandler handler)
        {
            element.RemoveHandler(ErrorEvent, handler);
        }

        public static void SetIsValidationScope(this FrameworkElement element, bool? value)
        {
            element.SetValue(IsValidationScopeProperty, value);
        }

        public static bool? GetIsValidationScope(this FrameworkElement element)
        {
            return (bool?)element.GetValue(IsValidationScopeProperty);
        }

        public static void SetHasError(DependencyObject element, bool value)
        {
            element.SetValue(HasErrorPropertyKey, value);
        }

        public static bool GetHasError(DependencyObject element)
        {
            return (bool)element.GetValue(HasErrorProperty);
        }

        public static void SetErrors(DependencyObject element, IEnumerable<ValidationError> value)
        {
            element.SetValue(ErrorsPropertyKey, value);
        }

        public static IEnumerable<ValidationError> GetErrors(DependencyObject element)
        {
            return (IEnumerable<ValidationError>)element.GetValue(ErrorsProperty);
        }

        public static void SetErrorProxies(this DependencyObject element, ErrorProxyCollection value)
        {
            element.SetValue(ErrorProxiesPropertyKey, value);
        }

        public static ErrorProxyCollection GetErrorProxies(this DependencyObject element)
        {
            return (ErrorProxyCollection)element.GetValue(ErrorProxiesProperty);
        }

        private static void OnIsValidationScopeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fe = d as FrameworkElement;
            if (fe == null)
            {
                return;
            }

            if (Equals(e.NewValue, true))
            {
                var proxy = fe.GetValue(ValidationProxyProperty);
                if (proxy == null)
                {
                    proxy = new ValidationProxy(fe);
                    fe.SetValue(ValidationProxyProperty, proxy);
                }

                var proxyCollection = new ErrorProxyCollection();
                d.SetErrorProxies(proxyCollection);
                AddErrorHandler(fe, OnError);
            }
            else
            {
                var proxy = (ValidationProxy)fe.GetValue(ValidationProxyProperty);
                if (proxy != null)
                {
                    proxy.Dispose();
                }
                var collection = d.GetErrorProxies();
                if (collection != null)
                {
                    collection.Dispose();
                }
                d.SetErrorProxies(null);
                RemoveErrorHandler(fe, OnError);
            }
        }

        private static void OnError(object sender, RoutedEventArgs e)
        {
            var source = e.Source as UIElement;
            var element = sender as UIElement;
            if (element != null && source != null)
            {
                var proxy = (ValidationProxy)source.GetValue(ValidationProxyProperty);
                if (proxy == null)
                {
                    return;
                }
                var errors = element.GetErrorProxies();

                if (errors == null)
                {
                    return;
                }

                if (proxy.HasError && !errors.Contains(proxy))
                {
                    errors.Add(proxy);
                }

                if (!proxy.HasError && errors.Contains(proxy))
                {
                    errors.Remove(proxy);
                }
                element.CoerceValue(HasErrorProperty);
                element.CoerceValue(ErrorsProperty);
            }
        }

        private static object OnHasErrorCoerce(DependencyObject d, object basevalue)
        {
            var proxyCollection = (ErrorProxyCollection)d.GetValue(ErrorProxiesProperty);
            if (proxyCollection == null)
            {
                return false;
            }
            return proxyCollection.Any(x => x.HasError);
        }

        private static object OnErrorsCoerce(DependencyObject d, object basevalue)
        {
            var proxyCollection = (ErrorProxyCollection)d.GetValue(ErrorProxiesProperty);
            if (proxyCollection == null)
            {
                return EmptyErrorCollection;
            }
            var errors = proxyCollection.SelectMany(x => x.Errors)
                                        .ToArray();
            return errors;
        }
    }
}