namespace Gu.Wpf.Validation.StringConverters
{
    using System.Windows.Controls;

    public class UShortConverter : StringConverter<ushort>
    {
        public override string ToFormattedString(ushort value, TextBox textBox)
        {
            return value.ToString(textBox.GetCulture());
        }

        public override bool TryParse(string s, TextBox textBox, out ushort result)
        {
            return ushort.TryParse(s, textBox.GetNumberStyles(), textBox.GetCulture(), out result);
        }
    }
}