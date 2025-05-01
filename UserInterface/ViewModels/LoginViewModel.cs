using System;
using UserManager;
using DatabaseManager;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using UserInterface.Commands;
using System.Windows;

namespace UserInterface.ViewModels
{
	public class LoginViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Login { get; set; }
		public string Password { get; set; }

		public RelayCommand TryLoginCommand { get; set; }
		public RelayCommand CreateUserCommand { get; set; }
		private MainWindow mainWindow;
		private Window window;

		private string _message;
		public string Message 
		{ get => _message;
			set
			{
				_message = value;
				OnPropertyChanged();
			}
		}

		private ConnectionInstance dbConnection;

		private LoginViewModel()
		{
            TryLoginCommand = new(TryLogin, _ => true);
            CreateUserCommand = new(CreateUser, _ => true);
        }

		public LoginViewModel(Window window) : this()
		{
			initDBConnection();
			this.window = window;
        }

        public LoginViewModel(Window window, ConnectionInstance dbConnection) : this()
        {
			this.dbConnection = dbConnection;
            this.window = window;
            TryLoginCommand = new(TryLogin, _ => true);
            CreateUserCommand = new(CreateUser, _ => true);
        }

        private async Task initDBConnection()
        {
            dbConnection = await ConnectionInstance.CreateInstance();
        }

        public async void TryLogin(Object obj)
		{
            if (Login == null || Password == null ||
				Login.Length <= 0 || Password.Length <= 0)
			{
				Message = "Login and password fields cant be empty";
				return;
			}

			User? user = await User.TryLogin(Login, Password, dbConnection.Client);

			if (user == null)
			{
				Message = "The username or password is incorrect";
				return;
			}

			mainWindow = await MainWindow.CreateMainWindow(user, dbConnection);
            mainWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
			mainWindow.Show();

			window.Close();
            return;
		}

		public async void CreateUser(Object obj)
		{
			if (Login == null || Password == null ||
				Login.Length <= 0 || Password.Length <= 0)
			{
				Message = "Login and password fields cant be empty";
				return;
			}

			if (!await User.CreateUser(Login, Password, dbConnection.Client))
			{
				Message = "Account with this username already exists";
				return;
			}

			Message = "Account was succesfuly created, you can log in now";
		}

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}