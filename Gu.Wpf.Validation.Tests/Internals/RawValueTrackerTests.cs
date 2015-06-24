namespace Gu.Wpf.Validation.Tests.Internals
{
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    using Gu.Wpf.Validation.Internals;
    using Gu.Wpf.Validation.Tests.Helpers;

    using NUnit.Framework;

    [RequiresSTA]
    public class RawValueTrackerTests
    {
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
            textBox.RaiseLoadedEvent();

            textBox.WriteText("1.234");
            Assert.AreEqual("1.234", textBox.GetRawText());
            Assert.AreEqual(1.234, textBox.GetRawValue());
            Assert.AreEqual(RawValueSource.User, textBox.GetRawValueSource());
        }

        [Test]
        public void UpdatesOnBinding()
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
            textBox.RaiseLoadedEvent();

            vm.NullableDoubleValue = 1.234;
            Assert.AreEqual("1.234", textBox.GetRawText());
            Assert.AreEqual(1.234, textBox.GetRawValue());
            Assert.AreEqual(RawValueSource.Binding, textBox.GetRawValueSource());
        }

        [Test]
        public void ResetsOnCulture()
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
            textBox.RaiseLoadedEvent();

            textBox.WriteText("1,234");
            Assert.AreEqual("1,234", textBox.GetRawText());
            Assert.AreEqual(RawValueTracker.Unset, textBox.GetRawValue());
            Assert.AreEqual(RawValueSource.User, textBox.GetRawValueSource());

            textBox.SetCulture(new CultureInfo("sv-SE"));
            Assert.AreEqual("1,234", textBox.GetRawText());
            Assert.AreEqual(1.234, textBox.GetRawValue());
            Assert.AreEqual(RawValueSource.User, textBox.GetRawValueSource());
        }

        [Test]
        public void ResetsOnDataContext()
        {
            var textBox = new TextBox();
            textBox.SetCulture(new CultureInfo("en-US"));
            textBox.SetDecimalDigits(2);

            var binding = new Binding
            {
                Path = new PropertyPath(DummyViewModel.NullableDoubleValuePropertyName),
                Mode = BindingMode.TwoWay
            };
            BindingOperations.SetBinding(textBox, Input.ValueProperty, binding);

            Assert.Inconclusive("What is most right here? Maybe propagating the value from vm -> view");
            textBox.WriteText("1.234");
            Assert.AreEqual("1.234", textBox.GetRawText());
            Assert.AreEqual(RawValueTracker.Unset, textBox.GetRawValue());
            Assert.AreEqual(RawValueSource.User, textBox.GetRawValueSource());

            var vm = new DummyViewModel { NullableDoubleValue = null };
            textBox.DataContext = vm;
            Assert.AreEqual("1.234", textBox.GetRawText());
            Assert.AreEqual(1.234, textBox.GetRawValue());
            Assert.AreEqual(RawValueSource.User, textBox.GetRawValueSource());
        }
    }
}
