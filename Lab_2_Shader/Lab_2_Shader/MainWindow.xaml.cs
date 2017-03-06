using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Lab_2_Shader.ViewModels;

namespace Lab_2_Shader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ColorChangerViewModel ccvm = (ColorChangerViewModel) DataContext;
            Point pos = e.GetPosition(Image);
            ccvm.SourceColor = PickColor(pos.X, pos.Y, Image);
        }
        private void Picker_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ColorChangerViewModel ccvm = (ColorChangerViewModel)DataContext;
            Point pos = e.GetPosition(Picker);
            ccvm.TargetColor = PickColor(pos.X, pos.Y, Picker);
        }

        private Color PickColor(double x, double y, Image image)
        {
            BitmapSource bitmapSource = image.Source as BitmapSource;
            if (bitmapSource != null)
            { 
                x *= bitmapSource.PixelWidth / image.ActualWidth;
                if ((int)x > bitmapSource.PixelWidth - 1)
                    x = bitmapSource.PixelWidth - 1;
                else if (x < 0)
                    x = 0;
                y *= bitmapSource.PixelHeight / image.ActualHeight;
                if ((int)y > bitmapSource.PixelHeight - 1)
                    y = bitmapSource.PixelHeight - 1;
                else if (y < 0)
                    y = 0;

                // Lee Brimelow approach (http://thewpfblog.com/?p=62).
                byte[] pixels = new byte[4];
                CroppedBitmap cb = new CroppedBitmap(bitmapSource,
                                   new Int32Rect((int)x, (int)y, 1, 1));
                cb.CopyPixels(pixels, 4, 0);
                return Color.FromArgb(pixels[3], pixels[2], pixels[1], pixels[0]);
            }
            return Colors.Green;
        }
    }
}
