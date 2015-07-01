namespace Gu.Wpf.Validation.Demo
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Gu.Wpf.Validation.Demo.Annotations;

    public class SampleVm : INotifyPropertyChanged
    {
        private string _name;
        private int? _age;

        private string _email;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        public int? Age
        {
            get
            {
                return _age;
            }
            set
            {
                if (value == _age)
                {
                    return;
                }
                _age = value;
                OnPropertyChanged();
            }
        }
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                if (value == _email)
                {
                    return;
                }
                _email = value;
                OnPropertyChanged();
            }
        }
        public string Phone { get; set; }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
