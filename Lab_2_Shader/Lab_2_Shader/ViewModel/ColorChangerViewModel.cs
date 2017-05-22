using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using GalaSoft.MvvmLight.CommandWpf;

namespace Lab_2_Shader.ViewModels
{

    class ColorChangerViewModel : ViewModelBase
    {
        private Color _sourceColor;
        private Color _targetColor;
        private float _tolerance;
        private string _imageName;
        private ObservableCollection<String> _shaderNames;

        public Color SourceColor
        {
            get
            {
                return _sourceColor;
            }
            set
            {
                if (_sourceColor == value)
                    return;
                _sourceColor = value;
                RaisePropertyChanged(nameof(SourceColor));
            }
        }
        public Color TargetColor
        {
            get
            {
                return _targetColor;
            }
            set
            {
                if (_targetColor == value)
                    return;
                _targetColor = value;
                RaisePropertyChanged(nameof(TargetColor));
            }
        }
        public float Tolerance
        {
            get
            {
                return _tolerance;
            }
            set
            {
                if (_tolerance == value)
                    return;
                _tolerance = value;
                RaisePropertyChanged(nameof(Tolerance));
            }
        }
        public ObservableCollection<String> ShaderNames
        {
            get
            {
                return _shaderNames;
            }
            set
            {
                if (_shaderNames == value)
                    return;
                _shaderNames = value;
                RaisePropertyChanged(nameof(ShaderNames));
            }
        }
        public string ImageName {
            get { return _imageName; }
            set {
                if (value != _imageName) {
                    _imageName = value;
                    RaisePropertyChanged(nameof(ImageName));
                }
            }
        }

        public RelayCommand ChooseImageCommand { get; private set; }

        public ColorChangerViewModel()
        {
            SourceColor = Color.FromRgb(0,255,0);
            TargetColor = Colors.Yellow;
            Tolerance = 0.25f;
            _shaderNames = new ObservableCollection<String>
            {
                ShaderKeys.Shader76,
                ShaderKeys.Shader94
            };
            ChooseImageCommand = new RelayCommand(ChooseImage);
            ImageName = @"C:/Users/kathe/Documents/3_курс/6_сем/ComputerGraphics/Labs/Lab_2_Shader/Lab_2_Shader/Images/Lab_color_at_luminance_75%.png";
        }
        private void ChooseImage() {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Title = "Open Image";
            dlg.FileName = "Images"; // Default file name
            dlg.DefaultExt = ".jpg"; // Default file extension
            dlg.Filter = "Images (.jpg)|*.jpg; *.jpeg; *.bmp; *.gif; *.png;"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true) {
                // Open document
                ImageName = dlg.FileName;
            }
        }
    }
}
