namespace Gu.Wpf.Validation.StringConverters
{
    public class NullableIntConverter : NullableConverter<int>
    {
        private static readonly IntConverter _converter = new IntConverter();

        public override StringConverter<int> Converter
        {
            get { return _converter; }
        }
    }
}