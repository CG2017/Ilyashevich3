using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Lab_1_Color.Converters
{
    class ColorToMyColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
             ViewModels.Color myColor = (ViewModels.Color) value;
            return Color.FromRgb((byte)myColor.First, (byte)myColor.Second, (byte)myColor.Third);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color color = (Color)value;
            ViewModels.Color myColor = new ViewModels.Color()
            {
                DisplayName = ColorNames.RGB,
                First = color.R,
                Second = color.G,
                Third = color.B,
                FirstName = "R",
                SecondName = "G",
                ThirdName = "B",
                Max = new Tuple<double, double, double>(255, 255, 255),
                IsInt = true
            };
            return myColor;
        }
    }
}
