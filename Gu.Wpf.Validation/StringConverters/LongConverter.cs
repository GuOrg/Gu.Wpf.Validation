namespace Gu.Wpf.Validation.StringConverters
{
    using System.Windows.Controls;

    public class LongConverter : StringConverter<long>
    {
        public override string ToFormattedString(long value, TextBox textBox)
        {
            var format = textBox.GetStringFormat();
            return value.ToString(format, textBox.GetCulture());
        }

        public override bool TryParse(string s, TextBox textBox, out long result)
        {
            return long.TryParse(s, textBox.GetNumberStyles(), textBox.GetCulture(), out result);
        }
    }
}