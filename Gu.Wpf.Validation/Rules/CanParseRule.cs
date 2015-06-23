namespace Gu.Wpf.Validation.Rules
{
    using System.Windows.Controls;

    public class CanParseRule : RawValueRule
    {
        public override ValidationResult Validate(string text, TextBox textBox)
        {
            var converter = textBox.GetStringConverter();
            object temp;
            if (converter.TryParse(text, textBox, out temp))
            {
                return ValidationResult.ValidResult;
            }
            return new ValidationResult(false, new CanParseError(text, converter.Type));
        }
    }
}
