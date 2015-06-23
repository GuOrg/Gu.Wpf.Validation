namespace Gu.Wpf.Validation.Tests.InputTests
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    using Gu.Wpf.Validation.Internals;
    using Gu.Wpf.Validation.Tests.Helpers;

    using NUnit.Framework;

    [RequiresSTA]
    public class RawValuesTests
    {
        [Test]
        public void UpdatesWhenVmChanges()
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
            Assert.IsNullOrEmpty(textBox.GetRawText());
            Assert.AreEqual(null, textBox.GetRawValue());

            vm.NullableDoubleValue = 1.23456;

            Assert.AreEqual("1.23456", textBox.GetRawText());
            Assert.AreEqual(1.23456, textBox.GetRawValue());
            Assert.AreEqual(RawValueSource.Binding, textBox.GetRawValueSource());
        }

        [Test]
        public void UpdatesOnUserInput()
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
            Assert.IsNullOrEmpty(textBox.GetRawText());
            Assert.AreEqual(null, textBox.GetRawValue());

            TextCompositionManager.StartComposition(new TextComposition(InputManager.Current, textBox, "1.234"));

            Assert.AreEqual("1.23", textBox.Text);
            Assert.AreEqual("1.234", textBox.GetRawText());
            Assert.AreEqual(1.234, textBox.GetRawValue());
            Assert.AreEqual(RawValueSource.User, textBox.GetRawValueSource());

            TextCompositionManager.StartComposition(new TextComposition(InputManager.Current, textBox, "56"));

            Assert.AreEqual("1.24", textBox.Text);
            Assert.AreEqual("1.2356", textBox.GetRawText());
            Assert.AreEqual(1.2356, textBox.GetRawValue());
            Assert.AreEqual(RawValueSource.User, textBox.GetRawValueSource());
        }

        [Test]
        public void UpdatesOnUserErrorInput()
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
            Assert.IsNullOrEmpty(textBox.GetRawText());
            Assert.AreEqual(null, textBox.GetRawValue());

            TextCompositionManager.StartComposition(new TextComposition(InputManager.Current, textBox, "1.2dae"));

            Assert.AreEqual("1.2dae", textBox.GetRawText());
            Assert.AreEqual(RawValueTracker.Unset, textBox.GetRawValue());
            Assert.AreEqual(RawValueSource.User, textBox.GetRawValueSource());
        }

        [Test]
        public void UpdatesRawValueOnCultureChange()
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
            Assert.IsNullOrEmpty(textBox.GetRawText());
            Assert.AreEqual(null, textBox.GetRawValue());

            TextCompositionManager.StartComposition(new TextComposition(InputManager.Current, textBox, "1,234"));

            Assert.AreEqual("1,234", textBox.GetRawText());
            Assert.AreEqual(RawValueTracker.Unset, textBox.GetRawValue());
            Assert.AreEqual(RawValueSource.User, textBox.GetRawValueSource());

            textBox.SetCulture(new CultureInfo("sv-SE"));
            Assert.AreEqual("1,234", textBox.GetRawText());
            Assert.AreEqual(1.234, textBox.GetRawValue());
            Assert.AreEqual(RawValueSource.User, textBox.GetRawValueSource());
        }

        [Test(Description = "This only works in Release build. Debug extends lifetime for debugging")]
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
