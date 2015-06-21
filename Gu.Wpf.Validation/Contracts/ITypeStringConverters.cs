namespace Gu.Wpf.Validation
{
    using System;
    using StringConverters;

    public interface ITypeStringConverters
    {
        IStringConverter Get(Type type);
    }
}