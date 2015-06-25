namespace Gu.Wpf.Validation.StringConverters
{
    using System.Windows.Controls;

    public class IntConverter : StringConverter<int>
    {
        public override string ToFormattedString(int value, TextBox textBox)
        {
            var format = textBox.GetStringFormat();
            return value.ToString(format, textBox.GetCulture());
        }

        public override bool TryParse(string s, TextBox textBox, out int result)
        {
            return int.TryParse(s, textBox.GetNumberStyles(), textBox.GetCulture(), out result);
        }
    }
}