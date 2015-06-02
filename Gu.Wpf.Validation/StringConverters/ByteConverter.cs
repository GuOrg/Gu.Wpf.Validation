namespace Gu.Wpf.Validation.StringConverters
{
    using System.Windows.Controls;

    public class ByteConverter : StringConverter<byte>
    {
        public override string ToFormattedString(byte value, TextBox textBox)
        {
            return value.ToString();
        }

        public override bool TryParse(string s, TextBox textBox, out byte result)
        {
            return byte.TryParse(s, out result);
        }
    }
}