namespace Gu.Wpf.Validation
{
    using System;

    [Flags]
    public enum OnValidationErrorStrategy
    {
        None = 0,
        ResetValueOnError = 1 << 0,
        UpdateSourceOnError = 1 << 1,
        UpdateSourceOnSuccess = 1 << 2,
        Default = ResetValueOnError | UpdateSourceOnSuccess
    }
}