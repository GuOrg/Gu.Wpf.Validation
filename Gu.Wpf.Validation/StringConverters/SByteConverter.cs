namespace Gu.Wpf.Validation.StringConverters
{
    using System.Windows.Controls;

    public class SByteConverter : StringConverter<sbyte>
    {
        public override string ToFormattedString(sbyte value, TextBox textBox)
        {
            return value.ToString();
        }

        public override bool TryParse(string s, TextBox textBox, out sbyte result)
        {
            return sbyte.TryParse(s, out result);
        }
    }
}