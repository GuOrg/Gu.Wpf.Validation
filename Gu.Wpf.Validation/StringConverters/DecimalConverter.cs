namespace Gu.Wpf.Validation.StringConverters
{
    using System.Windows.Controls;

    public class DecimalConverter : StringConverter<decimal>
    {
        public override string ToFormattedString(decimal value, TextBox textBox)
        {
            var formatString =textBox.GetStringFormat();
            return value.ToString(formatString, textBox.GetCulture());
        }

        public override string ToRawString(decimal value, TextBox textBox)
        {
            return value.ToString(textBox.GetCulture());
        }

        public override bool TryParse(string s, TextBox textBox, out decimal result)
        {
            return decimal.TryParse(s, textBox.GetNumberStyles(), textBox.GetCulture(), out result);
        }
    }
}