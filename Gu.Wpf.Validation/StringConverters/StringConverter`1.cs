namespace Gu.Wpf.Validation.StringConverters
{
    using System;
    using System.Windows.Controls;

    public abstract class StringConverter<T> : IStringConverter
    {
        public Type Type { get { return typeof(T); } }

        string IStringConverter.ToString(object o, TextBox textBox)
        {
            return ToString((T)o, textBox);
        }

        bool IStringConverter.TryParse(object o, TextBox textBox, out object result)
        {
            T typedResult;
            if (TryParse((string)o, textBox, out typedResult))
            {
                result = typedResult;
                return true;
            }
            result = null;
            return false;
        }

        public override string ToString()
        {
            return string.Format("StringConverter<{0}>", Type.Name);
        }

        public abstract string ToString(T value, TextBox textBox);

        public abstract bool TryParse(string s, TextBox textBox, out T result);
    }
}