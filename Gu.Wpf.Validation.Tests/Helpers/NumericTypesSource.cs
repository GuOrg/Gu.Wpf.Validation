namespace Gu.Wpf.Validation.Tests.Helpers
{
    using System;
    using System.Collections.Generic;

    public class NumericTypesSource : List<Type>
    {
        public NumericTypesSource()
        {
            Add(typeof(double));
            Add(typeof(double?));
            Add(typeof(float));
            Add(typeof(float?));
            Add(typeof(decimal));
            Add(typeof(decimal?));
            Add(typeof(int));
            Add(typeof(int?));
            Add(typeof(uint));
            Add(typeof(uint?));
            Add(typeof(long));
            Add(typeof(long?));
            Add(typeof(ulong));
            Add(typeof(ulong?));
            Add(typeof(short));
            Add(typeof(short?));
            Add(typeof(ushort));
            Add(typeof(ushort?));
            Add(typeof(sbyte));
            Add(typeof(sbyte?));
            Add(typeof(byte));
            Add(typeof(byte?));
        }
    }
}
