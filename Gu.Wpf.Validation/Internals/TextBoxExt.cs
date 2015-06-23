namespace Gu.Wpf.Validation.Internals
{
    using System;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    internal static class TextBoxExt
    {
        private static readonly DependencyProperty IsUpdatingProperty = DependencyProperty.RegisterAttached(
            "IsUpdating",
            typeof(int),
            typeof(TextBoxExt),
            new PropertyMetadata(0));


        internal static void SetIsUpdating(this TextBox element, bool value)
        {
            var i = (int)element.GetValue(IsUpdatingProperty);
            if (value)
            {
                element.SetValue(IsUpdatingProperty, i + 1);
            }
            else
            {
                element.SetValue(IsUpdatingProperty, i - 1);
            }
        }

        internal static bool GetIsUpdating(this TextBox element)
        {
            var value = (int)element.GetValue(IsUpdatingProperty);
            return value > 0;
        }

        internal static void SetTextUndoable(this TextBox textBox, string text)
        {
            if (text == null)
            {
                if (String.IsNullOrEmpty(textBox.Text))
                {
                    return;
                }
                text = String.Empty;
            }
            // http://stackoverflow.com/questions/27083236/change-the-text-in-a-textbox-with-text-binding-sometext-so-it-is-undoable/27083548?noredirect=1#comment42677255_27083548
            // Dunno if nice, testing it for now
            var caretIndex = textBox.CaretIndex;
            var atEnd = caretIndex == textBox.Text.Length;
            textBox.SelectAll();
            textBox.SelectedText = text;
            textBox.Select(0, 0);
            if (atEnd)
            {
                textBox.CaretIndex = textBox.Text.Length;
            }
            else
            {
                textBox.CaretIndex = caretIndex;
            }
        }

        internal static object GetSourceValue(this TextBox textBox)
        {
            var expression = BindingOperations.GetBindingExpression(textBox, Input.ValueProperty);
            if (expression == null)
            {
                return null;
            }
            var source = expression.ResolvedSource;
            if (source == null)
            {
                return null;
            }
            var propertyInfo = source.GetType().GetProperty(expression.ResolvedSourcePropertyName);
            if (propertyInfo == null)
            {
                return null;
            }
            return propertyInfo.GetValue(source);
        }

        internal static void ResetValue(this TextBox textBox)
        {
            var sourceValue = textBox.GetSourceValue();
            textBox.SetCurrentValue(Input.ValueProperty, sourceValue);
        }

        internal static void Update(this TextBox textBox, DependencyProperty property, object value)
        {
            textBox.SetIsUpdating(true);
            textBox.SetCurrentValue(property, value);
            textBox.SetIsUpdating(false);
        }
    }
}
