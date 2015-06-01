namespace Gu.Wpf.Validation.StringConverters
{
    public class NullableSByteConverter : NullableConverter<sbyte>
    {
        private static readonly SByteConverter _converter = new SByteConverter();

        public override StringConverter<sbyte> Converter
        {
            get { return _converter; }
        }
    }
}