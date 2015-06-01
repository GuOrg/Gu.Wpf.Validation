namespace Gu.Wpf.Validation.StringConverters
{
    public class NullableUShortConverter : NullableConverter<ushort>
    {
        private static readonly UShortConverter _converter = new UShortConverter();

        public override StringConverter<ushort> Converter
        {
            get { return _converter; }
        }
    }
}