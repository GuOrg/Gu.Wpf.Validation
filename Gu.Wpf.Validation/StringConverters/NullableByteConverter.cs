namespace Gu.Wpf.Validation.StringConverters
{
    public class NullableByteConverter : NullableConverter<byte>
    {
        private static readonly ByteConverter _converter = new ByteConverter();

        public override StringConverter<byte> Converter
        {
            get { return _converter; }
        }
    }
}