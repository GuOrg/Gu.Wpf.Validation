namespace Gu.Wpf.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Controls;

    public interface ITypeRules
    {
        IReadOnlyCollection<ValidationRule> Get(Type type);
    }
}