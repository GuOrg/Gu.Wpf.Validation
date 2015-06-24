namespace Gu.Wpf.Validation.Tests
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Gu.Wpf.Validation.Internals;
    using Gu.Wpf.Validation.Tests.Helpers;

    using NUnit.Framework;

    public class SourceValueTypesTests
    {
        [TestCaseSource(typeof(NumericTypesSource)), Explicit("Code gen")]
        public void NullableHasProperty(Type type)
        {
            foreach (var source in new NumericTypesSource().Where(f=>f.IsNullable()))
            {
                var underlyingType = Nullable.GetUnderlyingType(source);
                Console.WriteLine("public static readonly Type Nullable{0} = typeof({1}?);", underlyingType.Name, underlyingType.PrettyName());
            }
            //if (type.IsNullable())
            //{
            //    var field = typeof(SourceValueTypes).GetFields(BindingFlags.Static | BindingFlags.Public).SingleOrDefault(f => f.FieldType == type);
            //    if (field == null)
            //    {
            //        var underlyingType = Nullable.GetUnderlyingType(type);
            //        Console.WriteLine("public static readonly Type Nullable{0} = typeof({1}?);", underlyingType.Name, underlyingType.PrettyName());
            //    }
            //    Assert.IsTrue(field != null);
            //}
        }
    }
}
