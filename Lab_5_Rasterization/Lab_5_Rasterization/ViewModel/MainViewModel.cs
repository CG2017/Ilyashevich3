using System;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using Lab_5_Rasterization.Model;

namespace Lab_5_Rasterization.ViewModel
{

    public class MainViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;

        public const string WelcomeTitlePropertyName = "WelcomeTitle";

        private string _welcomeTitle = string.Empty;
        private ObservableCollection<String> _typesOfRasterization = new ObservableCollection<string>()
        {
            AlgorithmsNames.StepByStep,
            AlgorithmsNames.DDA,
            AlgorithmsNames.BresenhamLine,
            AlgorithmsNames.BresenhamCircle
        };
        private string _selectedTypesOfRasterization;
        private int _min = 1;
        private int _max = 20;
        private int _startX = 4;
        private int _startY = 4;
        private int _finishX = 20;
        private int _finishY = 20;
        private int _radius = 3;

        public string WelcomeTitle
        {
            get
            {
                return _welcomeTitle;
            }
            set
            {
                Set(ref _welcomeTitle, value);
            }
        }
        public ObservableCollection<String> TypesOfRasterization
        {
            get
            {
                return _typesOfRasterization;
            }
            set
            {
                Set(ref _typesOfRasterization, value);
            }
        }
        public string SelectedTypesOfRasterization
        {
            get
            {
                return _selectedTypesOfRasterization;
            }
            set
            {
                Set(ref _selectedTypesOfRasterization, value);
            }
        }
        public int StartX
        {
            get
            {
                return _startX;
            }
            set
            { 
                if(value <= _max && value >= _min)
                    Set(ref _startX, value);
            }
        }
        public int StartY
        {
            get
            {
                return _startY;
            }
            set
            {
                if (value <= _max && value >= _min)
                    Set(ref _startY, value);
            }
        }
        public int FinishX
        {
            get
            {
                return _finishX;
            }
            set
            {
                if (value <= _max && value >= _min)
                    Set(ref _finishX, value);
            }
        }
        public int FinishY
        {
            get
            {
                return _finishY;
            }
            set
            {
                if (value <= _max && value >= _min)
                    Set(ref _finishY, value);
            }
        }
        public int Radius
        {
            get
            {
                return _radius;
            }
            set
            {
                if (value <= _max && value >= _min)
                    Set(ref _radius, value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataService dataService)
        {
            _selectedTypesOfRasterization = _typesOfRasterization.First();
            _dataService = dataService;
            _dataService.GetData(
                (item, error) =>
                {
                    if (error != null)
                    {
                        // Report error here
                        return;
                    }

                    WelcomeTitle = item.Title;
                });
        }

        public bool ValidatePoint(int x, int y)
        {
            if (x <= _max && x >= _min && y <= _max && y >= _min)
                return true;
            return false;
        }
    }
}