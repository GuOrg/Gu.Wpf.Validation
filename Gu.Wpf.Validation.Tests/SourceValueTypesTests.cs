namespace Gu.Wpf.Validation.Tests
{
    using System;

    using Gu.Wpf.Validation.Internals;
    using Gu.Wpf.Validation.Tests.Helpers;

    using NUnit.Framework;

    public class SourceValueTypesTests
    {
        [TestCaseSource(typeof(NumericTypesSource)), Explicit("Code gen")]
        public void NullableHasProperty(Type type)
        {
            foreach (var source in new NumericTypesSource())
            {
                if (source.IsNullable())
                {
                    var underlyingType = Nullable.GetUnderlyingType(source);
                    Console.WriteLine("public static readonly Type Nullable{0} = typeof({1}?);", underlyingType.Name, underlyingType.PrettyName());
                }
                else
                {
                    Console.WriteLine("public static readonly Type {0} = typeof({1});", source.Name, source.PrettyName());
                }
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
