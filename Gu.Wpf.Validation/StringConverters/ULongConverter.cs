namespace Gu.Wpf.Validation.StringConverters
{
    using System.Windows.Controls;

    public class ULongConverter : StringConverter<ulong>
    {
        public override string ToString(ulong value, TextBox textBox)
        {
            return value.ToString();
        }

        public override bool TryParse(string s, TextBox textBox, out ulong result)
        {
            return ulong.TryParse(s, out result);
        }
    }
}