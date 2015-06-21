namespace Gu.Wpf.Validation
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows;

    [DebuggerDisplay("Count: {Count}")]
    public sealed class ErrorProxyCollection : ObservableCollection<ValidationProxy>, IDisposable
    {
        public void Dispose()
        {
            foreach (var validationProxy in this)
            {
                if (validationProxy != null)
                {
                    validationProxy.Dispose();
                }
            }
        }
    }
}