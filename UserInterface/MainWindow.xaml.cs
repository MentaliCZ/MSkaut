using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using UserManager;
using DatabaseManager;
using UserInterface.ViewModels;
using WpfAnimatedGif;

namespace UserInterface
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindow()
        {   
            InitializeComponent();
        }

        private async Task Init(User user, ConnectionInstance dbConnection)
        {
            MainViewModel mainViewModel = await MainViewModel.CreateMainViewModel(this, user, dbConnection);
            DataContext = mainViewModel;
        }

        public static async Task<MainWindow> CreateMainWindow(User user, ConnectionInstance dbConnection)
        {
            MainWindow mainWindow = new MainWindow();
            await mainWindow.Init(user, dbConnection);
            return mainWindow;
        }

    }
}