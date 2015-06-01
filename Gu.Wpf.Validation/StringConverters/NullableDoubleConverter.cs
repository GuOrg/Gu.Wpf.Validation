namespace Gu.Wpf.Validation.StringConverters
{
    public class NullableDoubleConverter : NullableConverter<double>
    {
        private static readonly DoubleConverter _converter = new DoubleConverter();

        public override StringConverter<double> Converter
        {
            get { return _converter; }
        }
    }
}