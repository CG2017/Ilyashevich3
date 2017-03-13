using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Lab_3_Histograms.ViewModels
{
    public class Histogram : ViewModelBase
    {
        private int[] _bytesInfo;
        private SolidColorBrush _colorBrush;
        private double _average;

        public int[] BytesInfo
        {
            get
            {
                return _bytesInfo;
            }
            set
            {
                if (value == _bytesInfo)
                    return;
                _bytesInfo = value;
                RaisePropertyChanged(nameof(BytesInfo));
            }
        }
        public SolidColorBrush ColorBrush
        {
            get { return _colorBrush; }
            set
            {
                if (value == _colorBrush)
                    return;
                _colorBrush = value;
                RaisePropertyChanged(nameof(ColorBrush));
            }
        }
        public double Average
        {
            get
            {
                return _average;
            }
            set
            {
                if (value.Equals(_average))
                    return;
                _average = value;
                RaisePropertyChanged(nameof(Average));
            }
        }

        public Histogram()
        {
            DisplayName = string.Empty;
            BytesInfo = new int[255];
            ColorBrush = new SolidColorBrush();
        }
        public Histogram(string name, int[] bytesInfo, Color color)
        {
            DisplayName = name;
            BytesInfo = bytesInfo;
            ColorBrush = new SolidColorBrush(color);
            double sum = 0, pixelsCount = 0;
            for (int i = 0; i < bytesInfo.Length; i++)
            {
                sum += bytesInfo[i] * i;
                pixelsCount += bytesInfo[i];
            }
            Average = sum / pixelsCount;
        }
    }
}
