namespace Gu.Wpf.Validation.Internals
{
    using System;

    internal static class ToStringExt
    {
        internal static string ToDebugString(this object o)
        {
            if (o == null)
            {
                return "null";
            }
            var s = o as string;
            if (s != null)
            {
                if (s == string.Empty)
                {
                    return "string.Empty";
                }
                return string.Format(@"""{0}""", s);
            }

            var type = o as Type;
            if (type != null)
            {
                return type.PrettyName();
            }
            return o.ToString();
        }
    }
}
