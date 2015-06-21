namespace Gu.Wpf.Validation.StringConverters
{
    using System;
    using System.Windows.Controls;

    public class DefaultStringConverter : IStringConverter
    {
        public DefaultStringConverter(Type type)
        {
            Type = type;
        }
        public Type Type { get; private set; }

        public string ToFormattedString(object o, TextBox textBox)
        {
            return ToRawString(o, textBox);
        }

        public string ToRawString(object value, TextBox textBox)
        {
            if (value == null)
            {
                return "";
            }
            return value.ToString();
        }

        public bool TryParse(object o, TextBox textBox, out object result)
        {
            try
            {
                result = Convert.ChangeType(o, Type);
                return true;
            }
            catch (Exception)
            {
                result = null;
                return false;
            }
        }
    }
}
