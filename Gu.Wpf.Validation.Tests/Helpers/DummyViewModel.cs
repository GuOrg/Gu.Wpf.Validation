namespace Gu.Wpf.Validation.Tests.Helpers
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using Gu.Wpf.Validation.Demo.Annotations;

    public class DummyViewModel : INotifyPropertyChanged
    {
        private double _doubleValue;
        private double? _nullableDoubleValue = 0;
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