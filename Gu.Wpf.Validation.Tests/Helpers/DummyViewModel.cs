namespace Gu.Wpf.Validation.Tests.Helpers
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using Gu.Wpf.Validation.Tests.Annotations;

    public class DummyViewModel : INotifyPropertyChanged
    {
        internal static string NullableDoubleValuePropertyName = "NullableDoubleValue";
        internal static string DoubleValuePropertyName = "DoubleValue";
        internal static string StringIntValuePropertyName = "StringIntValue";
        private double _doubleValue;
        private double? _nullableDoubleValue = 0;

        private int _intValue;

        private string _stringIntValue;

        public event PropertyChangedEventHandler PropertyChanged;

        public double DoubleValue
        {
            get { return _doubleValue; }
            set
            {
                if (Equals(value, _doubleValue)) return;
                _doubleValue = value;
                OnPropertyChanged();
            }
        }

        public int IntValue
        {
            get
            {
                return _intValue;
            }
            set
            {
                if (value == _intValue)
                {
                    return;
                }
                _intValue = value;
                OnPropertyChanged();
            }
        }

        public double? NullableDoubleValue
        {
            get { return _nullableDoubleValue; }
            set
            {
                if (value.Equals(_nullableDoubleValue)) return;
                _nullableDoubleValue = value;
                OnPropertyChanged();
            }
        }

        public string StringIntValue
        {
            get
            {
                return _stringIntValue;
            }
            set
            {
                if (value == _stringIntValue)
                {
                    return;
                }
                _stringIntValue = value;
                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}