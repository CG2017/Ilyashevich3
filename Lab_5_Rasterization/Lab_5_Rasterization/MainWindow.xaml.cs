using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Lab_5_Rasterization.Model;
using Lab_5_Rasterization.ViewModel;

namespace Lab_5_Rasterization
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel mainViewModel;
        private List<Rectangle> rectangles = new List<Rectangle>();
        private Ellipse circle = new Ellipse();
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            mainViewModel = DataContext as MainViewModel;
            //Canvas.Children.Add(new Line() {X1 = 0, X2 = 600, Y1 = 30, Y2 = 30});
            PaintCanvas();
            Closing += (s, e) => ViewModelLocator.Cleanup();
        }

        private void PaintCanvas()
        {
            double size = Canvas.Width;
            for (int i = 1; i < 20; i++)
            {
                int t = (int)size*i/20;
                Canvas.Children.Add(new Line() { X1 = 0, X2 = 600, Y1 = t, Y2 = t });
                Canvas.Children.Add(new Line() { X1 = t, X2 = t, Y1 = 0, Y2 = 600 });
            }
        }

        private void Refresh_Button_Click(object sender, RoutedEventArgs e)
        {
            Clean_Button_Click(sender, e);
            switch (mainViewModel.SelectedTypesOfRasterization)
            {
                case AlgorithmsNames.StepByStep:
                    UseStepByStep();
                    break;
                case AlgorithmsNames.DDA:
                    UseDDA();
                    break;
                case AlgorithmsNames.BresenhamLine:
                    UseBresenham4Line(mainViewModel.StartX, mainViewModel.StartY, 
                        mainViewModel.FinishX, mainViewModel.FinishY);
                    break;
                case AlgorithmsNames.BresenhamCircle:
                    MyBresenhamCircle(mainViewModel.StartX, mainViewModel.StartY,
                        mainViewModel.Radius);
                    break;
                default:
                    break;
            }
        }

        private void UseStepByStep()
        {
            int x = mainViewModel.StartX;
            int y = mainViewModel.StartY;
            double k = ((double)y - (double)mainViewModel.FinishY)/((double)x - (double)mainViewModel.FinishX);
            double b = y - k*x;
            Paint(x, y);
            while (x != mainViewModel.FinishX)
            {
                if (mainViewModel.FinishX > x)
                    x++;
                else
                    x--;
                int newY = (int)Math.Round(k*x + b);
                while (y != newY)
                {
                    if(newY > y)
                        y++;
                    else
                        y--;
                    Paint(x, y);
                }
            }
        }
        private void UseDDA()
        {
            // Целочисленные значения координат начала и конца отрезка,
            // округленные до ближайшего целого
            int iX1 = mainViewModel.StartX;
            int iY1 = mainViewModel.StartY;
            int iX2 = mainViewModel.FinishX;
            int iY2 = mainViewModel.FinishY;

            // Длина и высота линии
            int deltaX = Math.Abs(iX1 - iX2);
            int deltaY = Math.Abs(iY1 - iY2);

            // Считаем минимальное количество итераций, необходимое
            // для отрисовки отрезка. Выбирая максимум из длины и высоты
            // линии, обеспечиваем связность линии
            int length = Math.Max(deltaX, deltaY);

            // особый случай, на экране закрашивается ровно один пиксел
            if (length == 0)
            {
                Paint(iX1, iY1);
                return;
            }

            // Вычисляем приращения на каждом шаге по осям абсцисс и ординат
            double dX = ((double)mainViewModel.FinishX - (double)mainViewModel.StartX) / length;
            double dY = ((double)mainViewModel.FinishY - (double)mainViewModel.StartY) / length;

            // Начальные значения
            double x = (double)mainViewModel.StartX;
            double y = (double)mainViewModel.StartY;

            // Основной цикл
            length++;
            while (length-- != 0)
            {
                Paint((int)Math.Round(x), (int)Math.Round(y));
                x += dX;
                y += dY;            
            }
        }
        private void UseBresenham4Line(int x0, int y0, int x1, int y1)
        {
            //Изменения координат
            int dx = (x1 > x0) ? (x1 - x0) : (x0 - x1);
            int dy = (y1 > y0) ? (y1 - y0) : (y0 - y1);
            //Направление приращения
            int sx = (x1 >= x0) ? (1) : (-1);
            int sy = (y1 >= y0) ? (1) : (-1);

            if (dy < dx)
            {
                int d = (dy << 1) - dx;
                int d1 = dy << 1;
                int d2 = (dy - dx) << 1;
                Paint(x0, y0);
                int x = x0 + sx;
                int y = y0;
                for (int i = 1; i <= dx; i++)
                {
                    if (d > 0)
                    {
                        d += d2;
                        y += sy;
                    }
                    else
                        d += d1;
                    Paint(x, y);
                    x += sx;
                }
            }
            else
            {
                int d = (dx << 1) - dy;
                int d1 = dx << 1;
                int d2 = (dx - dy) << 1;
                Paint(x0, y0);
                int x = x0;
                int y = y0 + sy;
                for (int i = 1; i <= dy; i++)
                {
                    if (d > 0)
                    {
                        d += d2;
                        x += sx;
                    }
                    else
                        d += d1;
                    Paint(x, y);
                    y += sy;
                }
            }
        }
        public void MyBresenhamCircle(int x0, int y0, int radius) {
            int x = 0;
            int y = radius;
            PutCirclePixel(x0, y0, x, y);
            while (y > 0) {
                int xr = x + 1;
                int xv = x;
                int xd = x + 1;
                int yr = y;
                int yv = y - 1;
                int yd = y - 1;
                double diffR = CountDiff(xr, yr, radius);
                double diffV = CountDiff(xv, yv, radius);
                double diffD = CountDiff(xd, yd, radius);
                if (diffR < diffD && diffR < diffV) {
                    PutCirclePixel(x0, y0, xr, yr);
                    x = xr;
                    y = yr;
                }
                else if (diffV < diffD && diffV < diffD) {
                    PutCirclePixel(x0, y0, xv, yv);
                    x = xv;
                    y = yv;
                }
                else {
                    PutCirclePixel(x0, y0, xd, yd);
                    x = xd;
                    y = yd;
                }
            }
            circle = new Ellipse() {
                Margin = new Thickness((x0 - radius) * 30 - 15, 600 - ((y0 + radius) * 30 - 15), 0, 0),
                Width = radius * 2 * 30,
                Height = radius * 2 * 30,
                Stroke = new SolidColorBrush(Colors.Red)
            };
            Canvas.Children.Add(circle);
        }
        public void PutCirclePixel(int x0, int y0, int x, int y) {
            Paint(x0 + x, y0 + y);
            Paint(x0 + x, y0 - y);
            Paint(x0 - x, y0 - y);
            Paint(x0 - x, y0 + y);
        }

        public double CountDiff(int x, int y, int radius) {
            double dist = Math.Sqrt(x * x + y * y);
            double diff = Math.Abs(dist - radius);
            return diff;
        }
        public void UseBresenhamCircle(int _x, int _y, int radius) {
            int x = 0, y = radius, gap = 0, delta = (2 - 2 * radius);
            while (y >= 0) {
                if (mainViewModel.ValidatePoint(_x + x, _y + y))
                    Paint(_x + x, _y + y);
                if (mainViewModel.ValidatePoint(_x + x, _y - y))
                    Paint(_x + x, _y - y);
                if (mainViewModel.ValidatePoint(_x - x, _y - y))
                    Paint(_x - x, _y - y);
                if (mainViewModel.ValidatePoint(_x - x, _y + y))
                    Paint(_x - x, _y + y);
                gap = 2 * (delta + y) - 1;
                if (delta < 0 && gap <= 0) {
                    x++;
                    delta += 2 * x + 1;
                    continue;
                }
                if (delta > 0 && gap > 0) {
                    y--;
                    delta -= 2 * y + 1;
                    continue;
                }
                x++;
                delta += 2 * (x - y);
                y--;
            }
            circle = new Ellipse() {
                Margin = new Thickness((_x - radius) * 30 - 15, 600 - ((_y + radius) * 30 - 15), 0, 0),
                Width = radius * 2 * 30,
                Height = radius * 2 * 30,
                Stroke = new SolidColorBrush(Colors.Red)
            };
            Canvas.Children.Add(circle);
        }
        private void Paint(int x, int y)
        {
            Rectangle myRect = new System.Windows.Shapes.Rectangle();
            myRect.Stroke = System.Windows.Media.Brushes.DarkBlue;
            myRect.Fill = System.Windows.Media.Brushes.Blue;
            myRect.Margin = new Thickness((x - 1) * 30, (20 - y) * 30, 0, 0);
            myRect.Height = 30;
            myRect.Width = 30;
            rectangles.Add(myRect);
            Canvas.Children.Add(myRect);
        }

        private void Clean_Button_Click(object sender, RoutedEventArgs e)
        {
            foreach(var rect in rectangles)
                Canvas.Children.Remove(rect);
            rectangles.Clear();
            Canvas.Children.Remove(circle);
        }
    }
}