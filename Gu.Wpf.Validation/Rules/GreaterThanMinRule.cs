namespace Gu.Wpf.Validation.Rules
{
    using System.Collections;
    using System.Windows.Controls;

    using Gu.Wpf.Validation.Internals;

    public class GreaterThanMinRule : RawValueRule
    {
        public override ValidationResult Validate(string _, TextBox target)
        {
            var min = target.GetMin();
            if (min == null)
            {
                return ValidationResult.ValidResult;
            }
            
            var rawValue = target.GetRawValue();
            if (rawValue == TextBoxExt.Unset)
            {
                return ValidationResult.ValidResult;
            }

            var i = Comparer.Default.Compare(rawValue, min);

            if (i < 0)
            {
                return new ValidationResult(false, new LessThanMinResult(min, rawValue));
            }
            return ValidationResult.ValidResult;
        }
    }
}
