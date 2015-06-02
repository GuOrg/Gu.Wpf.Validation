namespace Gu.Wpf.Validation.StringConverters
{
    using System;
    using System.Windows.Controls;

    public interface IStringConverter
    {
        Type Type { get; }

        string ToFormattedString(object o, TextBox textBox);

        string ToRawString(object value, TextBox textBox);

        bool TryParse(object o, TextBox textBox, out object result);
    }
}