namespace Gu.Wpf.Validation.Tests.InputTests
{
    using System.Windows;
    using System.Windows.Data;

    using Gu.Wpf.Validation.Tests.Helpers;

    using NUnit.Framework;

    [RequiresSTA]
    public class SourceValueTypeTests
    {
        [Test]
        public void WhenDataContextChanges()
        {
            var textBox = new System.Windows.Controls.TextBox { DataContext = null };
            var binding = new Binding
            {
                Path = new PropertyPath(DummyViewModel.NullableDoubleValuePropertyName),
                Mode = BindingMode.TwoWay
            };
            BindingOperations.SetBinding(textBox, Input.ValueProperty, binding);
            textBox.RaiseLoadedEvent();
            Assert.AreEqual(null, textBox.GetSourceValueType());

            var vm = new DummyViewModel { NullableDoubleValue = null };
            textBox.DataContext = vm;
            Assert.AreEqual(typeof(double?), textBox.GetSourceValueType());
        }

        [Test]
        public void NullableDouble()
        {
            var vm = new DummyViewModel { NullableDoubleValue = null };
            var textBox = new System.Windows.Controls.TextBox { DataContext = vm };
            var binding = new Binding
            {
                Path = new PropertyPath(DummyViewModel.NullableDoubleValuePropertyName),
                Mode = BindingMode.TwoWay
            };
            BindingOperations.SetBinding(textBox, Input.ValueProperty, binding);
            textBox.RaiseLoadedEvent();
            textBox.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));
            Assert.AreEqual(typeof(double?), textBox.GetSourceValueType());

            vm.NullableDoubleValue = 1;
            Assert.AreEqual(typeof(double?), textBox.GetSourceValueType());
        }

        [Test]
        public void WithConverter()
        {
            var vm = new DummyViewModel { StringIntValue = "1" };
            var textBox = new System.Windows.Controls.TextBox { DataContext = vm };
            var binding = new Binding
            {
                Converter = new StringToIntConverter(),
                Path = new PropertyPath(DummyViewModel.StringIntValuePropertyName),
                Mode = BindingMode.TwoWay
            };
            BindingOperations.SetBinding(textBox, Input.ValueProperty, binding);
            textBox.RaiseLoadedEvent();
            Assert.AreEqual(typeof(int), textBox.GetSourceValueType());
        }

        [Test]
        public void SideEffectsOnChange()
        {
            var textBox = new System.Windows.Controls.TextBox();
            Assert.IsNull(BindingOperations.GetBindingExpression(textBox, System.Windows.Controls.TextBox.TextProperty));

            textBox.SetSourceValueType(typeof(int));
            textBox.RaiseLoadedEvent();
            Assert.NotNull(BindingOperations.GetBindingExpression(textBox, System.Windows.Controls.TextBox.TextProperty));

            Assert.AreEqual(DefaultNumberStyles.DefaultInteger, textBox.GetNumberStyles());
            Assert.AreEqual(DefaultStringConverters.Converters[typeof(int)], textBox.GetStringConverter());
            Assert.AreEqual(DefaultRules.Rules[typeof(int)], textBox.GetValidationRules());
        }
    }
}
