namespace Gu.Wpf.Validation.Tests
{
    using System;
    using System.Globalization;

    using Gu.Wpf.Validation.Tests.Helpers;

    using NUnit.Framework;

    public class DefaultNumberStylesTests
    {
        [TestCaseSource(typeof(NumericTypesSource))]
        public void ForNumericType(Type type)
        {
            var defaultRules = new DefaultNumberStyles();
            var styles = defaultRules.Get(type);
            Assert.AreNotEqual(NumberStyles.None, styles);
        }
    }
}