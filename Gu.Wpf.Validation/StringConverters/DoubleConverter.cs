namespace Gu.Wpf.Validation.StringConverters
{
    using System.Collections.Concurrent;
    using System.Windows.Controls;

    public class DoubleConverter : StringConverter<double>
    {
        protected static readonly ConcurrentDictionary<int, string> Formats = new ConcurrentDictionary<int, string>();

        public override string ToString(double value, TextBox textBox)
        {
            var formatString = GetFormatString(textBox);
            return value.ToString(formatString, textBox.GetCulture());
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
