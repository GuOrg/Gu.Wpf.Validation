namespace Gu.Wpf.Validation.Tests.IntegrationTests
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    using Gu.Wpf.Validation.Internals;
    using Gu.Wpf.Validation.Tests.Helpers;

    using NUnit.Framework;

    [RequiresSTA]
    public abstract class TextBoxTests
    {
        protected readonly List<PropertyChangedEventArgs> VmChanges = new List<PropertyChangedEventArgs>();
        protected TextBox TextBox;
        protected DummyViewModel Vm;

        protected abstract PropertyInfo Property { get; }

        [SetUp]
        public void SetUp()
        {
            VmChanges.Clear();
            Vm = new DummyViewModel();
            Vm.PropertyChanged += VmOnPropertyChanged;
            TextBox = new TextBox { DataContext = Vm };
            TextBox.SetCulture(new CultureInfo("sv-SE"));
            var binding = new Binding
            {
                Path = new PropertyPath(Property.Name),
                Source = Vm,
                Mode = BindingMode.TwoWay
            };
            BindingOperations.SetBinding(TextBox, Input.ValueProperty, binding);
            TextBox.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));
        }

        [TearDown]
        public void TearDown()
        {
            Vm.PropertyChanged -= VmOnPropertyChanged;
        }

        [Test]
        public void EditingTextUpdatesValueAndVm()
        {
            TextBox.SetTextUndoable("1,23");
            Assert.AreEqual(1.23, TextBox.GetValue(Input.ValueProperty));
            Assert.AreEqual(0, Property.GetValue(Vm));
            TextBox.RaiseEvent(new RoutedEventArgs(UIElement.LostFocusEvent));
            Assert.AreEqual(1.23, Property.GetValue(Vm));
            Assert.AreEqual("1,23", TextBox.GetRawText());
            Assert.AreEqual(1.23, TextBox.GetRawValue());
        }

        [Test]
        public void ChangingDecimalDigitsOnlyChangesUi()
        {
            TextBox.SetTextUndoable("1,23456");
            TextBox.RaiseEvent(new RoutedEventArgs(UIElement.LostFocusEvent));
            TextBox.SetDecimalDigits(2);
            Assert.AreEqual("1,23", TextBox.Text);
            Assert.AreEqual("1,23456", TextBox.GetRawText());
            Assert.AreEqual(1.23456, TextBox.GetRawValue());
            Assert.AreEqual(1.23456, TextBox.GetValue(Input.ValueProperty));
            Assert.AreEqual(1.23456, Property.GetValue(Vm));

            TextBox.SetDecimalDigits(3);
            Assert.AreEqual("1,235", TextBox.Text);
            Assert.AreEqual("1,23456", TextBox.GetRawText());
            Assert.AreEqual(1.23456, TextBox.GetRawValue());
            Assert.AreEqual(1.23456, TextBox.GetValue(Input.ValueProperty));
            Assert.AreEqual(1.23456, Property.GetValue(Vm));

            TextBox.SetDecimalDigits(7);
            Assert.AreEqual("1,2345600", TextBox.Text);
            Assert.AreEqual("1,23456", TextBox.GetRawText());
            Assert.AreEqual(1.23456, TextBox.GetRawValue());
            Assert.AreEqual(1.23456, TextBox.GetValue(Input.ValueProperty));
            Assert.AreEqual(1.23456, Property.GetValue(Vm));
        }

        [Test]
        public void ChangingCultureOnlyChangesUi()
        {
            TextBox.SetTextUndoable("1,2345");
            TextBox.SetDecimalDigits(2);
            TextBox.SetCulture(new CultureInfo("en-US"));
            Assert.AreEqual("1.23", TextBox.Text);
            Assert.AreEqual("1,2345", TextBox.GetRawText());
            Assert.AreEqual(1.2345, TextBox.GetRawValue());
            Assert.AreEqual(1.2345, TextBox.GetValue(Input.ValueProperty));
            Assert.AreEqual(1.2345, Property.GetValue(Vm));
        }

        [Test]
        public void ValidationFailureResetsValue()
        {
            TextBox.SetTextUndoable("1,23");
            Assert.AreEqual(1.23, TextBox.GetValue(Input.ValueProperty));
            Assert.AreEqual("1,23", TextBox.GetRawText());
            Assert.AreEqual(1.23, TextBox.GetRawValue());

            TextBox.SetTextUndoable("1,23e");

            Assert.AreEqual(0, TextBox.GetValue(Input.ValueProperty));
            Assert.AreEqual("1,23e", TextBox.GetRawText());
            Assert.AreEqual(RawValueTracker.Unset, TextBox.GetRawValue());
        }

        [Test]
        public void ValidationSuccessUpdatesValue()
        {
            TextBox.SetTextUndoable("1.23");
            Assert.AreEqual(0, TextBox.GetValue(Input.ValueProperty));
            Assert.AreEqual("1.23", TextBox.GetRawText());
            Assert.AreEqual(RawValueTracker.Unset, TextBox.GetRawValue());

            TextBox.SetCulture(new CultureInfo("en-US"));

            Assert.AreEqual(1.23, TextBox.GetValue(Input.ValueProperty));
            Assert.AreEqual(1.23, Property.GetValue(Vm));
            Assert.AreEqual("1.23", TextBox.GetRawText());
            Assert.AreEqual(1.23, TextBox.GetRawValue());
        }

        protected void VmOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            VmChanges.Add(propertyChangedEventArgs);
        }
    }
}
