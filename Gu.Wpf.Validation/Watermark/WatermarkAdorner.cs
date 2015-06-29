namespace Gu.Wpf.Validation
{
    using System.Windows;
    using System.Windows.Controls;

    public class WatermarkAdorner : ContentAdorner
    {
        static WatermarkAdorner()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WatermarkAdorner), new FrameworkPropertyMetadata(typeof(WatermarkAdorner)));
        }

        public WatermarkAdorner(TextBox textBox, object content)
            : base(textBox, content)
        {
            TextBox = textBox;
        }

        public TextBox TextBox { get; private set; }
    }
}