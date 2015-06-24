namespace Gu.Wpf.Validation
{
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
        //protected static readonly PropertyPath ValuePath = new PropertyPath(Input.ValueProperty);
        protected static readonly PropertyPath RawTextPath = new PropertyPath(RawValueTracker.RawTextProperty);

        protected static readonly UpdateFormattingConverter UpdateFormattingConverter = new UpdateFormattingConverter();

        protected static readonly DependencyProperty UpdateFormattingProxyProperty = DependencyProperty.RegisterAttached(
            "UpdateFormattingProxy",
            typeof(object),
            typeof(DefaultFormatter),
            new PropertyMetadata(null));

        private static readonly RoutedEventHandler OnLoadedHandler = new RoutedEventHandler(OnLoaded);

        public virtual void Bind(TextBox textBox)
        {
            if (textBox == null)
            {
                return;
            }
            if (textBox.IsLoaded)
            {
                ClearBindings(textBox);
                AddBinding(textBox);
            }
            else
            {
                textBox.UpdateHandler(FrameworkElement.LoadedEvent, OnLoadedHandler);
            }
        }

        /// <summary>
        /// Using a binding with converter to update formatting
        /// </summary>
        /// <param name="textBox"></param>
        protected virtual void AddBinding(TextBox textBox)
        {
            var binding = new MultiBinding { Converter = UpdateFormattingConverter, ConverterParameter = textBox };
            binding.Bindings.Add(CreateBinding(textBox, IsKeyboardFocusedPath));
            binding.Bindings.Add(CreateBinding(textBox, DecimalDigitsPath));
            binding.Bindings.Add(CreateBinding(textBox, CulturePath));
            binding.Bindings.Add(CreateBinding(textBox, RawTextPath));
            BindingOperations.SetBinding(textBox, UpdateFormattingProxyProperty, binding);
        }

        protected virtual void ClearBindings(TextBox textBox)
        {
            BindingOperations.ClearBinding(textBox, UpdateFormattingProxyProperty);
        }

        protected virtual Binding CreateBinding(
            System.Windows.Controls.TextBox source,
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

        private static void OnLoaded(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            textBox.RemoveHandler(FrameworkElement.LoadedEvent, OnLoadedHandler);
            var formatter = textBox.GetFormatter() as DefaultFormatter;
            if (formatter != null)
            {
                formatter.AddBinding(textBox);
            }
        }
    }
}
