namespace Gu.Wpf.Validation
{
    using System.Windows;
    using System.Windows.Controls;

    public class SettingStyleSelector : StyleSelector
    {
        public Style TextBlockStyle { get; set; }

        public Style HeaderedContentControlStyle { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            var textBlock = container as TextBlock;
            if (textBlock != null)
            {
                return TextBlockStyle;
            }

            if (container != null && container.GetType() == typeof(HeaderedContentControl))
            {
                return HeaderedContentControlStyle;
            }

            return base.SelectStyle(item, container);
        }
    }
}
