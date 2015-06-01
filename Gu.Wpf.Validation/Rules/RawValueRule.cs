namespace Gu.Wpf.Validation.Rules
{
    using System;
    using System.Globalization;
    using System.Windows.Controls;
    using System.Windows.Data;

    public abstract class RawValueRule : ValidationRule
    {
        protected RawValueRule()
            : base(ValidationStep.RawProposedValue, true)
        {
        }

        public abstract ValidationResult Validate(string text, TextBox textBox);

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            throw new InvalidOperationException();
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo, BindingExpressionBase owner)
        {
            var s = value as string;
            var textBox = (TextBox)owner.Target;
            return Validate(s, textBox);
        }
    }
}