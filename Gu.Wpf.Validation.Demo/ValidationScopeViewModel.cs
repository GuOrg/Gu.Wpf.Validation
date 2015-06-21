namespace Gu.Wpf.Validation.Demo
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Annotations;

    public class ValidationScopeViewModel :  INotifyPropertyChanged
    {
        private string _value1;
        private string _vmValidatedValue;

        private string _value2;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Value1
        {
            get { return _value1; }
            set
            {
                if (value == _value1) return;
                _value1 = value;
                OnPropertyChanged();
            }
        }

        public string Value2
        {
            get
            {
                return _value2;
            }
            set
            {
                if (value == _value2)
                {
                    return;
                }
                _value2 = value;
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
