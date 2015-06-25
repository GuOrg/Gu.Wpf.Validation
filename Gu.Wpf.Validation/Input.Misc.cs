namespace Gu.Wpf.Validation
{
    using System.Windows;
    using System.Windows.Controls;

    public static partial class Input
    {
        public static readonly DependencyProperty HelpTextProperty = DependencyProperty.RegisterAttached(
            "HelpText", 
            typeof (string), 
            typeof (Input),
            new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty ValidationSymbolTemplateSelectorProperty = DependencyProperty.RegisterAttached(
            "ValidationSymbolTemplateSelector", 
            typeof (DataTemplateSelector),
            typeof (Input), 
            new FrameworkPropertyMetadata(default(DataTemplateSelector), FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty ErrorHelpTemplateSelectorProperty = DependencyProperty.RegisterAttached(
            "ErrorHelpTemplateSelector", 
            typeof (DataTemplateSelector),
            typeof (Input),
            new FrameworkPropertyMetadata(default(DataTemplateSelector), FrameworkPropertyMetadataOptions.Inherits));

        public static void SetHelpText(DependencyObject element, string value)
        {
            element.SetValue(HelpTextProperty, value);
        }

        public static string GetHelpText(DependencyObject element)
        {
            return (string) element.GetValue(HelpTextProperty);
        }

        public static void SetValidationSymbolTemplateSelector(DependencyObject element, DataTemplateSelector value)
        {
            element.SetValue(ValidationSymbolTemplateSelectorProperty, value);
        }

        public static DataTemplateSelector GetValidationSymbolTemplateSelector(DependencyObject element)
        {
            return (DataTemplateSelector)element.GetValue(ValidationSymbolTemplateSelectorProperty);
        }

        public static void SetErrorHelpTemplateSelector(DependencyObject element, DataTemplateSelector value)
        {
            element.SetValue(ErrorHelpTemplateSelectorProperty, value);
        }

        public static DataTemplateSelector GetErrorHelpTemplateSelector(DependencyObject element)
        {
            return (DataTemplateSelector)element.GetValue(ErrorHelpTemplateSelectorProperty);
        }
    }
}
