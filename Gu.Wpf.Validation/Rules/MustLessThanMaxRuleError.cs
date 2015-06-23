namespace Gu.Wpf.Validation.Rules
{
    using System;

    public class MustLessThanMaxRuleError
    {
        public MustLessThanMaxRuleError(object max, object value)
        {
            Type = max.GetType();
            Max = max;
            Value = value;
        }
        public Type Type { get; private set; }

        public object Max { get; private set; }

        public object Value { get; private set; }

        public override string ToString()
        {
            return string.Format("Type: {0} Value: {1} > Max: {2}", Type, Value, Max);
        }
    }
}