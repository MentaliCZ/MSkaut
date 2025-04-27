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

namespace UserInterface
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ConnectionInstance dbConnection;
        public MainWindow()
        {   
            InitializeComponent();
            initDBConnection();
        }

        private async Task initDBConnection() 
        {
            dbConnection = await ConnectionInstance.CreateInstance();
        }


        private async void LoginButton(object sender, RoutedEventArgs e)
        {
            if (usernameInput.Text == null || passwordInput == null)
            {
                //TODO, write some kind of error message
                return;
            }

            User? user = await User.TryLogin(usernameInput.Text, passwordInput.Text, dbConnection.Client);

            if (user == null)
            {
                //TODO, write some kind of error message
                return;
            }

            //TODO: change screen to main page

        }

        private async void Create_new_User_Click(object sender, RoutedEventArgs e)
        {
            if (usernameInput.Text == null || passwordInput == null)
            {
                //TODO, write some kind of error message
                return;
            }

            if (!await User.CreateUser(usernameInput.Text, passwordInput.Text, dbConnection.Client))
            {
                //TODO, write some kind of error message
            }

            //TODO, write some supportive message
        }
    }
}