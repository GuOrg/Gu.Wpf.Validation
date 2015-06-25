namespace Gu.Wpf.Validation.StringConverters
{
    using System.Windows.Controls;

    public class FloatConverter : StringConverter<float>
    {
        public override string ToFormattedString(float value, TextBox textBox)
        {
            var formatString = textBox.GetStringFormat();
            return value.ToString(formatString, textBox.GetCulture());
        }

        public override string ToRawString(float value, TextBox textBox)
        {
            return value.ToString(textBox.GetCulture());
        }

        public override bool TryParse(string s, TextBox textBox, out float result)
        {
            return float.TryParse(s, textBox.GetNumberStyles(), textBox.GetCulture(), out result);
        }
    }
}