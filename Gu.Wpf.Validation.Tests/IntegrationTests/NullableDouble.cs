namespace Gu.Wpf.Validation.Tests.IntegrationTests
{
    using System.Reflection;

    using Gu.Wpf.Validation.Tests.Helpers;

    using NUnit.Framework;

    [RequiresSTA]
    public class NullableDouble : TextBoxTests
    {
        protected override PropertyInfo Property
        {
            get { return typeof(DummyViewModel).GetProperty("NullableDoubleValue"); }
        }
    }
}