namespace Gu.Wpf.Validation
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Threading;

    using Gu.Wpf.Validation.Internals;

    public static partial class Input
    {
        public static readonly DependencyProperty WatermarkTextProperty = DependencyProperty.RegisterAttached(
            "WatermarkText",
            typeof(string),
            typeof(Input),
            new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.Inherits, OnWatermarkTextChanged));

        public static readonly DependencyProperty VisibleWhenProperty = DependencyProperty.RegisterAttached(
            "VisibleWhen",
            typeof(WatermarkVisibility),
            typeof(Input),
            new PropertyMetadata(WatermarkVisibility.WhenEmpty));

        private static readonly DependencyProperty WatermarkAdornerProperty = DependencyProperty.RegisterAttached(
            "WatermarkAdorner",
            typeof(ContentAdorner),
            typeof(Input),
            new PropertyMetadata(default(ContentAdorner), OnWaterMarkAdornerChanged));

        private static readonly RoutedEventHandler OnWatermarkedLoadedHandler = OnWatermarkedLoaded;

        public static void SetWatermarkText(this DependencyObject element, string value)
        {
            element.SetValue(WatermarkTextProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static string GetWatermarkText(this DependencyObject element)
        {
            return (string)element.GetValue(WatermarkTextProperty);
        }

        public static void SetVisibleWhen(this DependencyObject element, WatermarkVisibility value)
        {
            element.SetValue(VisibleWhenProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static WatermarkVisibility GetVisibleWhen(this DependencyObject element)
        {
            return (WatermarkVisibility)element.GetValue(VisibleWhenProperty);
        }

        private static void SetWatermarkAdorner(this DependencyObject element, ContentAdorner value)
        {
            element.SetValue(WatermarkAdornerProperty, value);
        }

        private static ContentAdorner GetWatermarkAdorner(this DependencyObject element)
        {
            return (ContentAdorner)element.GetValue(WatermarkAdornerProperty);
        }

        private static void OnWatermarkTextChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var textBox = o as TextBox;
            if (textBox == null)
            {
                return;
            }

            var text = (string)e.NewValue;
            if (string.IsNullOrEmpty(text))
            {
                textBox.ClearValue(WatermarkAdornerProperty);
            }
            else
            {
                AddOrUpdateAdorner(textBox, null);
            }
        }

        private static void AddOrUpdateAdorner(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            var adorner = textBox.GetWatermarkAdorner();
            if (adorner != null)
            {
                adorner.Content = textBox.GetWatermarkText();
            }
            else
            {
                textBox.SetWatermarkAdorner(new WatermarkAdorner(textBox, textBox.GetWatermarkText()));                
            }
        }

        private static void OnWaterMarkAdornerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = (TextBox)d;
            if (e.OldValue != null)
            {
                ShowWatermarkAdorner(textBox, false, true);
            }

            if (e.NewValue != null)
            {
                ShowWatermarkAdorner(textBox, true, true);
            }
        }

        private static void OnWatermarkedLoaded(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            textBox.RemoveHandler(FrameworkElement.LoadedEvent, OnWatermarkedLoadedHandler);
            ShowWatermarkAdorner(textBox, true, true);
        }

        private static void ShowWatermarkAdorner(TextBox textBox, bool show, bool tryAgain)
        {
            if (!textBox.IsLoaded)
            {
                textBox.UpdateHandler(FrameworkElement.LoadedEvent, OnWatermarkedLoadedHandler);
                return;
            }
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(textBox);
            if (adornerLayer == null)
            {
                if (tryAgain)
                {
                    // try again later, perhaps giving layout a chance to create the adorner layer
                    textBox.Dispatcher.BeginInvoke(DispatcherPriority.Loaded,
                                new DispatcherOperationCallback(ShowAdornerOperation),
                                new object[] { textBox, show });
                }
                return;
            }

            var adorner = textBox.ReadLocalValue(WatermarkAdornerProperty) as Adorner;

            if (show && adorner != null)
            {
                adornerLayer.Add(adorner);
            }
            else if (!show && adorner != null)
            {
                adornerLayer.Remove(adorner);
            }
        }

        private static object ShowAdornerOperation(object arg)
        {
            object[] args = (object[])arg;
            TextBox textBox = (TextBox)args[0];
            bool show = (bool)args[1];

            ShowWatermarkAdorner(textBox, show, false);

            return null;
        }
    }
}
