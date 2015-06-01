namespace Gu.Wpf.Validation.Rules
{
    using System;

    public class LessThanMinResult
    {
        public LessThanMinResult(object min, object value)
        {
            Type = min.GetType();
            Min = min;
            Value = value;
        }
        public Type Type { get; private set; }

        public object Min { get; private set; }

        public object Value { get; private set; }

        public override string ToString()
        {
            return string.Format("Type: {0} Value: {1} < Min: {2}", Type, Value, Min);
        }
    }
}