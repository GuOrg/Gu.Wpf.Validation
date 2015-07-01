namespace Gu.Wpf.Validation.Tests
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    using Gu.Wpf.Validation.Internals;
    using Gu.Wpf.Validation.Tests.Helpers;

    using NUnit.Framework;

    [RequiresSTA]
    public class ScopeTests
    {
        private DummyViewModel _vm;
        private TextBox _doubleBox;
        private TextBox _intBox;
        private Grid _grid;

        [SetUp]
        public void SetUp()
        {
            _vm = new DummyViewModel();
            _doubleBox = CreateTextBox("DoubleValue");
            _intBox = CreateTextBox("IntValue");
            _grid = new Grid();
            _grid.Children.Add(_doubleBox);
            _grid.Children.Add(_intBox);
            _grid.SetIsValidationScope(true);
        }

        [Test]
        public void PromotesChildrenHasErrors()
        {
            Assert.IsFalse(Scope.GetHasError(_grid));
            _doubleBox.WriteText("abc");
            Assert.IsTrue(Scope.GetHasError(_grid));
        }

        [Test]
        public void AggregatesChildErrors()
        {
            CollectionAssert.IsEmpty(Scope.GetErrors(_grid));
            _doubleBox.WriteText("abc");
            Assert.AreEqual(1, Scope.GetErrors(_grid).Count());
            _intBox.WriteText("bdfas");
            Assert.AreEqual(2, Scope.GetErrors(_grid).Count());
        }

        private System.Windows.Controls.TextBox CreateTextBox(string propertyName)
        {
            var textBox = new System.Windows.Controls.TextBox { DataContext = _vm };
            textBox.SetCulture(new CultureInfo("sv-SE"));
            var binding = new Binding
            {
                Path = new PropertyPath(propertyName),
                Source = _vm,
                Mode = BindingMode.TwoWay
            };
            BindingOperations.SetBinding(textBox, Input.ValueProperty, binding);
            textBox.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));
            return textBox;
        }
    }
}
