namespace Gu.Wpf.Validation.Rules
{
    using System.Windows.Controls;

    public class IsRequiredRule : RawValueRule
    {
        protected override ValidationResult Validate(string text, TextBox textBox)
        {
            if (textBox.GetIsRequired())
            {
                if (string.IsNullOrEmpty(text))
                {
                    return new ValidationResult(false, new IsRequiredError());
                }
            }
            return ValidationResult.ValidResult;
        }
    }
}
