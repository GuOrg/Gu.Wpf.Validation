namespace Gu.Wpf.Validation.Rules
{
    public class RegexResult
    {
        public RegexResult(string text, string pattern)
        {
            Text = text;
            Pattern = pattern;
        }
        public string Text { get; private set; }

        public string Pattern { get; private set; }

        public override string ToString()
        {
            return string.Format("Regex failed for Text: {0}, Pattern: {1}", Text, Pattern);
        }
    }
}