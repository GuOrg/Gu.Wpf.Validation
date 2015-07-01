namespace Gu.Wpf.Validation
{
    using System.Windows;
    using System.Windows.Controls;

    public static class PropertyGrid
    {
        public static readonly DependencyProperty ShowHelpTextProperty = DependencyProperty.RegisterAttached(
            "ShowHelpText",
            typeof(bool),
            typeof(PropertyGrid),
            new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty ShowSymbolsProperty = DependencyProperty.RegisterAttached(
            "ShowSymbols",
            typeof(bool),
            typeof(PropertyGrid),
            new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty ShowErrorTextProperty = DependencyProperty.RegisterAttached(
            "ShowErrorText",
            typeof(bool),
            typeof(PropertyGrid),
            new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty RowMarginProperty = DependencyProperty.RegisterAttached(
            "RowMargin",
            typeof(Thickness),
            typeof(PropertyGrid),
            new PropertyMetadata(default(Thickness)));

        public static readonly DependencyProperty ContentMarginProperty = DependencyProperty.RegisterAttached(
            "ContentMargin",
            typeof(double),
            typeof(PropertyGrid),
            new PropertyMetadata(default(double)));

        public static void SetShowHelpText(this Control element, bool value)
        {
            element.SetValue(ShowHelpTextProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(ItemsControl))]
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static bool GetShowHelpText(this Control element)
        {
            return (bool)element.GetValue(ShowHelpTextProperty);
        }

        public static void SetShowSymbols(this Control element, bool value)
        {
            element.SetValue(ShowSymbolsProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(ItemsControl))]
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static bool GetShowSymbols(this Control element)
        {
            return (bool)element.GetValue(ShowSymbolsProperty);
        }

        public static void SetShowErrorText(this Control element, bool value)
        {
            element.SetValue(ShowErrorTextProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(ItemsControl))]
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static bool GetShowErrorText(this Control element)
        {
            return (bool)element.GetValue(ShowErrorTextProperty);
        }

        public static void SetRowMargin(this Control element, Thickness value)
        {
            element.SetValue(RowMarginProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(ItemsControl))]
        //[AttachedPropertyBrowsableForType(typeof(HeaderedContentControl))]
        public static Thickness GetRowMargin(this Control element)
        {
            return (Thickness)element.GetValue(RowMarginProperty);
        }

        public static void SetContentMargin(this Control element, double value)
        {
            element.SetValue(ContentMarginProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(ItemsControl))]
        //[AttachedPropertyBrowsableForType(typeof(HeaderedContentControl))]
        public static double GetContentMargin(this Control element)
        {
            return (double)element.GetValue(ContentMarginProperty);
        }
    }
}
