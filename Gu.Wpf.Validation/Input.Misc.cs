namespace Gu.Wpf.Validation
{
    using System.Windows;
    using System.Windows.Controls;

    public static partial class Input
    {
        public static readonly DependencyProperty HelpContentProperty = DependencyProperty.RegisterAttached(
            "HelpContent", 
            typeof (object), 
            typeof (Input),
            new FrameworkPropertyMetadata(default(object), FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty HelpTemplateProperty = DependencyProperty.RegisterAttached(
            "HelpTemplate",
            typeof(DataTemplate),
            typeof(Input),
            new FrameworkPropertyMetadata(default(DataTemplate), FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty HelpTemplateSelectorProperty = DependencyProperty.RegisterAttached(
            "HelpTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(Input),
            new FrameworkPropertyMetadata(default(DataTemplateSelector), FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty ValidationSymbolTemplateSelectorProperty = DependencyProperty.RegisterAttached(
            "ValidationSymbolTemplateSelector", 
            typeof (DataTemplateSelector),
            typeof (Input), 
            new FrameworkPropertyMetadata(default(DataTemplateSelector), FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty ErrorSymbolTemplateSelectorProperty = DependencyProperty.RegisterAttached(
            "ErrorSymbolTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(Input),
            new FrameworkPropertyMetadata(default(DataTemplateSelector), FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty ErrorTemplateProperty = DependencyProperty.RegisterAttached(
            "ErrorTemplate",
            typeof(DataTemplate),
            typeof(Input),
            new FrameworkPropertyMetadata(default(DataTemplate), FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty ErrorTemplateSelectorProperty = DependencyProperty.RegisterAttached(
            "ErrorTemplateSelector", 
            typeof (DataTemplateSelector),
            typeof (Input),
            new FrameworkPropertyMetadata(default(DataTemplateSelector), FrameworkPropertyMetadataOptions.Inherits));

        public static void SetHelpContent(DependencyObject element, object value)
        {
            element.SetValue(HelpContentProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(HeaderedContentControl))]
        public static object GetHelpContent(DependencyObject element)
        {
            return element.GetValue(HelpContentProperty);
        }

        public static void SetHelpTemplate(this DependencyObject element, DataTemplate value)
        {
            element.SetValue(HelpTemplateProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(HeaderedContentControl))]
        public static DataTemplate GetHelpTemplate(this DependencyObject element)
        {
            return (DataTemplate)element.GetValue(HelpTemplateProperty);
        }

        public static void SetHelpTemplateSelector(this DependencyObject element, DataTemplateSelector value)
        {
            element.SetValue(HelpTemplateSelectorProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(HeaderedContentControl))]
        public static DataTemplateSelector GetHelpTemplateSelector(this DependencyObject element)
        {
            return (DataTemplateSelector)element.GetValue(HelpTemplateSelectorProperty);
        }

        public static void SetValidationSymbolTemplateSelector(DependencyObject element, DataTemplateSelector value)
        {
            element.SetValue(ValidationSymbolTemplateSelectorProperty, value);
        }

        public static DataTemplateSelector GetValidationSymbolTemplateSelector(DependencyObject element)
        {
            return (DataTemplateSelector)element.GetValue(ValidationSymbolTemplateSelectorProperty);
        }


        public static void SetErrorTemplate(this DependencyObject element, DataTemplate value)
        {
            element.SetValue(ErrorTemplateProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(ItemsControl))]
        [AttachedPropertyBrowsableForType(typeof(HeaderedContentControl))]
        public static DataTemplate GetErrorTemplate(this DependencyObject element)
        {
            return (DataTemplate)element.GetValue(ErrorTemplateProperty);
        }

        public static void SetErrorTemplateSelector(this DependencyObject element, DataTemplateSelector value)
        {
            element.SetValue(ErrorTemplateSelectorProperty, value);
        }

                [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(ItemsControl))]
        [AttachedPropertyBrowsableForType(typeof(HeaderedContentControl))]
        public static DataTemplateSelector GetErrorTemplateSelector(this DependencyObject element)
        {
            return (DataTemplateSelector)element.GetValue(ErrorTemplateSelectorProperty);
        }

        public static void SetErrorSymbolTemplateSelector(this DependencyObject element, DataTemplateSelector value)
        {
            element.SetValue(ErrorSymbolTemplateSelectorProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(ItemsControl))]
        [AttachedPropertyBrowsableForType(typeof(HeaderedContentControl))]
        public static DataTemplateSelector GetErrorSymbolTemplateSelector(this DependencyObject element)
        {
            return (DataTemplateSelector)element.GetValue(ErrorSymbolTemplateSelectorProperty);
        }
    }
}
