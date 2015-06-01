namespace Gu.Wpf.Validation.StringConverters
{
    public class NullableULongConverter : NullableConverter<ulong>
    {
        private static readonly ULongConverter _converter = new ULongConverter();

        public override StringConverter<ulong> Converter
        {
            get { return _converter; }
        }
    }
}