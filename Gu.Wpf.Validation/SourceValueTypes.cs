namespace Gu.Wpf.Validation
{
    using System;

    public static class SourceValueTypes
    {
        public static readonly Type NullableDouble = typeof(double?);
        public static readonly Type NullableSingle = typeof(float?);
        public static readonly Type NullableDecimal = typeof(decimal?);
        public static readonly Type NullableInt32 = typeof(int?);
        public static readonly Type NullableUInt32 = typeof(uint?);
        public static readonly Type NullableInt64 = typeof(long?);
        public static readonly Type NullableUInt64 = typeof(ulong?);
        public static readonly Type NullableInt16 = typeof(short?);
        public static readonly Type NullableUInt16 = typeof(ushort?);
        public static readonly Type NullableSByte = typeof(sbyte?);
        public static readonly Type NullableByte = typeof(byte?);
    }
}
