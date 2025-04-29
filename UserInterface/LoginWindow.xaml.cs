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
using UserInterface.Commands;

namespace UserInterface
{

    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {   
            InitializeComponent();
            LoginViewModel loginViewModel = new(this);
            DataContext = loginViewModel;
        }

        public LoginWindow(ConnectionInstance dbInstance)
        {
            InitializeComponent();
            LoginViewModel loginViewModel = new(this);
            DataContext = loginViewModel;
        }
    }
}