namespace Gu.Wpf.Validation.Rules
{
    using System.Collections;
    using System.Windows.Controls;

    public class GreaterThanMinRule : ConvertedValidationRule
    {
        public override ValidationResult Validate(object value, TextBox target)
        {
            var min = target.GetMin();
            if (min == null)
            {
                return ValidationResult.ValidResult;
            }
            var i = Comparer.Default.Compare(value, min);

            if (i < 0)
            {
                return new ValidationResult(false, new LessThanMinResult(min, value));
            }
            return ValidationResult.ValidResult;
        }
    }
}
