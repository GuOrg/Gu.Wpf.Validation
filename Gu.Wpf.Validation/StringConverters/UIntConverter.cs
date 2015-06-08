namespace Gu.Wpf.Validation.StringConverters
{
    using System.Windows.Controls;

    public class UIntConverter : StringConverter<uint>
    {
        public override string ToFormattedString(uint value, TextBox textBox)
        {
            return value.ToString(textBox.GetCulture());
        }

        public override bool TryParse(string s, TextBox textBox, out uint result)
        {
            return uint.TryParse(s, textBox.GetNumberStyles(), textBox.GetCulture(), out result);
        }
    }
}