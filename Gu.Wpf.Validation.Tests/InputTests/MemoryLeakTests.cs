namespace Gu.Wpf.Validation.Tests.InputTests
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    using Gu.Wpf.Validation.Tests.Helpers;

    using NUnit.Framework;

    [RequiresSTA]
    [TestFixture(Description = "This only works in Release build. Debug extends lifetime for debugging")]
    public class MemoryLeakTests
    {
        [Test]
        public void DoesNotLeakMemory()
        {
            var vm = new DummyViewModel { NullableDoubleValue = null };
            var textBox = new TextBox { DataContext = vm };
            textBox.SetCulture(new CultureInfo("en-US"));
            textBox.SetDecimalDigits(2);

            var binding = new Binding
            {
                Path = new PropertyPath(DummyViewModel.NullableDoubleValuePropertyName),
                Mode = BindingMode.TwoWay
            };
            BindingOperations.SetBinding(textBox, Input.ValueProperty, binding);

            var wr = new WeakReference(textBox);
            textBox = null;
            GC.Collect();
            Assert.IsFalse(wr.IsAlive);
        }
    }
}
