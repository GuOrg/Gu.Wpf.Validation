namespace Gu.Wpf.Validation.Demo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using Annotations;

    public class InputValidationViewModel : INotifyPropertyChanged
    {
        private double _doubleValue;
        private double? _nullableDoubleValue;

        private int _intValue;

        private int? _nullableIntValue;

        private string _stringValue;

        public event PropertyChangedEventHandler PropertyChanged;

        public IEnumerable<IFormatProvider> Cultures
        {
            get
            {
                return new[] { CultureInfo.GetCultureInfo("sv"), CultureInfo.GetCultureInfo("en"), };
            }
        }

        public double DoubleValue
        {
            get { return _doubleValue; }
            set
            {
                if (value.Equals(_doubleValue)) return;
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

        public int? NullableIntValue
        {
            get
            {
                return _nullableIntValue;
            }
            set
            {
                if (value == _nullableIntValue)
                {
                    return;
                }
                _nullableIntValue = value;
                OnPropertyChanged();
            }
        }

        public string StringValue
        {
            get
            {
                return _stringValue;
            }
            set
            {
                if (value == _stringValue)
                {
                    return;
                }
                _stringValue = value;
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
