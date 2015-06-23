namespace Gu.Wpf.Validation.Internals
{
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;

    public static class RawValueTracker
    {
        internal static readonly object Unset = "RawValueTracker.Unset";

        private static readonly DependencyPropertyKey RawTextPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "RawText",
            typeof(string),
            typeof(TextBoxExt),
            new PropertyMetadata(null, OnRawTextChanged));

        private static readonly DependencyProperty RawTextProperty = RawTextPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey RawValuePropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "RawValue",
            typeof(object),
            typeof(TextBoxExt),
            new PropertyMetadata(Unset, null, OnRawValueCoerce));

        internal static DependencyProperty RawValueProperty = RawValuePropertyKey.DependencyProperty;


        internal static void SetRawText(this TextBox element, string value)
        {
            element.SetValue(RawTextPropertyKey, value);
        }

        internal static string GetRawText(this TextBox element)
        {
            return (string)element.GetValue(RawTextProperty);
        }

        private static void SetRawValue(this DependencyObject element, object value)
        {
            element.SetValue(RawValuePropertyKey, value);
        }

        internal static object GetRawValue(this DependencyObject element)
        {
            return (object)element.GetValue(RawValueProperty);
        }

        private static void OnRawTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = (TextBox)d;
            Debug.Assert(!textBox.GetIsUpdating());
            textBox.SetRawValue(Unset);
        }

        private static object OnRawValueCoerce(DependencyObject d, object value)
        {
            var textBox = (TextBox)d;
            if (value != Unset)
            {
                return value;
            }
            var converter = textBox.GetStringConverter();
            if (converter == null)
            {
                return Unset;
            }
            var rawText = textBox.GetRawText();
            if (converter.TryParse(rawText, textBox, out value))
            {
                return value;
            }
            return Unset;
        }

        internal static void UpdateRawValue(this TextBox textBox)
        {
            var rawValue = textBox.GetRawValue();
            textBox.SetRawValue(rawValue);
        }

        internal static void ResetValue(this TextBox textBox)
        {
            var sourceValue = textBox.GetSourceValue();
            textBox.SetCurrentValue(Input.ValueProperty, sourceValue);
        }

    }
}
