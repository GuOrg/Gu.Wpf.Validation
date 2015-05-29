using System.Globalization;
using System.Windows.Controls;

namespace ValidationScopeBox
{
    public class MehRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var s = value as string;
            if (s == null)
            {
                return ValidationResult.ValidResult;
            }
            if (s.Contains("s"))
            {
                return new ValidationResult(false,"Contains s");
            }
            return ValidationResult.ValidResult;
        }
    }
}
