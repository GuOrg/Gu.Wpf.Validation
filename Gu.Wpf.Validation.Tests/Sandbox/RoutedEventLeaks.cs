namespace Gu.Wpf.Validation.Tests.Sandbox
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    using NUnit.Framework;

    [RequiresSTA]
    public class RoutedEventLeaks
    {
        private static readonly RoutedEventHandler OnLoadedHandler = new RoutedEventHandler(OnLoaded);

        [Test]
        public void AddHandler()
        {
            TextBox textBox = new TextBox();
            textBox.AddHandler(FrameworkElement.LoadedEvent, OnLoadedHandler);
            WeakReference wr = new WeakReference(textBox);
            textBox = null;
            GC.Collect();
            Assert.IsFalse(wr.IsAlive);
        }

        private static void OnLoaded(object sender, EventArgs e)
        {
        }
    }
}