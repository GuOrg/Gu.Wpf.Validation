namespace Gu.Wpf.Validation.Tests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using Internals;
    using NUnit.Framework;

    [RequiresSTA]
    public class TextBoxTests
    {
        private readonly List<PropertyChangedEventArgs> _vmChanges = new List<PropertyChangedEventArgs>();
        private TextBox _textBox;
        private DummyViewModel<double> _vm;

        [SetUp]
        public void SetUp()
        {
            _vmChanges.Clear();
            _vm = new DummyViewModel<double>();
            _vm.PropertyChanged += VmOnPropertyChanged;
            _textBox = new TextBox { DataContext = _vm };
            _textBox.SetCulture(new CultureInfo("sv-SE"));
            var binding = new Binding
            {
                Path = new PropertyPath("Value"),
                Source = _vm,
                Mode = BindingMode.TwoWay
            };
            BindingOperations.SetBinding(_textBox, Input.ValueProperty, binding);
            _textBox.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));
        }

        [TearDown]
        public void TearDown()
        {
            _vm.PropertyChanged -= VmOnPropertyChanged;
        }

        [Test]
        public void EditingTextUpdatesValueAndVm()
        {
            _textBox.SetTextUndoable("1,23");
            Assert.AreEqual(1.23, _textBox.GetValue(Input.ValueProperty));
            Assert.AreEqual(0, _vm.Value);
            _textBox.RaiseEvent(new RoutedEventArgs(UIElement.LostFocusEvent));
            Assert.AreEqual(1.23, _vm.Value);
            Assert.AreEqual("1,23", _textBox.GetRawText());
            Assert.AreEqual(1.23, _textBox.GetRawValue());
        }

        [Test]
        public void ChangingDecimalDigitsOnlyChangesUi()
        {
            _textBox.SetTextUndoable("1,23456");
            _textBox.RaiseEvent(new RoutedEventArgs(UIElement.LostFocusEvent));
            _textBox.SetDecimalDigits(2);
            Assert.AreEqual("1,23",_textBox.Text);
            Assert.AreEqual("1,23456", _textBox.GetRawText());
            Assert.AreEqual(1.23456, _textBox.GetRawValue());
            Assert.AreEqual(1.23456, _textBox.GetValue(Input.ValueProperty));
            Assert.AreEqual(1.23456, _vm.Value);

            _textBox.SetDecimalDigits(3);
            Assert.AreEqual("1,235", _textBox.Text);
            Assert.AreEqual("1,23456", _textBox.GetRawText());
            Assert.AreEqual(1.23456, _textBox.GetRawValue());
            Assert.AreEqual(1.23456, _textBox.GetValue(Input.ValueProperty));
            Assert.AreEqual(1.23456, _vm.Value);

            _textBox.SetDecimalDigits(7);
            Assert.AreEqual("1,2345600", _textBox.Text);
            Assert.AreEqual("1,23456", _textBox.GetRawText());
            Assert.AreEqual(1.23456, _textBox.GetRawValue());
            Assert.AreEqual(1.23456, _textBox.GetValue(Input.ValueProperty));
            Assert.AreEqual(1.23456, _vm.Value);
        }

        [Test]
        public void ChangingCultureOnlyChangesUi()
        {
            _textBox.SetTextUndoable("1,2345");
            _textBox.SetDecimalDigits(2);
            _textBox.SetCulture(new CultureInfo("en-US"));
            Assert.AreEqual("1.23", _textBox.Text);
            Assert.AreEqual("1,2345", _textBox.GetRawText());
            Assert.AreEqual(1.2345, _textBox.GetRawValue());
            Assert.AreEqual(1.2345, _textBox.GetValue(Input.ValueProperty));
            Assert.AreEqual(1.2345, _vm.Value);
        }

        [Test]
        public void ValidationFailureResetsValue()
        {
            _textBox.SetTextUndoable("1,23");
            Assert.AreEqual(1.23, _textBox.GetValue(Input.ValueProperty));
            Assert.AreEqual("1,23", _textBox.GetRawText());
            Assert.AreEqual(1.23, _textBox.GetRawValue());

            _textBox.SetTextUndoable("1,23e");
           
            Assert.AreEqual(0, _textBox.GetValue(Input.ValueProperty));
            Assert.AreEqual("1,23e", _textBox.GetRawText());
            Assert.AreEqual(TextBoxExt.Unset, _textBox.GetRawValue());
        }

        [Test]
        public void ValidationSuccessUpdatesValue()
        {
            _textBox.SetTextUndoable("1.23");
            Assert.AreEqual(0, _textBox.GetValue(Input.ValueProperty));
            Assert.AreEqual("1.23",_textBox.GetRawText());
            Assert.AreEqual(TextBoxExt.Unset, _textBox.GetRawValue());

            _textBox.SetCulture(new CultureInfo("en-US"));
            
            Assert.AreEqual(1.23, _textBox.GetValue(Input.ValueProperty));
            Assert.AreEqual(1.23, _vm.Value);
            Assert.AreEqual("1.23", _textBox.GetRawText());
            Assert.AreEqual(1.23, _textBox.GetRawValue());
        }

        private void VmOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            _vmChanges.Add(propertyChangedEventArgs);
        }
    }
}
