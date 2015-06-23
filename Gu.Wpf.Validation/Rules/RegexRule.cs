namespace Gu.Wpf.Validation.Rules
{
    using System;
    using System.Text.RegularExpressions;
    using System.Windows.Controls;

    public class RegexRule : RawValueRule
    {
        public override ValidationResult Validate(string text, TextBox textBox)
        {
            var pattern = textBox.GetPattern();
            if (pattern == null)
            {
                return ValidationResult.ValidResult;
            }
            if (text == null)
            {
                text = String.Empty;
            }
            try
            {
                if (Regex.IsMatch(text, pattern))
                {
                    return ValidationResult.ValidResult;
                }
            }
            catch (Exception e)
            {
                return new ValidationResult(false, new RegexResult(text, pattern, e));
            }
            return new ValidationResult(false, new RegexResult(text, pattern));
        }
    }
}