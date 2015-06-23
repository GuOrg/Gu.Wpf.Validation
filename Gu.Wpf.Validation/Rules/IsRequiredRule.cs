namespace Gu.Wpf.Validation.Rules
{
    using System.Windows.Controls;

    public class IsRequiredRule : RawValueRule
    {
        protected override ValidationResult Validate(string text, TextBox textBox)
        {
            if (textBox.GetIsRequired() && string.IsNullOrEmpty(text))
            {
                return new ValidationResult(false, new RequiredResult());
            }
            return ValidationResult.ValidResult;
        }
    }

    public class RequiredResult
    {
        public override string ToString()
        {
            return "Value is required";
        }
    }
}
