using System.Windows;
using GalaSoft.MvvmLight.Threading;

namespace Lab_5_Rasterization
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            DispatcherHelper.Initialize();
        }
    }
}
