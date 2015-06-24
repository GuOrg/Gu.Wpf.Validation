namespace Gu.Wpf.Validation.Tests.InputTests
{
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using Helpers;
    using NUnit.Framework;
    using Rules;

    [RequiresSTA]
    public class ValidationTests
    {
        [Test(Description = "Not sure how we want this")]
        public void WhenRequiredAndNoDataContext()
        {
            var textBox = new TextBox { DataContext = null };
            var binding = new Binding
            {
                Path = new PropertyPath(DummyViewModel.NullableDoubleValuePropertyName),
                Mode = BindingMode.TwoWay
            };
            BindingOperations.SetBinding(textBox, Input.ValueProperty, binding);
            textBox.RaiseLoadedEvent();

            Assert.Inconclusive("Not sure how we want this");
            textBox.SetIsRequired(true);
            AssertError<IsRequiredError>(textBox);

            textBox.SetIsRequired(false);
            AssertNoError(textBox);
        }

        [Test]
        public void WhenRequiredButMissing()
        {
            var textBox = new TextBox { DataContext = new DummyViewModel { NullableDoubleValue = null } };
            var binding = new Binding
            {
                Path = new PropertyPath(DummyViewModel.NullableDoubleValuePropertyName),
                Mode = BindingMode.TwoWay
            };
            BindingOperations.SetBinding(textBox, Input.ValueProperty, binding);
            textBox.RaiseLoadedEvent();

            textBox.SetIsRequired(true);
            AssertError<IsRequiredError>(textBox);

            textBox.SetIsRequired(false);
            AssertNoError(textBox);
        }

        [Test]
        public void WhenRequiredAndGetsValueFromBinding()
        {
            var vm = new DummyViewModel { NullableDoubleValue = null };
            var textBox = new TextBox { DataContext = vm };
            var binding = new Binding
            {
                Path = new PropertyPath(DummyViewModel.NullableDoubleValuePropertyName),
                Mode = BindingMode.TwoWay
            };
            BindingOperations.SetBinding(textBox, Input.ValueProperty, binding);
            textBox.RaiseLoadedEvent();

            textBox.SetIsRequired(true);
            AssertError<IsRequiredError>(textBox);

            vm.NullableDoubleValue = 1;
            AssertNoError(textBox);
        }

        [Test]
        public void WhenRequiredAndGetsValueFromUser()
        {
            var textBox = new TextBox { DataContext = new DummyViewModel { NullableDoubleValue = null } };
            var binding = new Binding
            {
                Path = new PropertyPath(DummyViewModel.NullableDoubleValuePropertyName),
                Mode = BindingMode.TwoWay
            };
            BindingOperations.SetBinding(textBox, Input.ValueProperty, binding);
            textBox.RaiseLoadedEvent();

            textBox.SetIsRequired(true);
            AssertError<IsRequiredError>(textBox);

            textBox.WriteText("1");
            AssertNoError(textBox);
        }

        [Test]
        public void UpdatesWhenCultureChanges()
        {
            var vm = new DummyViewModel() { NullableDoubleValue = 3 };
            var textBox = new TextBox { DataContext = vm };
            var binding = new Binding
            {
                Path = new PropertyPath(DummyViewModel.NullableDoubleValuePropertyName),
                Mode = BindingMode.TwoWay
            };
            BindingOperations.SetBinding(textBox, Input.ValueProperty, binding);
            textBox.RaiseLoadedEvent();

            textBox.SetCulture(new CultureInfo("en-US"));

            textBox.WriteText("1,2", true);
            AssertError<CanParseError>(textBox);
            Assert.AreEqual(3, textBox.GetValue(Input.ValueProperty));
            Assert.AreEqual(3, vm.NullableDoubleValue);

            textBox.SetCulture(new CultureInfo("sv-SE"));
            AssertNoError(textBox);
            Assert.AreEqual(1.2, textBox.GetValue(Input.ValueProperty));
            Assert.AreEqual(1.2, vm.NullableDoubleValue);
        }

        [Test]
        public void WhenGreaterThanMaxUpdatesOnInput()
        {
            var textBox = new TextBox { DataContext = new DummyViewModel() };
            var binding = new Binding
            {
                Path = new PropertyPath(DummyViewModel.NullableDoubleValuePropertyName),
                Mode = BindingMode.TwoWay
            };
            BindingOperations.SetBinding(textBox, Input.ValueProperty, binding);
            textBox.RaiseLoadedEvent();

            textBox.SetMax(4.0);
            textBox.WriteText("5", true);
            AssertError<MustBeLessThanMaxError>(textBox);
            textBox.WriteText("3", true);
            AssertNoError(textBox);
        }

        [Test]
        public void WhenGreaterThanMaxUpdatesOnMax()
        {
            var textBox = new TextBox { DataContext = new DummyViewModel() };
            var binding = new Binding
            {
                Path = new PropertyPath(DummyViewModel.NullableDoubleValuePropertyName),
                Mode = BindingMode.TwoWay
            };
            BindingOperations.SetBinding(textBox, Input.ValueProperty, binding);
            textBox.RaiseLoadedEvent();

            textBox.WriteText("5", true);

            textBox.SetMax(4.0);
            AssertError<MustBeLessThanMaxError>(textBox);

            textBox.SetMax(6.0);
            AssertNoError(textBox);
        }

        [Test]
        public void WhenGreaterThanMaxUpdatesOnBinding()
        {
            var vm = new DummyViewModel { NullableDoubleValue = null };
            var textBox = new TextBox { DataContext = vm };
            var binding = new Binding
            {
                Path = new PropertyPath(DummyViewModel.NullableDoubleValuePropertyName),
                Mode = BindingMode.TwoWay
            };
            BindingOperations.SetBinding(textBox, Input.ValueProperty, binding);
            textBox.RaiseLoadedEvent();

            textBox.SetMax(4.0);
            vm.NullableDoubleValue = 5;
            AssertError<MustBeLessThanMaxError>(textBox);
            vm.NullableDoubleValue = 3;
            AssertNoError(textBox);
        }

        [Test]
        public void WhenGreaterThanMaxWithConverter()
        {
            var textBox = new TextBox { DataContext = new DummyViewModel() };
            var binding = new Binding
            {
                Path = new PropertyPath(DummyViewModel.StringIntValuePropertyName),
                Converter = new StringToIntConverter(),
                Mode = BindingMode.TwoWay
            };
            BindingOperations.SetBinding(textBox, Input.ValueProperty, binding);
            textBox.RaiseLoadedEvent();

            textBox.SetMax(4);
            textBox.WriteText("5", true);
            AssertError<MustBeLessThanMaxError>(textBox);

            textBox.SetMax(6);
            AssertNoError(textBox);
        }

        [Test]
        public void WhenLessThanMin()
        {
            var textBox = new TextBox { DataContext = new DummyViewModel() };
            var binding = new Binding
            {
                Path = new PropertyPath(DummyViewModel.NullableDoubleValuePropertyName),
                Mode = BindingMode.TwoWay
            };
            BindingOperations.SetBinding(textBox, Input.ValueProperty, binding);
            textBox.RaiseLoadedEvent();

            textBox.SetMin(6.0);
            textBox.WriteText("5", true);
            AssertError<MustBeGreaterThanMinError>(textBox);

            textBox.SetMin(4.0);
            AssertNoError(textBox);
        }

        private static void AssertError<T>(TextBox textBox)
        {
            var errors = Validation.GetErrors(textBox);
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(typeof(T), errors[0].ErrorContent.GetType());
        }

        private static void AssertNoError(TextBox textBox)
        {
            var errors = Validation.GetErrors(textBox);
            Assert.AreEqual(0, errors.Count);
        }
    }
}
