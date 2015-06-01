namespace Gu.Wpf.Validation
{
    using System;
    using System.Globalization;

    public interface ITypeNumberStyles
    {
        NumberStyles Get(Type type);
    }
}