namespace Gu.Wpf.Validation.StringConverters
{
    using System.Windows.Controls;

    public abstract class NullableConverter<T> : StringConverter<T?>
        where T : struct 
    {
        public abstract StringConverter<T> Converter { get; }

        public override string ToString(T? value, TextBox textBox)
        {
            if (value == null)
            {
                return "";
            }
            return Converter.ToString(value.Value, textBox);
        }

        public override bool TryParse(string s, TextBox textBox, out T? result)
        {
            if (string.IsNullOrEmpty(s))
            {
                result = null;
                return true;
            }
            T value;
            if (Converter.TryParse(s, textBox, out value))
            {
                result = value;
                return true;
            }
            result = null;
            return false;
        }
    }
}