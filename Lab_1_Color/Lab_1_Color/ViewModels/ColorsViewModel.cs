using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using Lab_1_Color.Common;

namespace Lab_1_Color.ViewModels
{
    public class ColorsViewModel : ViewModelBase
    {
        private ObservableCollection<Color> _colors = new ObservableCollection<Color>();
        private Color _selectedColor;

        public ObservableCollection<Color> Colors
        {
            get
            {
                return _colors;
            }
            set
            {
                if (value == _colors)
                    return;
                _colors = value;
                RaisePropertyChanged(nameof(Colors));
            }
        }
        public Color SelectedColor
        {
            get
            {
                return _selectedColor;
            }
            set
            {
                if (_selectedColor == value)
                    return;
                _selectedColor = value;
                RaisePropertyChanged(nameof(SelectedColor));
            }
        }

        public Color Background
        {
            get { return Colors[0]; }
            set
            {
                if(Colors[0].Equals(value))
                    return;
                Colors[0] = value;
                //RaisePropertyChanged(nameof(Background));
            }
        }

        public ColorsViewModel()
        {
            Colors.CollectionChanged += ColorsCollectionChanged;
            Colors.Add(new Color()
            {
                DisplayName = ColorNames.RGB,
                First = 0,
                Second = 0,
                Third = 0,
                FirstName = "R",
                SecondName = "G",
                ThirdName = "B", 
                Max = new Tuple<double, double, double>(255,255,255),
                IsInt = true
            });
            Colors.Add(new Color()
            {
                DisplayName = ColorNames.CMY,
                First = 0,
                Second = 0,
                Third = 0,
                FirstName = "C",
                SecondName = "M",
                ThirdName = "Y",
                Max = new Tuple<double, double, double>(1, 1, 1)
            });
            Colors.Add(new Color()
            {
                DisplayName = ColorNames.HSV,
                First = 0,
                Second = 0,
                Third = 0,
                FirstName = "H",
                SecondName = "S",
                ThirdName = "V",
                Max = new Tuple<double, double, double>(360, 1, 1)
            });
            Colors.Add(new Color()
            {
                DisplayName = ColorNames.Lab,
                First = 0,
                Second = 0,
                Third = 0,
                FirstName = "L*",
                SecondName = "a*",
                ThirdName = "b",
                Max = new Tuple<double, double, double>(100, 100, 100),
                Min = new Tuple<double, double, double>(0, -100, -100)
            });
            SelectedColor = Colors[0];
            Colors[0].Third--;
        }

        public void ColorsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (Color item in e.OldItems)
                {
                    item.PropertyChanged -= ColorPropertyChanged;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (Color item in e.NewItems)
                {
                    item.PropertyChanged += ColorPropertyChanged;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                foreach (Color item in e.OldItems)
                {
                    item.PropertyChanged -= ColorPropertyChanged;
                }
                foreach (Color item in e.NewItems)
                {
                    SelectedColor = Colors[0];
                    item.PropertyChanged += ColorPropertyChanged;
                    ColorPropertyChanged(item, null);
                }
            }
        }

        public void ColorPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Color changed = sender as Color;
            if(changed != SelectedColor)
                return;
            Color rgb = Colors[0];
            Color cmy = Colors[1];
            Color hsv = Colors[2];
            Color lab = Colors[3];
            switch (changed.DisplayName)
            {
                case ColorNames.RGB:
                    ConvertColorsService.FromRgbToCmy(changed, ref cmy);
                    ConvertColorsService.FromRgbToHsv(changed, ref hsv);
                    ConvertColorsService.FromRgbToLab(changed, ref lab);
                    break;
                case ColorNames.CMY:
                    ConvertColorsService.FromCmyToRgb(changed, ref rgb);
                    ConvertColorsService.FromRgbToHsv(rgb, ref hsv);
                    ConvertColorsService.FromRgbToLab(rgb, ref lab);
                    break;
                case ColorNames.HSV:
                    ConvertColorsService.FromHsvToRgb(changed, ref rgb);
                    ConvertColorsService.FromRgbToCmy(rgb, ref cmy);
                    ConvertColorsService.FromRgbToLab(rgb, ref lab);
                    break;
                case ColorNames.Lab:
                    ConvertColorsService.FromLabToRgb(changed, ref rgb);
                    ConvertColorsService.FromRgbToCmy(rgb, ref cmy);
                    ConvertColorsService.FromRgbToHsv(rgb, ref hsv);
                    break;

                default:
                    break;
            }
            RaisePropertyChanged(nameof(Background));
        } 
    }
}
