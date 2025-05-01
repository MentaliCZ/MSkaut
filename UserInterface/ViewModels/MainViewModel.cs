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
using System.Collections.ObjectModel;

namespace UserInterface.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public User User { get; set; }
        private ConnectionInstance dbConnection;

        private ObservableCollection<EventClass> _events;
        public ObservableCollection<EventClass> Events
        {
            get => _events;

            set
            {
                _events = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<TransactionType> _transactionTypes;
        public ObservableCollection<TransactionType> TransactionTypes
        {
            get => _transactionTypes;

            set
            {
                _transactionTypes = value;
                OnPropertyChanged();
            }
        }
        private Dictionary<long, TransactionType> transactionTypesDict;

        private ObservableCollection<Person> _usersPeople;
        public ObservableCollection<Person> UsersPeople 
        { get => _usersPeople;

          set
          {
                _usersPeople = value;
                OnPropertyChanged();
          } 
        }

        public RelayCommand ShowEventsPage { get; set; }
        public RelayCommand ShowPeoplePage { get; set; }
        public RelayCommand ShowTypesPage { get; set; }
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

        private Visibility _transactionTypesVisible;
        public Visibility TransactionTypesVisible
        {
            get => _transactionTypesVisible;

            set
            {
                _transactionTypesVisible = value;
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

        private MainViewModel(Window thisWindow, User user, ConnectionInstance dbConnection)
        {
            this.User = user;
            this.dbConnection = dbConnection;
            this.thisWindow = thisWindow;

            LogOutCommand = new(LogOut, _ => true);
            ShowEventsPage = new(ShowEvents, _ => true);
            ShowPeoplePage = new(ShowPeople, _ => true);
            ShowTypesPage = new(ShowTypes, _ => true);
            ShowExportPage = new(ShowExport, _ => true);

            ShowEvents(this);
        }

        public static async Task<MainViewModel> CreateMainViewModel(Window thisWindow,
            User user, ConnectionInstance dbConnection)
        {
            MainViewModel mainViewModel = new(thisWindow, user, dbConnection);
            await mainViewModel.InitStructures();
            return mainViewModel;
        }

        public async Task InitStructures()
        {
            TransactionTypes = await TransactionType.GetUsersTransactionTypes(User, dbConnection.Client);

            transactionTypesDict = new();
            foreach (TransactionType transactionType in TransactionTypes)
            {
                transactionTypesDict[transactionType.Id] = transactionType;
            }

            Events = await EventClass.GetUserEvents(User, transactionTypesDict, dbConnection.Client);
            UsersPeople = await Person.GetUsersPeople(User, dbConnection.Client);
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
            TransactionTypesVisible = Visibility.Hidden;
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

        private void ShowTypes(Object obj)
        {
            HideAllWindows();
            TransactionTypesVisible = Visibility.Visible;
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
