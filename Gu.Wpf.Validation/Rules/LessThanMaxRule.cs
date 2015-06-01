namespace Gu.Wpf.Validation.Rules
{
    using System.Collections;
    using System.Windows.Controls;

    public class LessThanMaxRule : ConvertedValidationRule
    {
        public override ValidationResult Validate(object value, TextBox target)
        {
            var max = target.GetMax();
            if (max == null)
            {
                return ValidationResult.ValidResult;
            }
            var i = Comparer.Default.Compare(value, max);

            if (i > 0)
            {
                return new ValidationResult(false, new GreaterThanMaxResult(max, value));
            }
            return ValidationResult.ValidResult;
        }
    }
}