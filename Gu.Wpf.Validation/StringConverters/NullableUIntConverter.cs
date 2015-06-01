namespace Gu.Wpf.Validation.StringConverters
{
    public class NullableUIntConverter : NullableConverter<uint>
    {
        private static readonly UIntConverter _converter = new UIntConverter();

        public override StringConverter<uint> Converter
        {
            get { return _converter; }
        }
    }
}