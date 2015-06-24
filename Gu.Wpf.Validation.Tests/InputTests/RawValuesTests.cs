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
            textBox.RaiseLoadedEvent();
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
            textBox.RaiseLoadedEvent();

            Assert.IsNullOrEmpty(textBox.GetRawText());
            Assert.AreEqual(null, textBox.GetRawValue());

            textBox.WriteText("1.234");

            Assert.AreEqual("1.234", textBox.GetRawText());
            Assert.AreEqual(1.234, textBox.GetRawValue());
            Assert.AreEqual(RawValueSource.User, textBox.GetRawValueSource());

            textBox.WriteText("56", false);

            Assert.AreEqual("1.23456", textBox.GetRawText());
            Assert.AreEqual(1.23456, textBox.GetRawValue());
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
            textBox.RaiseLoadedEvent();

            Assert.IsNullOrEmpty(textBox.GetRawText());
            Assert.AreEqual(null, textBox.GetRawValue());

            textBox.WriteText("1.2dae");

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
            textBox.RaiseLoadedEvent();

            Assert.IsNullOrEmpty(textBox.GetRawText());
            Assert.AreEqual(null, textBox.GetRawValue());

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
        public void RawTextNotAffectedByFormatting()
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
            Assert.AreEqual("1.234", textBox.GetRawText());
            Assert.AreEqual(1.234, textBox.GetRawValue());
            Assert.AreEqual(RawValueSource.User, textBox.GetRawValueSource());

            textBox.SetCulture(new CultureInfo("sv-SE"));
            Assert.AreEqual(1, count);
            Assert.AreEqual("1,234", textBox.Text);
            Assert.AreEqual("1.234", textBox.GetRawText());
            Assert.AreEqual(1.234, textBox.GetRawValue());
            Assert.AreEqual(RawValueSource.User, textBox.GetRawValueSource());
        }

        [Test]
        public void RawValueSourceNotAffectedByFormatting()
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

            vm.NullableDoubleValue = 1.234;
            Assert.AreEqual(1, count);
            Assert.AreEqual("1.234", textBox.GetRawText());
            Assert.AreEqual(1.234, textBox.GetRawValue());
            Assert.AreEqual(RawValueSource.Binding, textBox.GetRawValueSource());

            textBox.SetCulture(new CultureInfo("sv-SE"));
            Assert.AreEqual(1, count);
            Assert.AreEqual("1,234", textBox.Text);
            Assert.AreEqual("1.234", textBox.GetRawText());
            Assert.AreEqual(1.234, textBox.GetRawValue());
            Assert.AreEqual(RawValueSource.Binding, textBox.GetRawValueSource());
        }
    }
}
