namespace Gu.Wpf.Validation.Tests
{
    using System;

    using Gu.Wpf.Validation.Tests.Helpers;

    using NUnit.Framework;

    public class DefaultStringConvertersTests
    {
        [TestCaseSource(typeof(NumericTypesSource))]
        public void ForNumericType(Type type)
        {
            var converters = new DefaultStringConverters();
            var converter = converters.Get(type);
            Assert.IsNotNull(converter);
        }
    }
}