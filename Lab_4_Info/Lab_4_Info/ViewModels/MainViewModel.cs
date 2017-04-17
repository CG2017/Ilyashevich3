using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight.Command;


namespace Lab_4_Info.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private bool _isUpdating;
        private bool _showImages;
        private TimeSpan _updateTime;
        private string _imagesPath;
        private ObservableCollection<ImageInfo> _infos;

        public TimeSpan UpdateTime
        {
            get
            {
                return _updateTime;
            }
            set
            {
                if (value == _updateTime)
                    return;
                _updateTime = value;
                RaisePropertyChanged(nameof(UpdateTime));
            }
        }
        public bool ShowImages
        {
            get
            {
                return _showImages;
            }
            set
            {
                if (value == _showImages)
                    return;
                _showImages = value;
                RaisePropertyChanged(nameof(ShowImages));
            }
        }
        public string ImagesPath
        {
            get
            {
                return _imagesPath;
            }
            set
            {
                if (value == _imagesPath)
                    return;
                _imagesPath = value;
                RaisePropertyChanged(nameof(ImagesPath));
            }
        }
        public bool IsUpdating
        {
            get
            {
                return _isUpdating;
            }
            set
            {
                if (value == _isUpdating)
                    return;
                _isUpdating = value;
                RaisePropertyChanged(nameof(IsUpdating));
            }
        }
        public ObservableCollection<ImageInfo> Infos
        {
            get
            {
                return _infos;
            }
            set
            {
                if (value == _infos)
                    return;
                _infos = value;
                RaisePropertyChanged(nameof(Infos));
            }
        }

        private RelayCommand _updateCommand;

        public RelayCommand UpdateCommand => _updateCommand
                                            ?? (_updateCommand = new RelayCommand(
                                                () =>
                                                {
                                                    if (!IsUpdating)
                                                    {
                                                        IsUpdating = true;
                                                        UpdateInfos();
                                                        IsUpdating = false;
                                                    }
                                                })
                                             );

        public MainViewModel()
        {
            //ImagesPath = @"C:\Users\kathe\Documents\3_курс\6_сем\ComputerGraphics\_КГ 3курс ПИ\kg_dlya_lab_2_i_3\Для проверки Lab#3";
            ImagesPath = @"C:\Users\kathe\Documents\3_курс\6_сем\ComputerGraphics\_КГ 3курс ПИ\to_check_lab4\Для проверки Lab#4\";
            ShowImages = true;
            Infos = new ObservableCollection<ImageInfo>();
            IsUpdating = true;
            UpdateInfos();
            IsUpdating = false;
            ShowImages = false;
        }

        public void UpdateInfos()
        {
            string[] fileNames = Directory.GetFiles(ImagesPath, "*");
            Infos.Clear();
            DateTime startTime = DateTime.Now;
            foreach (var fileName in fileNames)
            {
                try
                {     
                    //ImageInfo imageInfo = new ImageInfo();
                    using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
                    {
                        Image image = new Bitmap(fileStream);
                        ImageInfo imageInfo = new ImageInfo()
                        {
                            Name = fileName.Substring(fileName.LastIndexOf(@"\") + 1),
                            Width = image.Width,
                            Height = image.Height,
                            Resolution = (int) image.HorizontalResolution,
                            CountOfColorsInPalette = image.Palette.Entries.Length,
                            PixelFormat = image.PixelFormat
                        };


                        int compressionTagIndex = Array.IndexOf(image.PropertyIdList, 0x103);
                        int Type = 0;
                        if (compressionTagIndex > -1)
                        {
                            PropertyItem compressionTag = image.PropertyItems[compressionTagIndex];
                            Type = BitConverter.ToInt16(compressionTag.Value, 0);
                        }
                        imageInfo.Compression = "No compression";
                        switch (Type)
                        {
                            case 2:
                                imageInfo.Compression = "CCITT modified Huffman RLE";
                                break;
                            case 3:
                                imageInfo.Compression = "CCITT Group 3 fax encoding";
                                break;
                            case 4:
                                imageInfo.Compression = "CCITT Group 4 fax encoding";
                                break;
                            case 5:
                                imageInfo.Compression = "LZW";
                                break;
                            case 6:
                                imageInfo.Compression = "'old-style' JPEG";
                                break;
                            case 7:
                                imageInfo.Compression = "'new-style' JPEG";
                                break;
                            case 32773:
                                imageInfo.Compression = "Macintosh RLE";
                                break;
                            default:
                                break;
                        }
                        Infos.Add(imageInfo);
                    }
                    //if (ShowImages)
                    //    imageInfo.FullPath = fileName;
                    //Infos.Add(imageInfo);
                }
               catch (Exception ex){}
            }
            DateTime endTime = DateTime.Now;
            UpdateTime = endTime.Subtract(startTime);
        }
    }

}
