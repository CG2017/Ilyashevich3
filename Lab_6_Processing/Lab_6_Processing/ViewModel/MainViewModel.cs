using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Lab_6_Processing.Model;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.Win32;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;

namespace Lab_6_Processing.ViewModel {

    public class MainViewModel : ViewModelBase {
        #region Private fields

        private const int NIBLACK_BLOCK_SIZE = 15;
        private const int K = 5;
        private const double ALPHA = 0.95; //от 0.3 до 0.8
        private const int ADAPTIVE_BLOCK_SIZE = 2 * K + 1;
        private int counter = 1;
        private string resultPath;
        private readonly IDataService _dataService;
        private string _imageName;
        private bool _fileWasChanged = false;
        private string _selectedAlg = Model.AlgorithmNames.GlobalThresholdProcessingHistogram;
        private ObservableCollection<string> _algorithmNames = new ObservableCollection<string>()
        {
            Model.AlgorithmNames.GlobalThresholdProcessingHistogram,
            Model.AlgorithmNames.LocalThresholdProcessingNiblack,
            Model.AlgorithmNames.AdaptiveThresholdProcessing,
            Model.AlgorithmNames.LinearContrasting
        };
        private BitmapImage _image;

        #endregion

        #region Properties

        public string ImageName {
            get { return _imageName; }
            set {
                if (value != _imageName) {
                    _imageName = value;
                    RaisePropertyChanged(() => ImageName);
                    RaisePropertyChanged(() => DisplayedImagePath);
                    RaisePropertyChanged(() => DisplayedNewImagePath);
                }
            }
        }
        public string SelectedAlg {
            get { return _selectedAlg; }
            set {
                if (value != _selectedAlg) {
                    _selectedAlg = value;
                    RaisePropertyChanged(() => SelectedAlg);
                }
            }
        }
        public ObservableCollection<string> AlgorithmNames => _algorithmNames;
        public string DisplayedImagePath => /*_fileWasChanged ?*/ ImageName; /*:*/
                                                                             //"pack://application:,,,/" + Assembly.GetEntryAssembly().GetName().Name + ";component/Images/1305х864х183.jpg";
        public string DisplayedNewImagePath => DisplayedImagePath.Substring(0, DisplayedImagePath.IndexOf(".")) +
                                               "_2" + DisplayedImagePath.Substring(DisplayedImagePath.IndexOf("."));

        public BitmapImage Image {
            get {
                return _image;
            }
            set {
                if (value == _image)
                    return;
                _image = value;
                RaisePropertyChanged(() => Image);
            }
        }

        #endregion


        public MainViewModel(IDataService dataService) {
            _dataService = dataService;
            _dataService.GetData(
                (item, error) => {
                    if (error != null) {
                        // Report error here
                        return;
                    }
                });
            ImageName = @"C:/Users/kathe/Documents/3_курс/6_сем/ComputerGraphics/Labs/Lab_6_Processing/Lab_6_Processing/Images/1305х864х183.jpg";
            Image = new BitmapImage(new Uri(DisplayedImagePath));
            InitializeCommands();
        }

        #region Commands

        public RelayCommand ChooseImageCommand { get; private set; }
        public RelayCommand UpdateResultCommand { get; private set; }

        #endregion

        #region Methods

        private void InitializeCommands() {
            ChooseImageCommand = new RelayCommand(ChooseImage);
            UpdateResultCommand = new RelayCommand(UpdateResult);
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
                _fileWasChanged = true;
                ImageName = dlg.FileName;
            }
        }

        private void UpdateResult() {
            Image = new BitmapImage(new Uri(DisplayedImagePath));
            resultPath = GenerateName();
            switch (SelectedAlg) {
                case Model.AlgorithmNames.GlobalThresholdProcessingHistogram:
                    GlobalThresholdProcessingHistogram();
                    break;
                case Model.AlgorithmNames.LocalThresholdProcessingNiblack:
                    LocalThresholdProcessingNiblack();
                    break;
                case Model.AlgorithmNames.AdaptiveThresholdProcessing:
                    AdaptiveThresholdProcessing();
                    break;
                case Model.AlgorithmNames.LinearContrasting:
                    LinearContrasting();
                    break;
                default:
                    break;
            }
            //RaisePropertyChanged(() => DisplayedResultPath);
            Image = new BitmapImage(new Uri(resultPath));
        }

