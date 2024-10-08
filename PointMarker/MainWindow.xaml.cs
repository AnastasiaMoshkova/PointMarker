using PointMarker.ViewModel;
using System;
using System.Windows;
using System.Windows.Media.Imaging;


namespace PointMarker
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            MainWindowVM viewModel = new MainWindowVM();
            InitializeComponent();
            this.DataContext = viewModel;


            Uri iconUri = new Uri("logo1.ico", UriKind.RelativeOrAbsolute);
            this.Icon = BitmapFrame.Create(iconUri);

        }
    }
}
