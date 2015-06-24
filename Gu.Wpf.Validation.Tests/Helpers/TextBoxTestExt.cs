namespace Gu.Wpf.Validation.Tests.Helpers
{
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
            TextCompositionManager.StartComposition(new TextComposition(InputManager.Current, textBox, text));
        }
    }
}
