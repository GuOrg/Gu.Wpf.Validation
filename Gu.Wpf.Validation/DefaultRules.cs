namespace Gu.Wpf.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Controls;
    using Rules;

    public class DefaultRules : ITypeRules
    {
        internal static readonly IReadOnlyCollection<ValidationRule> NumberRules = new ValidationRule[]
        {
            new CanParseRule(),
            new IsRequiredRule(), 
            new RegexRule(), 
            new MustBeGreaterThanMinRule(),
            new MustBeLessThanMaxRule(), 
        };

        internal static readonly IReadOnlyCollection<ValidationRule> StringRules = new ValidationRule[]
        {
            new CanParseRule(),
            new IsRequiredRule(), 
            new RegexRule(), 
        };

        internal static Dictionary<Type, IReadOnlyCollection<ValidationRule>> Rules = new Dictionary<Type, IReadOnlyCollection<ValidationRule>>
                {
                    { typeof(double), NumberRules },
                    { typeof(double?), NumberRules },
                    { typeof(float), NumberRules },
                    { typeof(float?), NumberRules },
                    { typeof(decimal), NumberRules },
                    { typeof(decimal?), NumberRules },
                    { typeof(int), NumberRules },
                    { typeof(int?), NumberRules },
                    { typeof(uint), NumberRules },
                    { typeof(uint?), NumberRules },
                    { typeof(long), NumberRules },
                    { typeof(long?), NumberRules },
                    { typeof(ulong), NumberRules },
                    { typeof(ulong?), NumberRules },
                    { typeof(short), NumberRules },
                    { typeof(short?), NumberRules },
                    { typeof(ushort), NumberRules },
                    { typeof(ushort?), NumberRules },
                    { typeof(byte), NumberRules },
                    { typeof(byte?), NumberRules },
                    { typeof(sbyte), NumberRules },
                    { typeof(sbyte?), NumberRules },
                    { typeof(string), StringRules },
                };

        public IReadOnlyCollection<ValidationRule> Get(Type type)
        {
            IReadOnlyCollection<ValidationRule> rules;
            if (Rules.TryGetValue(type, out rules))
            {
                return rules;
            }
            return new ValidationRule[0];
        }
    }
}
