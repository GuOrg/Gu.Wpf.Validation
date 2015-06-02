namespace Gu.Wpf.Validation.StringConverters
{
    using System.Windows.Controls;

    public class LongConverter : StringConverter<long>
    {
        public override string ToFormattedString(long value, TextBox textBox)
        {
            return value.ToString();
        }

        public override bool TryParse(string s, TextBox textBox, out long result)
        {
            return long.TryParse(s, out result);
        }
    }
}