namespace Gu.Wpf.Validation.Internals
{
    using System;
    using System.Reflection;
    using System.Windows.Data;

    public static class BindingExpressionExt
    {
        private static readonly PropertyInfo NeedsValidationProperty = typeof(BindingExpression).GetProperty("NeedsValidation", BindingFlags.Instance | BindingFlags.NonPublic);
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

        internal static void SetNeedsValidation(this BindingExpression expression, bool value)
        {
            // Hacking with reflection is better than writing all the code that is needed.
            // Don't want to use UpdateSource()
            NeedsValidationProperty.SetValue(expression, value);
        }
    }
}
