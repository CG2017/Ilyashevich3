using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using Color = Lab_1_Color.ViewModels.Color;

namespace Lab_1_Color.Converters
{
    class MyColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color color = (Color) value;
        
            return new SolidColorBrush(
                new System.Windows.Media.Color() { A = 255, R = (byte)color.First, G = (byte)color.Second, B = (byte)color.Third});
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
