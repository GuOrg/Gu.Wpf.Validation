namespace Gu.Wpf.Validation.Tests
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Demo.Annotations;

    public class DummyViewModel<T> : INotifyPropertyChanged
    {
        private T _value;
        public event PropertyChangedEventHandler PropertyChanged;

        public T Value
        {
            get { return _value; }
            set
            {
                if (Equals(value, _value)) return;
                _value = value;
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