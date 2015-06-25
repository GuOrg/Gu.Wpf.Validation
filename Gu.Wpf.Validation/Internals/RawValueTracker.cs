namespace Gu.Wpf.Validation.Internals
{
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;

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

        private static readonly DependencyProperty IsReceivingUserInputProperty = DependencyProperty.RegisterAttached(
            "IsReceivingUserInput",
            typeof(bool),
            typeof(RawValueTracker),
            new PropertyMetadata(default(bool)));

        private static readonly RoutedEventHandler OnUserInputHandler = new RoutedEventHandler(OnUserInput);
        private static readonly RoutedEventHandler RoutedEventHandler = new RoutedEventHandler(OnTextChanged);

        public static void TrackUserInput(TextBox textBox)
        {
            textBox.UpdateHandler(UIElement.PreviewKeyDownEvent, OnUserInputHandler);
            textBox.UpdateHandler(UIElement.PreviewTextInputEvent, OnUserInputHandler);
            textBox.UpdateHandler(TextBoxBase.TextChangedEvent, RoutedEventHandler);
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
            return element.GetValue(RawValueProperty);
        }

        private static void SetRawValueSource(this DependencyObject element, RawValueSource value)
        {
            element.SetValue(RawValueSourcePropertyKey, value);
        }

        public static RawValueSource GetRawValueSource(this DependencyObject element)
        {
            return (RawValueSource)element.GetValue(RawValueSourceProperty);
        }

        private static void SetIsReceivingUserInput(this DependencyObject element, bool value)
        {
            element.SetValue(IsReceivingUserInputProperty, value);
        }

        public static bool GetIsReceivingUserInput(this DependencyObject element)
        {
            return (bool)element.GetValue(IsReceivingUserInputProperty);
        }

        private static void OnRawValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == Unset)
            {
                return;
            }
            var textBox = (TextBox)d;
            if (textBox.GetIsUpdating())
            {
                return;
            }
            Debug.Assert(!textBox.GetIsUpdating());
            var rawText = textBox.GetRawText();
            string rawString;
            if (e.NewValue == null)
            {
                rawString = string.Empty;
            }
            else
            {
                var converter = textBox.GetStringConverter();
                if (converter == null)
                {
                    return;
                }
                rawString = converter.ToRawString(e.NewValue, textBox);
            }

            if (rawString != rawText)
            {
                Debug.WriteLine(@"OnRawValueChanged({0}) -> textBox.SetRawText({1}) textBox.Text: {2}", e.NewValue.ToDebugString(), rawString.ToDebugString(), textBox.Text.ToDebugString());
                textBox.SetRawText(rawString);
                if (textBox.GetIsReceivingUserInput())
                {
                    Debug.WriteLine(@"OnRawValueChanged({0}) -> SetRawValueSource(textBox, RawValueSource.User) textBox.Text: {1}", e.NewValue.ToDebugString(), textBox.Text.ToDebugString());
                    SetRawValueSource(textBox, RawValueSource.User);
                    textBox.SetIsReceivingUserInput(false);
                }
                else
                {
                    Debug.WriteLine(@"OnRawValueChanged({0}) -> textBox.SetRawValueSource(RawValueSource.Binding) textBox.Text: {1}", e.NewValue.ToDebugString(), textBox.Text.ToDebugString());
                    textBox.SetRawValueSource(RawValueSource.Binding);
                }
            }
            textBox.RaiseEvent(Input.ValidationDirtyArgs);
            textBox.RaiseEvent(Input.FormattingDirtyArgs);
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
                    Debug.WriteLine(@"OnRawTextChanged: Text: {0}, Value: {1}, Source: {2}", e.NewValue, value, RawValueSource.User);
                }
            }
            else
            {
                textBox.SetRawValue(Unset);
                textBox.SetRawValueSource(RawValueSource.User);
                Debug.WriteLine(@"OnRawTextChanged: Text: {0}, Value: {1}, Source: {2}", e.NewValue, value, RawValueSource.User);
            }
            textBox.RaiseEvent(Input.ValidationDirtyArgs);
            textBox.RaiseEvent(Input.FormattingDirtyArgs);
        }

        internal static void Update(TextBox textBox)
        {
            var rawValue = textBox.GetRawValue();
            if (rawValue == Unset)
            {
                var converter = textBox.GetStringConverter();
                object value;
                var rawText = textBox.GetRawText();
                if (converter.TryParse(rawText, textBox, out value))
                {
                    textBox.SetIsUpdating(true);
                    textBox.SetRawValue(value);
                    textBox.SetIsUpdating(false);
                    textBox.RaiseEvent(Input.ValidationDirtyArgs);
                }
            }
        }

        private static void OnUserInput(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            textBox.SetIsReceivingUserInput(true);
        }

        private static void OnTextChanged(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            if (textBox.GetIsReceivingUserInput())
            {
                textBox.SetRawText(textBox.Text);
            }
            textBox.SetIsReceivingUserInput(false);
        }
    }
}
