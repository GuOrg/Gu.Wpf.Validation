namespace Gu.Wpf.Validation.Demo
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Annotations;

    public class StrategyVm : INotifyPropertyChanged
    {
        private OnValidationErrorStrategy _strategy = OnValidationErrorStrategy.Default;
        public event PropertyChangedEventHandler PropertyChanged;

        public OnValidationErrorStrategy Strategy
        {
            get { return _strategy; }
            set
            {
                if (value == _strategy)
                {
                    return;
                }
                _strategy = value;
                OnPropertyChanged();
                OnPropertyChanged("ResetValueOnError");
                OnPropertyChanged("UpdateSourceOnSuccess");
                OnPropertyChanged("UpdateSourceOnError");
            }
        }

        public bool ResetValueOnError
        {
            get { return _strategy.HasFlag(OnValidationErrorStrategy.ResetValueOnError); }
            set
            {
                if (value == ResetValueOnError)
                {
                    return;
                }
                if (value)
                {
                    Strategy = _strategy | OnValidationErrorStrategy.ResetValueOnError;
                }
                else
                {
                    Strategy = _strategy & ~OnValidationErrorStrategy.ResetValueOnError;
                }
                OnPropertyChanged();
            }
        }

        public bool UpdateSourceOnSuccess
        {
            get { return _strategy.HasFlag(OnValidationErrorStrategy.UpdateSourceOnSuccess); }
            set
            {
                if (value == UpdateSourceOnSuccess)
                {
                    return;
                }
                if (value)
                {
                    Strategy = _strategy | OnValidationErrorStrategy.UpdateSourceOnSuccess;
                }
                else
                {
                    Strategy = _strategy & ~OnValidationErrorStrategy.UpdateSourceOnSuccess;
                }
                OnPropertyChanged();
            }
        }

        public bool UpdateSourceOnError
        {
            get { return _strategy.HasFlag(OnValidationErrorStrategy.UpdateSourceOnError); }
            set
            {
                if (value == UpdateSourceOnError)
                {
                    return;
                }
                if (value)
                {
                    Strategy = _strategy | OnValidationErrorStrategy.UpdateSourceOnError;
                }
                else
                {
                    Strategy = _strategy & ~OnValidationErrorStrategy.UpdateSourceOnError;
                }
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
