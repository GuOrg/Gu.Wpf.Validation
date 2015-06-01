namespace Gu.Wpf.Validation.StringConverters
{
    public class NullableLongConverter : NullableConverter<long>
    {
        private static readonly LongConverter _converter = new LongConverter();

        public override StringConverter<long> Converter
        {
            get { return _converter; }
        }
    }
}