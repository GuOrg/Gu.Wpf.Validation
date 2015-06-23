namespace Gu.Wpf.Validation.Internals
{
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Documents;
    using System.Windows.Input;

    using Gu.Wpf.Validation.StringConverters;

    public static class RawValueTracker
    {
        internal static readonly object Unset = "RawValueTracker.Unset";

        private static readonly DependencyPropertyKey RawTextPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "RawText",
            typeof(string),
            typeof(RawValueTracker),
            new PropertyMetadata(null, OnRawTextChanged));

        public static readonly DependencyProperty RawTextProperty = RawTextPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey RawValuePropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "RawValue",
            typeof(object),
            typeof(RawValueTracker),
            new PropertyMetadata(Unset, OnRawValueChanged));

        public static readonly DependencyProperty RawValueProperty = RawValuePropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey RawValueSourcePropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "RawValueSource",
            typeof(RawValueSource),
            typeof(RawValueTracker),
            new PropertyMetadata(RawValueSource.None));

        public static readonly DependencyProperty RawValueSourceProperty = RawValueSourcePropertyKey.DependencyProperty;

        public static void TrackUserInput(TextBox textBox)
        {
            //textBox.AddHandler(TextBoxBase.TextChangedEvent, new TextChangedEventHandler(OnTextChanged), true);
            TextCompositionManager.AddPreviewTextInputHandler(textBox, OnTextInput);
        }

        private static void SetRawText(this TextBox element, string value)
        {
            element.SetValue(RawTextPropertyKey, value);
        }

        internal static string GetRawText(this TextBox element)
        {
            return (string)element.GetValue(RawTextProperty);
        }

        internal static void SetRawValue(this DependencyObject element, object value)
        {
            element.SetValue(RawValuePropertyKey, value);
        }

        internal static object GetRawValue(this DependencyObject element)
        {
            return (object)element.GetValue(RawValueProperty);
        }

        private static void SetRawValueSource(this DependencyObject element, RawValueSource value)
        {
            element.SetValue(RawValueSourcePropertyKey, value);
        }

        public static RawValueSource GetRawValueSource(this DependencyObject element)
        {
            return (RawValueSource)element.GetValue(RawValueSourceProperty);
        }

        private static void OnRawValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == Unset)
            {
                return;
            }
            var textBox = (TextBox)d;
            Debug.Assert(!textBox.GetIsUpdating());
            var rawText = textBox.GetRawText();
            if (e.NewValue == null)
            {
                if (!string.IsNullOrEmpty(rawText))
                {
                    textBox.SetRawText(string.Empty);
                    textBox.SetRawValueSource(RawValueSource.Binding);
                    Debug.WriteLine("OnRawValueChanged: Text: {0}, Value: {1}, Source: {2}", "string.Empty", e.NewValue, RawValueSource.Binding);
                }
            }
            else
            {
                var converter = textBox.GetStringConverter();
                if (converter == null)
                {
                    return;
                }
                var rawString = converter.ToRawString(e.NewValue, textBox);
                if (rawString != rawText)
                {
                    textBox.SetRawText(rawString);
                    textBox.SetRawValueSource(RawValueSource.Binding);
                    Debug.WriteLine("OnRawValueChanged: Text: {0}, Value: {1}, Source: {2}", rawString, e.NewValue, RawValueSource.Binding);
                }
            }
        }

        private static void OnRawTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = (TextBox)d;
            if (textBox.GetIsUpdating())
            {
                return;
            }
            var converter = textBox.GetStringConverter();
            if (converter == null)
            {
                return;
            }
            object value;
            if (converter.TryParse(e.NewValue, textBox, out value))
            {
                var rawValue = textBox.GetRawValue();
                if (!Equals(rawValue, value))
                {
                    textBox.SetRawValue(value);
                    textBox.SetRawValueSource(RawValueSource.User);
                    Debug.WriteLine("OnRawTextChanged: Text: {0}, Value: {1}, Source: {2}", e.NewValue, value, RawValueSource.User);
                }
            }
            else
            {
                textBox.SetRawValue(Unset);
                textBox.SetRawValueSource(RawValueSource.User);
                Debug.WriteLine("OnRawTextChanged: Text: {0}, Value: {1}, Source: {2}", e.NewValue, value, RawValueSource.User);
            }
        }

        internal static void Update(TextBox textBox)
        {
            var rawValue = textBox.GetRawValue();
            if (rawValue == Unset)
            {
                var converter = textBox.GetStringConverter();
                object value;
                if (converter.TryParse(textBox.GetRawText(), textBox, out value))
                {
                    textBox.SetRawValue(value);
                }
            }
            else
            {
                textBox.SetRawValue(rawValue);
            }
        }


        private static void OnTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = (TextBox)sender;
            if (!textBox.GetIsUpdating())
            {
                SetRawText(textBox, GetText(textBox, e));
            }
        }

        private static string GetText(TextBox txt, TextCompositionEventArgs e)
        {
            int selectionStart = txt.SelectionStart;
            if (txt.Text.Length < selectionStart)
            {
                selectionStart = txt.Text.Length;
            }

            int selectionLength = txt.SelectionLength;
            if (txt.Text.Length < selectionStart + selectionLength)
            {
                selectionLength = txt.Text.Length - selectionStart;
            }

            var realtext = txt.Text.Remove(selectionStart, selectionLength);

            int caretIndex = txt.CaretIndex;
            if (realtext.Length < caretIndex)
            {
                caretIndex = realtext.Length;
            }

            var newtext = realtext.Insert(caretIndex, e.Text);

            return newtext;
        }
    }
}
