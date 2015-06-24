namespace Gu.Wpf.Validation.Demo.Wpf
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using Rules;

    public class ValidationTemplateSelector : DataTemplateSelector
    {
        public DataTemplate NoErrorTemplate { get; set; }

        public DataTemplate MissingTemplate { get; set; }

        public DataTemplate ErrorsTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var errors = item as IReadOnlyList<ValidationError>;
            if (errors == null || !errors.Any())
            {
                return NoErrorTemplate;
            }
            if (errors.Any(x => x.RuleInError.GetType() == typeof(IsRequiredRule)))
            {
                return MissingTemplate;
            }
            return ErrorsTemplate;
            return base.SelectTemplate(item, container);
        }
    }
}
