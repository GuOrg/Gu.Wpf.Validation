namespace Gu.Wpf.Validation.Tests.Helpers
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using Gu.Wpf.Validation.Demo.Annotations;

    public class DummyViewModel : INotifyPropertyChanged
    {
        private double _doubleValue;
        private double? _nullableDoubleValue = 0;

        private int _intValue;

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

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}