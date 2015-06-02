namespace Gu.Wpf.Validation.StringConverters
{
    using System.Windows.Controls;

    public class ShortConverter : StringConverter<short>
    {
        public override string ToFormattedString(short value, TextBox textBox)
        {
            return value.ToString();
        }

        public override bool TryParse(string s, TextBox textBox, out short result)
        {
            return short.TryParse(s, out result);
        }
    }
}