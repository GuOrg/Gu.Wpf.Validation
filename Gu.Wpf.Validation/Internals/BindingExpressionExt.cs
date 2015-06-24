namespace Gu.Wpf.Validation.Internals
{
    using System;
    using System.Windows.Data;

    public static class BindingExpressionExt
    {
        internal static Type GetSourceValueType(this BindingExpression expression)
        {
            if (expression == null)
            {
                return null;
            }
            var source = expression.ResolvedSource;
            if (source == null)
            {
                return null;
            }
            var propertyInfo = source.GetType().GetProperty(expression.ResolvedSourcePropertyName);
            if (propertyInfo == null)
            {
                return null;
            }
            return propertyInfo.PropertyType;
        }

        internal static object GetSourceValue(this BindingExpression expression)
        {
            if (expression == null)
            {
                return null;
            }
            var source = expression.ResolvedSource;
            if (source == null)
            {
                return null;
            }
            var propertyInfo = source.GetType().GetProperty(expression.ResolvedSourcePropertyName);
            if (propertyInfo == null)
            {
                return null;
            }
            return propertyInfo.GetValue(source);
        }
    }
}