        private Bitmap CopyImage(string path) {
            Bitmap myBitmap = new Bitmap(path);

            // Clone a portion of the Bitmap object.
            Rectangle cloneRect = new Rectangle(0, 0, myBitmap.Width, myBitmap.Height);
            System.Drawing.Imaging.PixelFormat format =
                myBitmap.PixelFormat;
            Bitmap cloneBitmap = myBitmap.Clone(cloneRect, format);
            FileStream outFileStream = new FileStream(resultPath, FileMode.OpenOrCreate);
            //outFileStream.Write(cloneBitmap.);
            cloneBitmap.Save(new StreamWriter(outFileStream).ToString());
            myBitmap.Dispose();
            return cloneBitmap;
        }

        private void SetPixel(Bitmap bitmap, int x, int y, Color color) {
            if (bitmap.PixelFormat == PixelFormat.Format8bppIndexed) {
                BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);

                // Copy the bytes from the image into a byte array
                byte[] bytes = new byte[data.Height * data.Stride];
                Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);

                bytes[y * data.Stride + x] = color.B; // Set the pixel at (5, 5) to the color #1

                // Copy the bytes from the byte array into the image
                Marshal.Copy(bytes, 0, data.Scan0, bytes.Length);

                bitmap.UnlockBits(data);
            }
            else {
                bitmap.SetPixel(x, y, color);
            }
        }

        private string GenerateName() {
            return string.Format("C:/Users/kathe/Documents/3_курс/6_сем/ComputerGraphics/Labs/Lab_6_Processing/Lab_6_Processing/Images/Temp/{0}.{1}",
                counter++, DisplayedImagePath.Substring(DisplayedImagePath.IndexOf(".") + 1));
        }

        #region GlobalThresholdProcessingHistogram

        private void GlobalThresholdProcessingHistogram() {
            //CopyImage(DisplayedNewImagePath);
            using (Bitmap im = new Bitmap(new FileStream(DisplayedNewImagePath, FileMode.Open))) {
                byte[,,] rgb = BitmapToByteRgb(im);
                int[] red = GetHistogram(rgb, 0);
                int[] green = GetHistogram(rgb, 1);
                int[] blue = GetHistogram(rgb, 2);
                int[] rgbHistogram = GetRGB(red, green, blue);

                int threshold = 0;
                int newThreshold = 128;
                while (Math.Abs(threshold - newThreshold) > 0) {
                    threshold = newThreshold;
                    int count1 = 0, count2 = 0, sum1 = 0, sum2 = 0;
                    for (int i = 0; i < threshold; i++) {
                        sum1 += i * rgbHistogram[i];
                        count1 += rgbHistogram[i];
                    }
                    for (int i = threshold; i < rgbHistogram.Length; i++) {
                        sum2 += i * rgbHistogram[i];
                        count2 += rgbHistogram[i];
                    }
                    int av1 = count1 != 0 ? sum1 / count1 : 0;
                    int av2 = count2 != 0 ? sum2 / count2 : 0;
                    newThreshold = (av1 + av2) / 2;
                }
                for (int y = 0; y < im.Height; ++y) {
                    for (int x = 0; x < im.Width; ++x) {
                        //im.SetPixel(x, y, (rgb[0, y, x] + rgb[1, y, x] + rgb[2, y, x]) / 3 > newThreshold ?
                        //    Color.White : Color.Black);
                        SetPixel(im, x, y, (rgb[0, y, x] + rgb[1, y, x] + rgb[2, y, x]) / 3 > newThreshold ?
                            Color.White : Color.Black);
                    }
                }
                im.Save(resultPath);
            }
            System.GC.Collect();
        }

        private byte[,,] BitmapToByteRgb(Bitmap bmp) {
            byte[,,] res = new byte[3, bmp.Height, bmp.Width];
            for (int y = 0; y < bmp.Height; ++y) {
                for (int x = 0; x < bmp.Width; ++x) {
                    Color color = bmp.GetPixel(x, y);
                    res[0, y, x] = color.R;
                    res[1, y, x] = color.G;
                    res[2, y, x] = color.B;
                }
            }
            return res;
        }
        private int[] GetHistogram(byte[,,] rgb, byte color) {
            int[] histogram = new int[256];
            for (int i = 0; i < Image.Height; i++)
                for (int j = 0; j < Image.Width; j++)
                    histogram[rgb[color, i, j]]++;
            return histogram;
        }
        private int[] GetRGB(int[] red, int[] green, int[] blue) {
            int[] histogram = new int[256];
            for (int i = 0; i < 256; i++)
                histogram[i] = (red[i] + green[i] + blue[i]);
            return histogram;
        }
        #endregion

        #region LocalThresholdProcessingNiblack

        private void LocalThresholdProcessingNiblack() {
            using (Bitmap im = new Bitmap(new FileStream(DisplayedNewImagePath, FileMode.Open))) {
                byte[,,] rgb = BitmapToByteRgb(im);
                int rowsCount = im.Height / NIBLACK_BLOCK_SIZE + (im.Height % NIBLACK_BLOCK_SIZE > 0 ? 1 : 0);
                int columnsCount = im.Width / NIBLACK_BLOCK_SIZE + (im.Width % NIBLACK_BLOCK_SIZE > 0 ? 1 : 0);
                for (int i = 0; i < rowsCount; i++) {
                    for (int j = 0; j < columnsCount; j++) {
                        int maxI = (i == rowsCount - 1) ? (im.Height % NIBLACK_BLOCK_SIZE > 0 ? im.Height % NIBLACK_BLOCK_SIZE : NIBLACK_BLOCK_SIZE) : NIBLACK_BLOCK_SIZE;
                        int maxJ = (j == columnsCount - 1) ? (im.Width % NIBLACK_BLOCK_SIZE > 0 ? im.Width % NIBLACK_BLOCK_SIZE : NIBLACK_BLOCK_SIZE) : NIBLACK_BLOCK_SIZE;
                        double average = GetAverage(i, j, rgb, maxI, maxJ);
                        double deviation = GetDeviation(i, j, rgb,
                            maxI, maxJ, average);
                        int threshold = (int)(average - 0.2 * deviation);
                        Repaint(i, j, im, rgb, maxI, maxJ, threshold);
                    }
                }
                im.Save(resultPath);
            }
            System.GC.Collect();
        }

        private void Repaint(int row, int column, Bitmap im, byte[,,] rgb, int maxI, int maxJ, int threshold) {
            for (int i = 0; i < maxI; i++) {
                for (int j = 0; j < maxJ; j++) {
                    int x = column * NIBLACK_BLOCK_SIZE + j;
                    int y = row * NIBLACK_BLOCK_SIZE + i;
                    im.SetPixel(x, y,
                        (rgb[0, y, x]
                        + rgb[1, y, x]
                        + rgb[2, y, x]) / 3 >= threshold ?
                        Color.White : Color.Black);
                }
            }
        }
        private double GetAverage(int row, int column, byte[,,] rgb, int maxI, int maxJ) {
            double sum = 0;
            int count = maxI * maxJ;
            for (int i = 0; i < maxI; i++) {
                for (int j = 0; j < maxJ; j++) {
                    int x = column * NIBLACK_BLOCK_SIZE + j;
                    int y = row * NIBLACK_BLOCK_SIZE + i;
                    sum += (rgb[0, y, x]
                        + rgb[1, y, x]
                        + rgb[2, y, x]) / (double)3;
                }
            }
            return sum / count;
        }
        private double GetDeviation(int row, int column, byte[,,] rgb, int maxI, int maxJ, double average) {
            double sum = 0;
            int count = maxI * maxJ;
            for (int i = 0; i < maxI; i++) {
                for (int j = 0; j < maxJ; j++) {
                    int x = column * NIBLACK_BLOCK_SIZE + j;
                    int y = row * NIBLACK_BLOCK_SIZE + i;
                    sum += Math.Pow((rgb[0, y, x]
                        + rgb[1, y, x]
                        + rgb[2, y, x]) / 3.0
                        - average,
                        2);
                }
            }
            return Math.Sqrt(1 / ((double)count - 1) * sum);
        }

        #endregion

        #region AdaptiveThresholdProcessing

        private void AdaptiveThresholdProcessing() {
            //CopyImage(DisplayedNewImagePath);
            using (Bitmap im = new Bitmap(new FileStream(DisplayedNewImagePath, FileMode.Open))) {
                byte[,,] rgb = BitmapToByteRgb(im);
                int[,] thresholds = new int[im.Height, im.Width];
                for (int i = 0; i < im.Height; i++) {
                    for (int j = 0; j < im.Width; j++) {
                        int startI = Math.Max(0, i - K);
                        int startJ = Math.Max(0, j - K);
                        int finishI = Math.Min(im.Height - 1, i + K);
                        int finishJ = Math.Min(im.Width - 1, j + K);
                        thresholds[i,j] = findT(rgb, startI, startJ, finishI, finishJ);
                    }
                }
                for (int y = 0; y < im.Height; ++y) {
                    for (int x = 0; x < im.Width; ++x) {
                        SetPixel(im, x, y, (rgb[0, y, x] + rgb[1, y, x] + rgb[2, y, x]) / 3 > thresholds[y, x] ?
                            Color.White : Color.Black);
                    }
                }
                im.Save(resultPath);
            }
            System.GC.Collect();
        }

        private int findT(byte[,,] rgb, int startI, int startJ, int finishI, int finishJ)
        {
            int max = -1;
            int min = 255;
            int sum = 0;
            for (int i = startI; i <= finishI; i++)
            {
                for (int j = startJ; j <= finishJ; j++)
                {
                    int brightness = (rgb[0, i, j]
                                      + rgb[1, i, j]
                                      + rgb[2, i, j])/3;
                    if (brightness > max)
                        max = brightness;
                    if (brightness < min)
                        min = brightness;
                    sum += brightness;
                }
            }
            double P = (double)sum/((finishI - startI + 1)*(finishJ - startJ + 1));
            double deltaMax = Math.Abs(P - max);
            double deltaMin = Math.Abs(P - min);
            if (deltaMax /*> */>= deltaMin)
                return (int)Math.Round(ALPHA * (2.0 *min/3 + max/3.0));
            return (int)Math.Round(ALPHA * (min / 3.0 + 2 * max / 3.0));
        }
        #endregion

        #region LinearContrasting

        private void LinearContrasting() {
            using (Bitmap im = new Bitmap(new FileStream(DisplayedNewImagePath, FileMode.Open))) {
                byte[,,] rgb = BitmapToByteRgb(im);
                int max = -1;
                int min = 255;
                for (int i = 0; i < im.Height; i++) {
                    for (int j = 0; j < im.Width; j++)
                    {
                        int brightness = GetBrightness(rgb, i, j);
                        if (brightness > max)
                            max = brightness;
                        if (brightness < min)
                            min = brightness;
                    }
                }
                for (int y = 0; y < im.Height; ++y) {
                    for (int x = 0; x < im.Width; ++x)
                    {
                        Color color = im.GetPixel(x, y);
                        double coef = ((GetBrightness(rgb, y, x) - min)*(254.0 - 0)/(max - min) + 0) / 254;
                        Color newColor = Color.FromArgb(255,
                            (int)(color.R * coef), (int)(color.G * coef), (int)(color.B * coef));
                        SetPixel(im, x, y, newColor);
                    }
                }
                im.Save(resultPath);
            }
            System.GC.Collect();
        }

        private int GetBrightness(byte[,,] rgb, int i, int j)
        {
            return (rgb[0, i, j] + rgb[1, i, j] + rgb[2, i, j])/3;
        }
        #endregion
        #endregion
    }
}