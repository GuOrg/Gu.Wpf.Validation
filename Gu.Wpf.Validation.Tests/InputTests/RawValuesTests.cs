namespace Gu.Wpf.Validation.Tests.InputTests
{
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
        }

        [Test]
        public void UpdatesOnInput()
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

            TextCompositionManager.StartComposition(new TextComposition(InputManager.Current, textBox, "1.23456"));

            Assert.AreEqual("1.23456", textBox.GetRawText());
            Assert.AreEqual(1.23456, textBox.GetRawValue());
        }
    }
}
