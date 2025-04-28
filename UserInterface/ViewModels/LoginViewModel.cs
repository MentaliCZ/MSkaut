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

		public LoginViewModel()
		{
			initDBConnection();
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

            mainWindow = new(user, dbConnection);
            mainWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
			mainWindow.Show();
			
            return;
		}

		public async void CreateUser(Object obj)
		{
			Message = "we failed men";
		}

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}