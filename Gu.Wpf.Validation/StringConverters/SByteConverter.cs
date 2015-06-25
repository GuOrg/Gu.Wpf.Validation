namespace Gu.Wpf.Validation.StringConverters
{
    using System.Windows.Controls;

    public class SByteConverter : StringConverter<sbyte>
    {
        public override string ToFormattedString(sbyte value, TextBox textBox)
        {
            var format = textBox.GetStringFormat();
            return value.ToString(format, textBox.GetCulture());
        }

        public override bool TryParse(string s, TextBox textBox, out sbyte result)
        {
            return sbyte.TryParse(s, textBox.GetNumberStyles(), textBox.GetCulture(), out result);
        }
    }
}