namespace Gu.Wpf.Validation.Demo
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Annotations;

    public class ValidationScopeViewModel :  INotifyPropertyChanged
    {
        private string _value;
        private string _vmValidatedValue;
        public event PropertyChangedEventHandler PropertyChanged;

        public string Value
        {
            get { return _value; }
            set
            {
                if (value == _value) return;
                _value = value;
                OnPropertyChanged();
            }
        }

        public string VmValidatedValue
        {
            get { return _vmValidatedValue; }
            set
            {
                if (_vmValidatedValue == value)
                {
                    return;
                }
                _vmValidatedValue = value;
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
