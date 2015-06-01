namespace Gu.Wpf.Validation.StringConverters
{
    using System;
    using System.Windows.Controls;

    public class StringToStringConverter : IStringConverter
    {
        public Type Type
        {
            get { return typeof(string); }
        }

        public string ToString(object o, TextBox textBox)
        {
            return o as string;
        }

        public bool TryParse(object o, TextBox textBox, out object result)
        {
            result = o;
            return true;
        }
    }
}