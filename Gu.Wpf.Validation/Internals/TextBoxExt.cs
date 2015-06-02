namespace Gu.Wpf.Validation.Internals
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    internal static class TextBoxExt
    {
        internal static readonly DependencyProperty IsUpdatingProperty = DependencyProperty.RegisterAttached(
            "IsUpdating",
            typeof(bool),
            typeof(TextBoxExt),
            new PropertyMetadata(default(bool)));

        internal static readonly DependencyProperty OldCultureProperty = DependencyProperty.RegisterAttached(
            "OldCulture",
            typeof(IFormatProvider),
            typeof(TextBoxExt),
            new PropertyMetadata(null));

        internal static readonly DependencyProperty RawTextProperty = DependencyProperty.RegisterAttached(
            "RawText",
            typeof(string),
            typeof(TextBoxExt),
            new PropertyMetadata(null));

        internal static void SetIsUpdating(this TextBox element, bool value)
        {
            element.SetValue(IsUpdatingProperty, value);
        }

        internal static bool GetIsUpdating(this TextBox element)
        {
            return (bool)element.GetValue(IsUpdatingProperty);
        }

        internal static void SetOldCulture(this TextBox element, IFormatProvider value)
        {
            element.SetValue(OldCultureProperty, value);
        }

        internal static IFormatProvider GetOldCulture(this TextBox element)
        {
            return (IFormatProvider)element.GetValue(OldCultureProperty);
        }

        internal static void SetRawText(this TextBox element, string value)
        {
            element.SetValue(RawTextProperty, value);
        }

        internal static string GetRawText(this TextBox element)
        {
            return (string)element.GetValue(RawTextProperty);
        }

        internal static void ResetValue(this TextBox textBox)
        {
            var sourceValue = textBox.GetSourceValue();
            textBox.SetCurrentValue(Input.ValueProperty, sourceValue);
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
            textBox.SelectAll();
            textBox.SelectedText = text;
            textBox.Select(0, 0);
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

        internal static void Update(this TextBox textBox, DependencyProperty property, object value)
        {
            textBox.SetIsUpdating(true);
            textBox.SetCurrentValue(property, value);
            textBox.SetIsUpdating(false);
        }
    }
}
