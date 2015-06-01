namespace Gu.Wpf.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public class DefaultNumberStyles : ITypeNumberStyles
    {
        internal static readonly NumberStyles DefaultFloat = NumberStyles.Float;

        internal static readonly NumberStyles DefaultInteger = NumberStyles.Integer;

        internal static readonly NumberStyles DefaultUnsignedInteger = NumberStyles.AllowLeadingWhite |
                                                                       NumberStyles.AllowTrailingWhite;

        private static readonly Dictionary<Type, NumberStyles> Styles = new Dictionary<Type, NumberStyles>
                                                                          {
                                                                              { typeof(double), DefaultFloat },
                                                                              { typeof(double?), DefaultFloat },
                                                                              { typeof(float), DefaultFloat },
                                                                              { typeof(float?), DefaultFloat },
                                                                              { typeof(decimal), DefaultFloat },
                                                                              { typeof(decimal?), DefaultFloat },
                                                                              { typeof(int), DefaultInteger },
                                                                              { typeof(int?), DefaultInteger },
                                                                              { typeof(uint), DefaultUnsignedInteger },
                                                                              { typeof(uint?), DefaultUnsignedInteger },
                                                                              { typeof(long), DefaultInteger },
                                                                              { typeof(long?), DefaultInteger },
                                                                              { typeof(ulong), DefaultUnsignedInteger },
                                                                              { typeof(ulong?), DefaultUnsignedInteger },
                                                                              { typeof(short), DefaultInteger },
                                                                              { typeof(short?), DefaultInteger },
                                                                              { typeof(ushort), DefaultUnsignedInteger },
                                                                              { typeof(ushort?), DefaultUnsignedInteger },
                                                                              { typeof(sbyte), DefaultInteger },
                                                                              { typeof(sbyte?), DefaultInteger },
                                                                              { typeof(byte), DefaultUnsignedInteger },
                                                                              { typeof(byte?), DefaultUnsignedInteger },
                                                                          };
        public virtual NumberStyles Get(Type type)
        {
            NumberStyles styles;
            if (Styles.TryGetValue(type, out styles))
            {
                return styles;
            }
            return NumberStyles.None;
        }
    }
}
