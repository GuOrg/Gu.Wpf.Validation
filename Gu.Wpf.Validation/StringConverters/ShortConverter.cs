namespace Gu.Wpf.Validation.StringConverters
{
    using System.Windows.Controls;

    public class ShortConverter : StringConverter<short>
    {
        public override string ToFormattedString(short value, TextBox textBox)
        {
            return value.ToString(textBox.GetCulture());
        }

        public override bool TryParse(string s, TextBox textBox, out short result)
        {
            return short.TryParse(s, textBox.GetNumberStyles(), textBox.GetCulture(), out result);
        }
    }
}