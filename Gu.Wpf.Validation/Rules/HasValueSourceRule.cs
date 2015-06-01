namespace Gu.Wpf.Validation.Rules
{
    using System;
    using System.Globalization;
    using System.Windows.Controls;
    using System.Windows.Data;

    public class HasValueSourceRule : ValidationRule
    {
        public HasValueSourceRule()
            : base(ValidationStep.RawProposedValue, false)
        {
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            throw new NotImplementedException();
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo, BindingExpressionBase owner)
        {
            throw new NotImplementedException("Do we want to assert that binding source is present?");
        }
    }
}