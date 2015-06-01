namespace Gu.Wpf.Validation.Tests
{
    using System;

    using Gu.Wpf.Validation.Tests.Helpers;

    using NUnit.Framework;

    public class DefaultRulesTests
    {
        [TestCaseSource(typeof(NumericTypesSource))]
        public void ForNumericType(Type type)
        {
            var defaultRules = new DefaultRules();
            var rules = defaultRules.Get(type);
            CollectionAssert.IsNotEmpty(rules);
        }
    }
}
