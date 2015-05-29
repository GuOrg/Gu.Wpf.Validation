using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Gu.Wpf.Validation
{
    public sealed class ValidationProxyCollection : DependencyObject, IEnumerable<ValidationProxy>, IDisposable
    {
        private readonly List<ValidationProxy> _validationProxies = new List<ValidationProxy>();

        public static readonly DependencyProperty HasErrorProperty = ValidationExt.HasErrorProperty.AddOwner(typeof(ValidationProxyCollection));

        public bool HasError
        {
            get { return (bool)GetValue(HasErrorProperty); }
            set { SetValue(ValidationExt.HasErrorPropertyKey, value); }
        }

        public void Add(ValidationProxy proxy)
        {
            proxy.HasErrorChanged += OnHasErrorsChanged;
            _validationProxies.Add(proxy);
        }

        public void RemoveFor(FrameworkElement e)
        {
            var match = _validationProxies.FirstOrDefault(x => ReferenceEquals(x.Element, e));
            if (match != null)
            {
                match.HasErrorChanged -= OnHasErrorsChanged;
                _validationProxies.Remove(match);
            }
        }

        public IEnumerator<ValidationProxy> GetEnumerator()
        {
            return _validationProxies.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Dispose()
        {
            foreach (var validationProxy in _validationProxies)
            {
                if (validationProxy != null)
                {
                    validationProxy.Dispose();
                }
            }
        }

        public override string ToString()
        {
            return string.Format("Count: {0}, HasError: {1}", _validationProxies.Count, HasError);
        }

        private void OnHasErrorsChanged(object sender, EventArgs e)
        {
            HasError = _validationProxies.Any(x => x.HasError);
        }
    }
}