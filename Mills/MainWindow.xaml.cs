using Mills.ViewModels;
using System.Windows;

namespace Mills
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var mainViewModel = new MainViewModel(BoardControl);
            DataContext = mainViewModel;
        }
    }
}
