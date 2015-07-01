namespace Gu.Wpf.Validation.StringConverters
{
    using System;
    using System.ComponentModel;
    using System.Windows.Controls;

    using Gu.Wpf.Validation.Internals;

    public abstract class StringConverter<T> : IStringConverter
    {
        public Type Type { get { return typeof(T); } }

        string IStringConverter.ToFormattedString(object o, TextBox textBox)
        {
            T value;
            try
            {
                value = (T)o;
            }
            catch (Exception e)
            {
                if (DesignerProperties.GetIsInDesignMode(textBox))
                {
                    var message = string.Format(@"StringConverter<{0}> could not convert {1} to type {0}", typeof(T).PrettyName(), o);
                    throw new ArgumentException(message, "o", e);
                }
                return o != null ? o.ToString() : "";
            }
            return ToFormattedString(value, textBox);
        }

        string IStringConverter.ToRawString(object o, TextBox textBox)
        {
            T value;
            try
            {
                value = (T)o;
            }
            catch (Exception e)
            {
                if (DesignerProperties.GetIsInDesignMode(textBox))
                {
                    var message = string.Format(@"StringConverter<{0}> could not convert {1} to type {0}", typeof(T).PrettyName(), o);
                    throw new ArgumentException(message, "o", e);
                }
                return o != null ? o.ToString() : "";
            }
            return ToRawString(value, textBox);
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

        public abstract string ToFormattedString(T value, TextBox textBox);

        public virtual string ToRawString(T value, TextBox textBox)
        {
            return ToFormattedString(value, textBox);
        }

        public abstract bool TryParse(string s, TextBox textBox, out T result);
    }
}