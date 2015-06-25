namespace Gu.Wpf.Validation.Tests.Sandbox
{
    using System;
    using System.ComponentModel;
    using System.Windows.Controls;

    using NUnit.Framework;

    /// <summary>
    /// https://agsmith.wordpress.com/2008/04/07/propertydescriptor-addvaluechanged-alternative/
    /// </summary>
    [RequiresSTA, Explicit("Just confirming that it still leaks")]
    public class DescriptorLeaks
    {
        [Test]
        public void NoDescriptor()
        {
            ListBoxItem i = new ListBoxItem();
            WeakReference wr = new WeakReference(i);
            i = null;
            GC.Collect();
            Assert.IsFalse(wr.IsAlive);
        }

        [Test]
        public void WithDescriptor()
        {
            ListBoxItem i = new ListBoxItem();
            WeakReference wr = new WeakReference(i);
            var descriptor = DependencyPropertyDescriptor.FromProperty(ListBoxItem.IsSelectedProperty, typeof(ListBoxItem));
            // the following yields the same pd
            //PropertyDescriptor prop = DependencyPropertyDescriptor.FromProperty(ListBoxItem.IsSelectedProperty, typeof(ListBoxItem));
            descriptor.AddValueChanged(i, OnValueChanged);
            i = null;
            GC.Collect();
            Assert.IsFalse(wr.IsAlive);
        }

        private void OnValueChanged(object sender, EventArgs e)
        {
        }
    }
}
