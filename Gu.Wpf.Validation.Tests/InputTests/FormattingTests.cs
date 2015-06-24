namespace Gu.Wpf.Validation.Tests.InputTests
{
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using Helpers;
    using NUnit.Framework;
    using Validation.Internals;

    [RequiresSTA]
    public class FormattingTests
    {
        [Test]
        public void UpdatesOnDigitsChange()
        {
            var vm = new DummyViewModel { NullableDoubleValue = null };
            int count = 0;
            vm.PropertyChanged += (_, __) => count++;
            var textBox = new TextBox { DataContext = vm };
            textBox.SetCulture(new CultureInfo("en-US"));
            var binding = new Binding
            {
                Path = new PropertyPath(DummyViewModel.NullableDoubleValuePropertyName),
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                Mode = BindingMode.TwoWay
            };
            BindingOperations.SetBinding(textBox, Input.ValueProperty, binding);
            textBox.RaiseLoadedEvent();

            Assert.IsNullOrEmpty(textBox.GetRawText());
            Assert.AreEqual(null, textBox.GetRawValue());

            textBox.WriteText("1.234");
            Assert.AreEqual(1, count);
            Assert.AreEqual("1.234", textBox.Text);
        
            textBox.SetDecimalDigits(2);
            Assert.AreEqual(1, count);
            Assert.AreEqual("1.23", textBox.Text);

            textBox.SetDecimalDigits(4);
            Assert.AreEqual(1, count);
            Assert.AreEqual("1.2340", textBox.Text);
        }

        [Test]
        public void UpdatesOnCultureChange()
        {
            var vm = new DummyViewModel { NullableDoubleValue = null };
            int count = 0;
            vm.PropertyChanged += (_, __) => count++;
            var textBox = new TextBox { DataContext = vm };
            textBox.SetCulture(new CultureInfo("en-US"));
            var binding = new Binding
            {
                Path = new PropertyPath(DummyViewModel.NullableDoubleValuePropertyName),
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                Mode = BindingMode.TwoWay
            };
            BindingOperations.SetBinding(textBox, Input.ValueProperty, binding);
            textBox.RaiseLoadedEvent();

            Assert.IsNullOrEmpty(textBox.GetRawText());
            Assert.AreEqual(null, textBox.GetRawValue());

            textBox.WriteText("1.234");
            Assert.AreEqual(1, count);
            Assert.AreEqual("1.234", textBox.Text);

            textBox.SetCulture(new CultureInfo("sv-SE"));
            Assert.AreEqual(1, count);
            Assert.AreEqual("1,234", textBox.Text);
        }
    }
}
