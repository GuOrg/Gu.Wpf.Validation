namespace Gu.Wpf.Validation.StringConverters
{
    public class NullableDecimalConverter : NullableConverter<decimal>
    {
        private static readonly DecimalConverter _converter = new DecimalConverter();

        public override StringConverter<decimal> Converter
        {
            get { return _converter; }
        }
    }
}