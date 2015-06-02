namespace Gu.Wpf.Validation.Tests.StringConverters
{
    using System.Windows.Controls;
    using NUnit.Framework;
    using Validation.StringConverters;

    [RequiresSTA]
    public class DoubleConverterTests
    {
        private TextBox _textBox;
        private DoubleConverter _converter;

        [SetUp]
        public void SetUp()
        {
            _textBox = new TextBox();
            _converter = new DoubleConverter();
        }

        [TestCase(1, "", "1")]
        public void ToStringTests(double value,string text, string expected)
        {
            _textBox.Text = text;
            var actual = _converter.ToFormattedString(value, _textBox);
            Assert.AreEqual(expected, actual);
        }
    }
}
