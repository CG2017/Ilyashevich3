using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_4_Info.ViewModels
{
    public class ImageInfo : ViewModelBase
    {
        private string _name;
        private string _path;
        private int _width;
        private int _height;
        private float _resolution;
        private int _countOfColorsInPalette;
        private PixelFormat _pixelFormat;
        private string _compression;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value == _name)
                    return;
                _name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }
        public string FullPath
        {
            get
            {
                return _path;
            }
            set
            {
                if (value == _path)
                    return;
                _path = value;
                RaisePropertyChanged(nameof(FullPath));
            }
        }

        public string Size => string.Format("{0} x {1}", Width, Height);
        public int Width
        {
            get
            {
                return _width;
            }
            set
            {
                if (value == _width)
                    return;
                _width = value;
                RaisePropertyChanged(nameof(Width));
                RaisePropertyChanged(nameof(Size));
            }
        }
        public int Height
        {
            get
            {
                return _height;
            }
            set
            {
                if (value == _height)
                    return;
                _height = value;
                RaisePropertyChanged(nameof(Height));
                RaisePropertyChanged(nameof(Size));
            }
        }
        public float Resolution
        {
            get
            {
                return _resolution;
            }
            set
            {
                if (value == _resolution)
                    return;
                _resolution = value;
                RaisePropertyChanged(nameof(Resolution));
            }
        }
        public int CountOfColorsInPalette
        {
            get
            {
                return _countOfColorsInPalette;
            }
            set
            {
                if (value == _countOfColorsInPalette)
                    return;
                _countOfColorsInPalette = value;
                RaisePropertyChanged(nameof(CountOfColorsInPalette));
            }
        }
        public PixelFormat PixelFormat
        {
            get
            {
                return _pixelFormat;
            }
            set
            {
                if (value == _pixelFormat)
                    return;
                _pixelFormat = value;
                RaisePropertyChanged(nameof(PixelFormat));
            }
        }
        public string Compression
        {
            get
            {
                return _compression;
            }
            set
            {
                if (value == _compression)
                    return;
                _compression = value;
                RaisePropertyChanged(nameof(Compression));
            }
        }
    }
}
