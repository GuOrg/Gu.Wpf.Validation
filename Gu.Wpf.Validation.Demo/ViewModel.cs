using System.ComponentModel;
using System.Runtime.CompilerServices;
using Gu.Wpf.Validation.Demo.Annotations;

namespace Gu.Wpf.Validation.Demo
{
    public class ViewModel : INotifyPropertyChanged
    {
        private string _value;
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

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
