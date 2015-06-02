namespace Gu.Wpf.Validation
{
    using System;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    using Gu.Wpf.Validation.Internals;

    public class DefaultFormatter : IFormatter
    {
        protected static readonly PropertyPath IsKeyboardFocusedPath = new PropertyPath(UIElement.IsKeyboardFocusedProperty);
        protected static readonly PropertyPath DecimalDigitsPath = new PropertyPath(Input.DecimalDigitsProperty);
        protected static readonly PropertyPath CulturePath = new PropertyPath(Input.CultureProperty);
        protected static readonly PropertyPath TextPath = new PropertyPath(TextBox.TextProperty);

        protected static readonly UpdateFormattingConverter UpdateFormattingConverter = new UpdateFormattingConverter();

        protected static readonly DependencyProperty UpdateFormattingProxyProperty = DependencyProperty.RegisterAttached(
            "UpdateFormattingProxy",
            typeof(object),
            typeof(DefaultFormatter),
            new PropertyMetadata(null));

        protected static readonly DependencyProperty TextProxyProperty = DependencyProperty.RegisterAttached(
            "TextProxy",
            typeof(string),
            typeof(DefaultFormatter),
            new PropertyMetadata(null, OnTextProxyChanged));

        public virtual void Bind(TextBox textBox)
        {
            if (textBox == null)
            {
                return;
            }
            textBox.SetValue(TextBoxExt.OldCultureProperty, textBox.GetValue(Input.CultureProperty));
            BindingOperations.SetBinding(textBox, TextProxyProperty, CreateBinding(textBox, TextPath));
            // Using a binding to update formatting
            var binding = new MultiBinding
                              {
                                  Converter = UpdateFormattingConverter,
                                  ConverterParameter = textBox
                              };
            binding.Bindings.Add(CreateBinding(textBox, IsKeyboardFocusedPath));
            binding.Bindings.Add(CreateBinding(textBox, DecimalDigitsPath));
            binding.Bindings.Add(CreateBinding(textBox, CulturePath));
            // Using a binding with converter to update formatting
            BindingOperations.SetBinding(textBox, UpdateFormattingProxyProperty, binding);
        }

        protected virtual Binding CreateBinding(
            TextBox source,
            PropertyPath path)
        {
            return new Binding
            {
                Path = path,
                Source = source,
                Mode = BindingMode.OneWay,
                ConverterParameter = source,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
        }

        private static void OnTextProxyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = d as TextBox;
            if (textBox.GetIsUpdating())
            {
                return;
            }
            var newValue = (string)e.NewValue;
            Debug.WriteLine("SetRawText: " + newValue);
            textBox.SetRawText(newValue);
        }
    }
}
