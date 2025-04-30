using System;
using DatabaseManager;
using System.Data.Common;

using MSkaut;
using UserManager;
using DatabaseManager;
using UserInterface.Commands;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UserInterface.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public User User { get; set; }
        private ConnectionInstance dbConnection;

        public RelayCommand ShowEventsPage { get; set; }
        public RelayCommand ShowPeoplePage { get; set; }
        public RelayCommand ShowCategoriesPage { get; set; }
        public RelayCommand ShowExportPage { get; set; }

        private Visibility _eventsVisible;
        public Visibility EventsVisible 
        { 
            get => _eventsVisible; 

            set
            {
                _eventsVisible = value;
                OnPropertyChanged();
            }
        }

        private Visibility _peopleVisible;
        public Visibility PeopleVisible
        {
            get => _peopleVisible;

            set
            {
                _peopleVisible = value;
                OnPropertyChanged();
            }
        }

        private Visibility _categoriesVisible;
        public Visibility CategoriesVisible
        {
            get => _categoriesVisible;

            set
            {
                _categoriesVisible = value;
                OnPropertyChanged();
            }
        }

        private Visibility _exportVisible;
        public Visibility ExportVisible
        {
            get => _exportVisible;

            set
            {
                _exportVisible = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand LogOutCommand { get; set; }
        private Window thisWindow;

        public MainViewModel(Window thisWindow, User user, ConnectionInstance dbConnection)
        {
            this.User = user;
            this.dbConnection = dbConnection;
            this.thisWindow = thisWindow;

            LogOutCommand = new(LogOut, _ => true);
            ShowEventsPage = new(ShowEvents, _ => true);
            ShowPeoplePage = new(ShowPeople, _ => true);
            ShowCategoriesPage = new(ShowCategories, _ => true);
            ShowExportPage = new(ShowExport, _ => true);
        }

        private void LogOut(Object obj)
        {
            LoginWindow loginWindow = new(dbConnection);
            loginWindow.Show();
            thisWindow.Close();
        }

        private void HideAllWindows()
        {
            EventsVisible = Visibility.Hidden;
            PeopleVisible = Visibility.Hidden;
            CategoriesVisible = Visibility.Hidden;
            ExportVisible = Visibility.Hidden;
        }

        private void ShowEvents(Object obj)
        {
            HideAllWindows();
            EventsVisible = Visibility.Visible;
        }

        private void ShowPeople(Object obj)
        {
            HideAllWindows();
            PeopleVisible = Visibility.Visible;
        }

        private void ShowCategories(Object obj)
        {
            HideAllWindows();
            CategoriesVisible = Visibility.Visible;
        }

        private void ShowExport(Object obj)
        {
            HideAllWindows();
            ExportVisible = Visibility.Visible;
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
