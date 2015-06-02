namespace Gu.Wpf.Validation.StringConverters
{
    using System.Collections.Concurrent;
    using System.Windows.Controls;

    public class DecimalConverter : StringConverter<decimal>
    {
        protected static readonly ConcurrentDictionary<int, string> Formats = new ConcurrentDictionary<int, string>();

        public override string ToFormattedString(decimal value, TextBox textBox)
        {
            var formatString = GetFormatString(textBox);
            return value.ToString(formatString, textBox.GetCulture());
        }

        public override string ToRawString(decimal value, TextBox textBox)
        {
            return value.ToString(textBox.GetCulture());
        }

        public override bool TryParse(string s, TextBox textBox, out decimal result)
        {
            return decimal.TryParse(s, textBox.GetNumberStyles(), textBox.GetCulture(), out result);
        }

        protected virtual string GetFormatString(TextBox textBox)
        {
            var digits = textBox.GetDecimalDigits() ?? -1;
            var format = Formats.GetOrAdd(digits, d => d == -1 ? "" : string.Format("F{0}", d));
            return format;
        }
    }
}