using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_1_Color.ViewModels
{
    public class Color : ViewModelBase
    {
        public Color()
        {
            Min = new Tuple<double, double, double>(0,0,0);
            IsInt = false;
        }
        private double _first;
        private double _second;
        private double _third;
        private string _firstName;
        private string _secondName;
        private string _thirdName;

        #region Public properties
        public bool IsInt { get; set; }
        public Tuple<double, double, double> Min { get; set; }
        public Tuple<double, double, double> Max { get; set; }

        public Tuple<bool, bool, bool> HasErrors
        {
            get
            {
                return new Tuple<bool, bool, bool>(!(First <= Max.Item1 && First >= Min.Item1),
                    !(Second <= Max.Item2 && Second >= Min.Item2),
                    !(Third <= Max.Item3 && Third >= Min.Item3));
            }
        }

        public double First
        {
            get
            {
                return _first;
            }
            set
            {
                if (_first.Equals(value))
                    return;
                    _first = value;
                    if (IsInt)
                        _first = Math.Round(_first);
                    RaisePropertyChanged(nameof(First));
                RaisePropertyChanged(nameof(HasErrors));
            }
        }
        public double Second
        {
            get
            {
                return _second;
            }
            set
            {
                if (_second.Equals(value))
                    return;
                _second = value;
                if (IsInt)
                    _second = Math.Round(_second);
                RaisePropertyChanged(nameof(Second));
                RaisePropertyChanged(nameof(HasErrors));
            }
        }
        public double Third
        {
            get
            {
                return _third;
            }
            set
            {
                if (_third.Equals(value))
                    return;
                _third = value;
                if (IsInt)
                    _third = Math.Round(_third);
                RaisePropertyChanged(nameof(Third));
                RaisePropertyChanged(nameof(HasErrors));
            }
        }

        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                if (value == _firstName)
                    return;
                _firstName = value;
                RaisePropertyChanged(nameof(FirstName));
            }
        }
        public string SecondName
        {
            get
            {
                return _secondName;
            }
            set
            {
                if (value == _secondName)
                    return;
                _secondName = value;
                RaisePropertyChanged(nameof(SecondName));
            }
        }
        public string ThirdName
        {
            get
            {
                return _thirdName;
            }
            set
            {
                if (value == _thirdName)
                    return;
                _thirdName = value;
                RaisePropertyChanged(nameof(ThirdName));
            }
        }
        #endregion

    }
}
