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

        public MainViewModel(User user, ConnectionInstance dbConnection)
        {
            this.User = user;
            this.dbConnection = dbConnection;
        }


    }
}
