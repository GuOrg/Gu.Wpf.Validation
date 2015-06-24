namespace Gu.Wpf.Validation.Rules
{
    using System.Collections;
    using System.Windows.Controls;

    using Gu.Wpf.Validation.Internals;

    public class MustBeLessThanMaxRule : RawValueRule
    {
        protected override ValidationResult Validate(string _, TextBox target)
        {
            var max = target.GetMax();
            if (max == null)
            {
                return ValidationResult.ValidResult;
            }

            var rawValue = target.GetRawValue();
            if (rawValue == RawValueTracker.Unset)
            {
                return ValidationResult.ValidResult;
            }

            var i = Comparer.Default.Compare(rawValue, max);

            if (i > 0)
            {
                return new ValidationResult(false, new MustBeLessThanMaxError(max, rawValue));
            }
            return ValidationResult.ValidResult;
        }
    }
}