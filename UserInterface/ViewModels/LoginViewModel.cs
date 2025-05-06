using System;
using UserManager;
using DatabaseManager;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using UserInterface.Commands;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using UserInterface.ViewModels.ModelRepresantations;

namespace UserInterface.ViewModels
{
	public class LoginViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

		private string _login;
        public string Login { get => _login; set { _login = value; TryLoginCommand.RaiseCanExecuteChanged(); CreateUserCommand.RaiseCanExecuteChanged(); } }

		private string _password;
		public string Password { get => _password; set { _password = value; TryLoginCommand.RaiseCanExecuteChanged(); CreateUserCommand.RaiseCanExecuteChanged(); } }

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
            TryLoginCommand = new(TryLogin, CanTryLogin);
            CreateUserCommand = new(CreateUser, CanTryLogin);
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
            TryLoginCommand = new(TryLogin, CanTryLogin);
            CreateUserCommand = new(CreateUser, CanTryLogin);
        }

        private async Task initDBConnection()
        {
            dbConnection = await ConnectionInstance.CreateInstance();
        }

        public async void TryLogin(Object obj)
		{
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

		private bool CanTryLogin(object? obj) =>
			Login is not null && Password is not null;


    }
}