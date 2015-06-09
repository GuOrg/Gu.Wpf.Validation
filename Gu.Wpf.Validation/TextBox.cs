﻿namespace Gu.Wpf.Validation
{
    using System.Windows;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Media;

    public static class TextBox
    {
        public static readonly DependencyProperty SelectAllOnClickProperty = DependencyProperty.RegisterAttached(
            "SelectAllOnClick",
            typeof(bool),
            typeof(TextBox),
            new PropertyMetadata(false, OnSelectAllOnClickChanged));

        public static readonly DependencyProperty SelectAllOnDoubleClickProperty = DependencyProperty.RegisterAttached(
            "SelectAllOnDoubleClick",
            typeof(bool),
            typeof(TextBox),
            new PropertyMetadata(false, OnSelectAllOnDoubleClickChanged));

        private static readonly string GotKeyboardFocusEventName = "GotKeyboardFocus";
        private static readonly string PreviewMouseLeftButtonDownEventName = "PreviewMouseLeftButtonDown";
        private static readonly string MouseDoubleClickEventName = "MouseDoubleClick";

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(TextBoxBase))]
        public static bool GetSelectAllOnClick(this TextBoxBase o)
        {
            return (bool)o.GetValue(SelectAllOnClickProperty);
        }

        public static void SetSelectAllOnClick(this TextBoxBase o, bool value)
        {
            o.SetValue(SelectAllOnClickProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(TextBoxBase))]
        public static bool GetSelectAllOnDoubleClick(this TextBoxBase element)
        {
            return (bool)element.GetValue(SelectAllOnDoubleClickProperty);
        }

        public static void SetSelectAllOnDoubleClick(this TextBoxBase element, bool value)
        {
            element.SetValue(SelectAllOnDoubleClickProperty, value);
        }

        private static void OnSelectAllOnClickChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = d as TextBoxBase;
            if (box != null)
            {
                var isSelecting = (e.NewValue as bool?).GetValueOrDefault(false);
                if (isSelecting)
                {
                    WeakEventManager<TextBoxBase, KeyboardFocusChangedEventArgs>.AddHandler(box, GotKeyboardFocusEventName, OnKeyboardFocusSelectText);
                    WeakEventManager<TextBoxBase, MouseButtonEventArgs>.AddHandler(box, PreviewMouseLeftButtonDownEventName, OnMouseLeftButtonDown);
                }
                else
                {
                    WeakEventManager<TextBoxBase, KeyboardFocusChangedEventArgs>.RemoveHandler(box, GotKeyboardFocusEventName, OnKeyboardFocusSelectText);
                    WeakEventManager<TextBoxBase, MouseButtonEventArgs>.RemoveHandler(box, PreviewMouseLeftButtonDownEventName, OnMouseLeftButtonDown);
                }
            }
        }

        private static void OnSelectAllOnDoubleClickChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = d as TextBoxBase;
            if (box != null)
            {
                var isSelecting = (e.NewValue as bool?).GetValueOrDefault(false);
                if (isSelecting)
                {
                    WeakEventManager<TextBoxBase, MouseButtonEventArgs>.AddHandler(box, MouseDoubleClickEventName, OnMouseDoubleClick);
                }
                else
                {
                    WeakEventManager<TextBoxBase, MouseButtonEventArgs>.RemoveHandler(box, MouseDoubleClickEventName, OnMouseDoubleClick);
                }
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

        private static void OnKeyboardFocusSelectText(object sender, KeyboardFocusChangedEventArgs e)
        {
            var box = e.OriginalSource as System.Windows.Controls.TextBox;
            if (box != null)
            {
                box.SelectAll();
            }
        }
    }
}
