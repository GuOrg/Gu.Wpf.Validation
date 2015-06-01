namespace Gu.Wpf.Validation.StringConverters
{
    using System.Windows.Controls;

    public class UShortConverter : StringConverter<ushort>
    {
        public override string ToString(ushort value, TextBox textBox)
        {
            return value.ToString();
        }

        public override bool TryParse(string s, TextBox textBox, out ushort result)
        {
            return ushort.TryParse(s, out result);
        }
    }
}