namespace Gu.Wpf.Validation.Tests.Helpers
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public static class TextBoxTestExt
    {
        public static void WriteText(this TextBox textBox, string text, bool overwrite = true)
        {
            if (overwrite)
            {
                textBox.SelectAll();
            }
            var composition = new TextComposition(InputManager.Current, textBox, text);
            TextCompositionManager.StartComposition(composition);
            if (text == "")
            {
                textBox.Text = "";
            }
        }

        public static void RaiseLoadedEvent(this TextBox textBox)
        {
            textBox.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));
        }

        public static void RaiseLostFocusEvent(this TextBox textBox)
        {
            textBox.RaiseEvent(new RoutedEventArgs(UIElement.LostFocusEvent));
        }
    }
}
