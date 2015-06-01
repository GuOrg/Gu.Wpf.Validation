namespace Gu.Wpf.Validation.Rules
{
    using System;
    using System.Globalization;
    using System.Windows.Controls;
    using System.Windows.Data;

    public abstract class ConvertedValidationRule : ValidationRule
    {
        protected ConvertedValidationRule()
            : base(ValidationStep.ConvertedProposedValue, true)
        {
        }

        public abstract ValidationResult Validate(object value, TextBox target);

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            throw new NotImplementedException();
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo, BindingExpressionBase owner)
        {
            value = GetValueOrFirst(value);
            if (value == null)
            {
                return ValidationResult.ValidResult;
            }
            return Validate(value, (TextBox) owner.Target);
        }

        private static object GetValueOrFirst(object value)
        {
            if (value == null)
            {
                return null;
            }
            var objects = value as object[];
            if (objects == null)
            {
                return value;
            }
            return objects[0];
        }
    }
}