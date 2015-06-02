namespace Gu.Wpf.Validation.StringConverters
{
    using System.Collections.Concurrent;
    using System.Windows.Controls;

    public class DoubleConverter : StringConverter<double>
    {
        protected static readonly ConcurrentDictionary<int, string> Formats = new ConcurrentDictionary<int, string>();

        public override string ToFormattedString(double value, TextBox textBox)
        {
            var formatString = GetFormatString(textBox);
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

        protected virtual string GetFormatString(TextBox textBox)
        {
            var digits = textBox.GetDecimalDigits() ?? -1;
            var format = Formats.GetOrAdd(digits, d => d == -1 ? "" :string.Format( "F{0}",d));
            return format;
        }
    }
}
