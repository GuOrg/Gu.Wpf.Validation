namespace Gu.Wpf.Validation.StringConverters
{
    public class NullableShortConverter : NullableConverter<short>
    {
        private static readonly ShortConverter _converter = new ShortConverter();

        public override StringConverter<short> Converter
        {
            get { return _converter; }
        }
    }
}