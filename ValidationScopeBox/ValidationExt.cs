using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace ValidationScopeBox
{
    public static class ValidationExt
    {
        public static readonly DependencyProperty IsValidationScopeProperty = DependencyProperty.RegisterAttached(
            "IsValidationScope",
            typeof(bool?),
            typeof(ValidationExt),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.Inherits,
                OnIsValidationScopeChanged));

        public static readonly DependencyPropertyKey ValidationProxyCollectionPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "ValidationProxyCollection",
            typeof(ValidationProxyCollection),
            typeof(ValidationExt),
            new PropertyMetadata(null));

        public static DependencyProperty ValidationProxyCollectionProperty = ValidationProxyCollectionPropertyKey.DependencyProperty;

        private static readonly DependencyProperty HasErrorProxyProperty = DependencyProperty.RegisterAttached(
            "HasErrorProxy",
            typeof(bool),
            typeof(ValidationExt),
            new PropertyMetadata(false, OnHasErrorChanged));

        public static readonly DependencyPropertyKey HasErrorPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "HasError",
            typeof(bool),
            typeof(ValidationExt),
            new PropertyMetadata(false));

        public static readonly DependencyProperty HasErrorProperty = HasErrorPropertyKey.DependencyProperty;

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

        public static void SetValidationProxyCollection(this DependencyObject element, ValidationProxyCollection value)
        {
            element.SetValue(ValidationProxyCollectionPropertyKey, value);
        }

        public static ValidationProxyCollection GetValidationProxyCollection(this DependencyObject element)
        {
            return (ValidationProxyCollection)element.GetValue(ValidationProxyCollectionProperty);
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
                if (fe.IsInitialized)
                {
                    AddToParent(fe);
                }
                else
                {
                    fe.Initialized += OnInitialized;
                }

                var proxyCollection = new ValidationProxyCollection();
                d.SetValidationProxyCollection(proxyCollection);
                var binding = new Binding
                {
                    Path = new PropertyPath(ValidationProxyCollection.HasErrorProperty),
                    Source = proxyCollection,
                    Mode = BindingMode.OneWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                };
                BindingOperations.SetBinding(d, HasErrorProxyProperty, binding);
            }
            else
            {
                var collection = d.GetValidationProxyCollection();
                if (collection != null)
                {
                    collection.Dispose();
                }
                d.SetValidationProxyCollection(null);
                BindingOperations.ClearBinding(d, HasErrorProxyProperty);
            }
        }

        private static void AddToParent(FrameworkElement fe)
        {
            var parent = VisualTreeHelper.GetParent(fe) as FrameworkElement;
            if (parent != null && GetIsValidationScope(parent) == true)
            {
                var proxyCollection = parent.GetValidationProxyCollection();
                if (proxyCollection != null)
                {
                    proxyCollection.Add(new ValidationProxy(fe));
                }
            }
        }

        private static void OnInitialized(object sender, EventArgs eventArgs)
        {
            var fe = (FrameworkElement)sender;
            fe.Initialized -= OnInitialized;
            AddToParent(fe);
        }

        private static void OnHasErrorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SetHasError(d, (bool)e.NewValue);
        }

        private static IEnumerable<DependencyObject> Ancestors(this DependencyObject o)
        {
            DependencyObject parent;
            while ((parent = VisualTreeHelper.GetParent(o))!=null)
            {
                yield return parent;
            }
        }
    }
}
