namespace Gu.Wpf.Validation.Rules
{
    using System;

    public class ParseErrorResult
    {
        public ParseErrorResult(string text, Type type)
        {
            Text = text;
            Type = type;
        }
        public string Text { get; private set; }

        public Type Type { get; private set; }

        public override string ToString()
        {
            return string.Format("Parsing: {0} to Type: {1} failed", Text, Type);
        }
    }
}