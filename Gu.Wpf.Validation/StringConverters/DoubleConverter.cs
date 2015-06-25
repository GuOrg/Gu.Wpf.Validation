namespace Gu.Wpf.Validation.StringConverters
{
    using System.Windows.Controls;

    public class DoubleConverter : StringConverter<double>
    {
        public override string ToFormattedString(double value, TextBox textBox)
        {
            var formatString = textBox.GetStringFormat();
            var culture = textBox.GetCulture();
            return value.ToString(formatString, culture);
        }

        public override string ToRawString(double value, TextBox textBox)
        {
            var culture = textBox.GetCulture();
            return value.ToString(culture);
        }

        public override bool TryParse(string s, TextBox textBox, out double result)
        {
            return double.TryParse(s, textBox.GetNumberStyles(), textBox.GetCulture(), out result);
        }
    }
}
