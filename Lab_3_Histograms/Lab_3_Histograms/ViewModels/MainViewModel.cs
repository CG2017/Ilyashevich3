using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.IO;


namespace Lab_3_Histograms.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private int width;
        private int height;
        private Bitmap _image;
        private string _imagePath;
        private ObservableCollection<Histogram> _histograms;
        private Histogram _selectedHistogram = new Histogram();
        private int _pixelsCount;

        public Bitmap Image
        {
            get
            {
                return _image;
            }
            set
            {
                if (value == _image)
                    return;
                _image = value;
                RaisePropertyChanged(nameof(Image));
            }
        }
        public string ImagePath
        {
            get
            {
                return _imagePath;
            }
            set
            {
                if (value == _imagePath)
                    return;
                _imagePath = value;
                RaisePropertyChanged(nameof(ImagePath));
            }
        }
        public ObservableCollection<Histogram> Histograms
        {
            get
            {
                return _histograms;
            }
            set
            {
                if (value == _histograms)
                    return;
                _histograms = value;
                RaisePropertyChanged(nameof(Histograms));
            }
        }
        public Histogram SelectedHistogram
        {
            get
            {
                return _selectedHistogram;
            }
            set
            {
                if (value == _selectedHistogram)
                    return;
                _selectedHistogram = value;
                RaisePropertyChanged(nameof(SelectedHistogram));
            }
        }
        public int PixelsCount
        {
            get
            {
                return _pixelsCount;
            }
            set
            {
                if (value == _pixelsCount)
                    return;
                _pixelsCount = value;
                RaisePropertyChanged(nameof(PixelsCount));
            }
        }

        public MainViewModel()
        {
            ImagePath = @"Images/yellow.png";
            Image = new Bitmap(new FileStream(@"../../" + ImagePath, FileMode.Open));
            byte[,,] rgb =  BitmapToByteRgb(Image);
            PixelsCount = rgb.Length/3;
            int[] red = GetHistogram(rgb, 0);
            int[] green = GetHistogram(rgb, 1);
            int[] blue = GetHistogram(rgb, 2);
            Histograms = new ObservableCollection<Histogram>()
            {
                new Histogram("RGB", GetRGB(red, green, blue), System.Windows.Media.Color.FromRgb(0, 0, 0)),
                new Histogram("Red", red, System.Windows.Media.Color.FromRgb(255, 0, 0)),
                new Histogram("Green", green, System.Windows.Media.Color.FromRgb(0, 255, 0)),
                new Histogram("Blue", blue, System.Windows.Media.Color.FromRgb(0, 0, 255))
            };
            SelectedHistogram = Histograms[0];
        }

        private int[] GetHistogram(byte[,,] rgb, byte color)
        {
            int[] histogram = new int[256];
            for (int i = 0; i < Image.Height; i++)
                for (int j = 0; j < Image.Width; j++)
                    histogram[rgb[color, i, j]]++;
            return histogram;
        }

        private byte[,,] BitmapToByteRgb(Bitmap bmp)
        {
            width = bmp.Width;
            height = bmp.Height;
            byte[,,] res = new byte[3, height, width];
            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    Color color = bmp.GetPixel(x, y);
                    res[0, y, x] = color.R;
                    res[1, y, x] = color.G;
                    res[2, y, x] = color.B;
                }
            }
            return res;
        }

        private int[] GetRGB(int[] red, int[] green, int[] blue)
        {
            int[] histogram = new int[256];
            for (int i = 0; i < 256; i ++)
                histogram[i] = (red[i] + green[i] + blue[i]);
            return histogram;
        }
    }

}
