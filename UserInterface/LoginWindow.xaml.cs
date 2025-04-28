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
    public partial class LoginWindow : Window
    {
        ConnectionInstance dbConnection;
        public LoginWindow()
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
            if (usernameInput.Text == null || passwordInput == null || 
                usernameInput.Text.Length <= 0 || passwordInput.Password.Length <= 0)
            {
                SetMessage("One of the input fields is empty");
                return;
            }

            User? user = await User.TryLogin(usernameInput.Text, passwordInput.Password, dbConnection.Client);

            ClearInputFields();
            if (user == null)
            {
                SetMessage("User name or password is incorrect");
                return;
            }

            SetMessage("Succesful login");

        }

        private async void Create_new_User_Click(object sender, RoutedEventArgs e)
        {
            if (usernameInput.Text == null || passwordInput == null ||
                usernameInput.Text.Length <= 0 || passwordInput.Password.Length <= 0)
            {
                SetMessage("One of the input fields is empty");
                return;
            }

            ClearInputFields();
            if (!await User.CreateUser(usernameInput.Text, passwordInput.Password, dbConnection.Client))
            {
                SetMessage("User with this username already exists");
            }
            else
            {
                SetMessage("Succesfuly created a new account");
            }
        }

        private void SetMessage(string message)
        {
            messageText.Text = message;
        }

        private void ClearInputFields() 
        {
            usernameInput.Text = "";
            passwordInput.Password = "";
        }
    }
}