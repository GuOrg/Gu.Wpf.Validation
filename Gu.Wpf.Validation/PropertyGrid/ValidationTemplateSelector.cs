namespace Gu.Wpf.Validation
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    using Gu.Wpf.Validation.Rules;

    public class ValidationTemplateSelector : DataTemplateSelector
    {
        public DataTemplate BlankTemplate { get; set; }

        public DataTemplate SuccessTemplate { get; set; }

        public DataTemplate RequiredButMissingTemplate { get; set; }

        public DataTemplate ErrorsTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var errors = item as IReadOnlyList<ValidationError>;
            if (errors == null || !errors.Any())
            {
                return SuccessTemplate;
            }

            if (errors.All(x => x.RuleInError.GetType() == typeof(IsRequiredRule)))
            {
                return RequiredButMissingTemplate;
            }

            return ErrorsTemplate;
        }
    }
}
