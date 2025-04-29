using System;
using DatabaseManager;
using System.Data.Common;

using MSkaut;
using UserManager;
using DatabaseManager;
using UserInterface.Commands;
using System.Windows;

namespace UserInterface.ViewModels
{
    public class MainViewModel
    {
        public User User { get; set; }
        private ConnectionInstance dbConnection;

        public RelayCommand LogOutCommand { get; set; }
        private Window thisWindow;

        public MainViewModel(Window thisWindow, User user, ConnectionInstance dbConnection)
        {
            this.User = user;
            this.dbConnection = dbConnection;
            this.thisWindow = thisWindow;
            LogOutCommand = new(LogOut, _ => true);
        }

        private void LogOut(Object obj)
        {
            LoginWindow loginWindow = new(dbConnection);
            loginWindow.Show();
            thisWindow.Close();
        }


    }
}
