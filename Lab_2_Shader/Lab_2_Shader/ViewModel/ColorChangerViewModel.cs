using System;
using System.Collections.Generic;
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


        public ColorChangerViewModel()
        {
            SourceColor = Colors.Green;
            TargetColor = Colors.Yellow;
            Tolerance = 0.1f;
        }
    }
}
