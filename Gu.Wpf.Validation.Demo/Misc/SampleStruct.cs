namespace Gu.Wpf.Validation.Demo.Misc
{
    using System;

    public struct SampleStruct : IEquatable<SampleStruct>
    {
        public readonly double Value;

        public SampleStruct(double value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return string.Format("Value: {0}", Value);
        }

        public bool Equals(SampleStruct other)
        {
            return Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            return obj is SampleStruct && Equals((SampleStruct)obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(SampleStruct left, SampleStruct right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SampleStruct left, SampleStruct right)
        {
            return !left.Equals(right);
        }
    }
}
