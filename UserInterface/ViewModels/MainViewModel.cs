using System;
using DatabaseManager;
using System.Data.Common;

using MSkaut;
using UserManager;
using UserInterface.Commands;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using UserInterface.ViewModels.ModelRepresantations;

namespace UserInterface.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public User User { get; set; }
        private ConnectionInstance dbConnection;

        public ObservableCollection<EventViewModel> Events { get; set; }

        public ObservableCollection<TransactionTypeViewModel> TransactionTypes { get; set; }
        private Dictionary<long, TransactionType> transactionTypesDict;

        public ObservableCollection<Gender> Genders { get; set; }
        private Dictionary<long, Gender> genderDict;

        public ObservableCollection<PersonViewModel> UsersPeople { get; set; }

        public RelayCommand ShowEventsPage { get; set; }
        public RelayCommand AddEventCommand { get; set; }

        public RelayCommand ShowPeoplePage { get; set; }
        public RelayCommand AddPersonCommand { get; set; }

        public RelayCommand ShowTypesPage { get; set; }
        public RelayCommand AddTypeCommand { get; set; }

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
            AddEventCommand = new(AddEvent, _ => true);

            ShowPeoplePage = new(ShowPeople, _ => true);
            AddPersonCommand = new(AddPerson, _ => true);

            ShowTypesPage = new(ShowTypes, _ => true);
            AddTypeCommand = new(AddType, _ => true);

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
            TransactionTypes = await TransactionTypeViewModel.GetUsersTransactionTypes(User, dbConnection.Client);

            transactionTypesDict = new();
            foreach (TransactionTypeViewModel transactionType in TransactionTypes)
            {
                transactionTypesDict[(long)transactionType.Id] = transactionType.getTransactionType();
            }

            Genders = await Gender.GetAllGendersEN(dbConnection.Client);

            genderDict = new();
            foreach (Gender gender in Genders)
            {
                genderDict[gender.Id] = gender;
            }

            UsersPeople = await PersonViewModel.GetUsersPeople(User, genderDict, dbConnection.Client);
            Events = await EventViewModel.GetUserEvents(User, transactionTypesDict, genderDict, UsersPeople, dbConnection.Client);
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

        private void AddEvent(Object obj)
        {
            EventClass eventClass = new("Insert event name", "...", User.Id);

            Events.Add(new EventViewModel(eventClass, UsersPeople, dbConnection.Client));
        }

        private void ShowPeople(Object obj)
        {
            HideAllWindows();
            PeopleVisible = Visibility.Visible;
        }

        private void AddPerson(Object obj)
        {
            Person person = new Person("Insert first name", "Insert last name", DateTime.Now, null, User.Id);
            
            UsersPeople.Add(new PersonViewModel(person, dbConnection.Client));
        }

        private void ShowTypes(Object obj)
        {
            HideAllWindows();
            TransactionTypesVisible = Visibility.Visible;
        }

        private void AddType(Object obj)
        {
            //TODO: DO SOMETHING
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
