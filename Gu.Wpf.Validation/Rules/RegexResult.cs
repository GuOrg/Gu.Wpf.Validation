namespace Gu.Wpf.Validation.Rules
{
    using System;

    public class RegexResult
    {
        public RegexResult(string text, string pattern)
        {
            Text = text;
            Pattern = pattern;
        }

        public RegexResult(string text, string pattern, Exception exception)
            : this(text, pattern)
        {
            Exception = exception;
        }

        public string Text { get; private set; }

        public string Pattern { get; private set; }

        public Exception Exception { get; private set; }

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
            if (Exception == null)
            {
                return string.Format("Text: {0} was not matched by, Pattern: {1}", text, Pattern);
            }
            return
                string.Format(
                    "An exception was thrown when matching text: {0} with pattern: {1}.{2}The exception message is:{2}{3}",
                    text,
                    Pattern,
                    Environment.NewLine,
                    Exception.Message);
        }
    }
}