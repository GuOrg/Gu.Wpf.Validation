namespace Gu.Wpf.Validation.Rules
{
    using System.Collections;
    using System.Windows.Controls;

    using Gu.Wpf.Validation.Internals;

    public class MustGreaterThanMinRule : RawValueRule
    {
        protected override ValidationResult Validate(string _, TextBox target)
        {
            var min = target.GetMin();
            if (min == null)
            {
                return ValidationResult.ValidResult;
            }
            
            var rawValue = target.GetRawValue();
            if (rawValue == RawValueTracker.Unset)
            {
                return ValidationResult.ValidResult;
            }

            var i = Comparer.Default.Compare(rawValue, min);

            if (i < 0)
            {
                return new ValidationResult(false, new MustGreaterThanMinError(min, rawValue));
            }
            return ValidationResult.ValidResult;
        }
    }
}
