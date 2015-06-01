namespace Gu.Wpf.Validation.Tests
{
    using System.Globalization;
    using System.Windows.Controls;
    using NUnit.Framework;

    [RequiresSTA]
    public class SetupTests
    {
        private TextBox _textBox;

        [SetUp]
        public void SetUp()
        {
            _textBox = new TextBox();
        }

        [Test]
        public void SettingSourceValueTypeUpdatesNumberStyles()
        {
            Assert.AreEqual(NumberStyles.None, _textBox.GetNumberStyles());
            _textBox.SetSourceValueType(typeof(double));
            Assert.AreEqual(DefaultNumberStyles.DefaultFloat, _textBox.GetNumberStyles());
        }

        [Test]
        public void SettingSourceValueTypeUpdatesStringConverter()
        {
            Assert.IsNull(_textBox.GetStringConverter());
            _textBox.SetSourceValueType(typeof(double));
            Assert.AreEqual(new DefaultStringConverters().Get(typeof(double)), _textBox.GetStringConverter());
        }

        [Test]
        public void SettingSourceValueTypeUpdatesMin()
        {
            Assert.IsNull(_textBox.GetStringConverter());
            _textBox.SetSourceValueType(typeof(double));
            Assert.AreEqual(new DefaultStringConverters().Get(typeof(double)), _textBox.GetStringConverter());
        }
    }
}
