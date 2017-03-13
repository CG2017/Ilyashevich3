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


        public ColorChangerViewModel()
        {
            SourceColor = Color.FromRgb(0,255,0);
            TargetColor = Colors.Yellow;
            Tolerance = 0.3f;
            _shaderNames = new ObservableCollection<String>
            {
                ShaderKeys.Shader76,
                ShaderKeys.Shader94
            };
        }
    }
}
