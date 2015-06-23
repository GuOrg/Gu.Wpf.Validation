namespace Gu.Wpf.Validation.Rules
{
    using System;

    using Gu.Wpf.Validation.Internals;

    public class CanParseError
    {
        public CanParseError(string text, Type type)
        {
            Text = text;
            Type = type;
        }
        public string Text { get; private set; }

        public Type Type { get; private set; }

        public override string ToString()
        {
            string text = Text;
            if (text == null)
            {
                text = "null";
            }
            if (text == string.Empty)
            {
                text = "string.Empty";
            }
            return string.Format("Parsing: {0} to Type: {1} failed", text, Type.PrettyName());
        }
    }
}