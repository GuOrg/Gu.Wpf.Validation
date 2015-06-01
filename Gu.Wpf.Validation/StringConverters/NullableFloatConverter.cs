namespace Gu.Wpf.Validation.StringConverters
{
    public class NullableFloatConverter : NullableConverter<float>
    {
        private static readonly FloatConverter _converter = new FloatConverter();

        public override StringConverter<float> Converter
        {
            get { return _converter; }
        }
    }
}