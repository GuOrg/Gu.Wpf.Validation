namespace Gu.Wpf.Validation
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Media;

    using Gu.Wpf.Validation.Internals;

    public static partial class Input
    {
        public static readonly DependencyProperty SelectAllOnClickProperty = DependencyProperty.RegisterAttached(
            "SelectAllOnClick",
            typeof(bool),
            typeof(Input),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits, OnSelectAllOnClickChanged));

        public static readonly DependencyProperty SelectAllOnDoubleClickProperty = DependencyProperty.RegisterAttached(
            "SelectAllOnDoubleClick",
            typeof(bool),
            typeof(Input),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits, OnSelectAllOnDoubleClickChanged));
        private static readonly RoutedEventHandler OnKeyboardFocusSelectTextHandler = new RoutedEventHandler(OnKeyboardFocusSelectText);
        private static readonly MouseButtonEventHandler OnMouseLeftButtonDownHandler = new MouseButtonEventHandler(OnMouseLeftButtonDown);
        private static readonly MouseButtonEventHandler OnMouseDoubleClickHandler = new MouseButtonEventHandler(OnMouseDoubleClick);

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static bool GetSelectAllOnClick(this UIElement o)
        {
            return (bool)o.GetValue(SelectAllOnClickProperty);
        }

        public static void SetSelectAllOnClick(this UIElement o, bool value)
        {
            o.SetValue(SelectAllOnClickProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static bool GetSelectAllOnDoubleClick(this UIElement element)
        {
            return (bool)element.GetValue(SelectAllOnDoubleClickProperty);
        }

        public static void SetSelectAllOnDoubleClick(this UIElement element, bool value)
        {
            element.SetValue(SelectAllOnDoubleClickProperty, value);
        }

        private static void OnSelectAllOnClickChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = d as TextBoxBase;
            if (box == null)
            {
                return;
            }
            var isSelecting = (e.NewValue as bool?).GetValueOrDefault(false);
            if (isSelecting)
            {
                box.UpdateHandler(UIElement.GotKeyboardFocusEvent, OnKeyboardFocusSelectTextHandler);
                box.UpdateHandler(UIElement.PreviewMouseLeftButtonDownEvent, OnMouseLeftButtonDownHandler);
            }
            else
            {
                box.RemoveHandler(UIElement.GotKeyboardFocusEvent, OnKeyboardFocusSelectTextHandler);
                box.RemoveHandler(UIElement.PreviewMouseLeftButtonDownEvent, OnMouseLeftButtonDownHandler);
            }
        }

        private static void OnSelectAllOnDoubleClickChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = d as TextBoxBase;
            if (box == null)
            {
                return;
            }
            var isSelecting = (e.NewValue as bool?).GetValueOrDefault(false);
            if (isSelecting)
            {
                box.UpdateHandler(Control.MouseDoubleClickEvent, OnMouseDoubleClickHandler);
            }
            else
            {
                box.RemoveHandler(Control.MouseDoubleClickEvent, OnMouseDoubleClickHandler);
            }
        }

        private static void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var textBoxBase = GetParentFromVisualTree(e.OriginalSource);
            if (textBoxBase == null)
            {
                return;
            }
            textBoxBase.SelectAll();
        }

        private static void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var textBoxBase = GetParentFromVisualTree(e.OriginalSource);
            if (textBoxBase == null)
            {
                return;
            }

            var box = textBoxBase;
            if (!box.IsKeyboardFocusWithin)
            {
                box.Focus();
                e.Handled = true;
            }
        }

        private static TextBoxBase GetParentFromVisualTree(object source)
        {
            DependencyObject parent = source as UIElement;
            while (parent != null && !(parent is TextBoxBase))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            return parent as TextBoxBase;
        }

        private static void OnKeyboardFocusSelectText(object sender, RoutedEventArgs e)
        {
            var box = e.OriginalSource as TextBox;
            if (box != null)
            {
                box.SelectAll();
            }
        }
    }
}
