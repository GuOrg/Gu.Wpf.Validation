namespace Gu.Wpf.Validation.StringConverters
{
    using System.Windows.Controls;

    public class IntConverter : StringConverter<int>
    {
        public override string ToString(int value, TextBox textBox)
        {
            return value.ToString();
        }

        public override bool TryParse(string s, TextBox textBox, out int result)
        {
            return int.TryParse(s, out result);
        }
    }
} 