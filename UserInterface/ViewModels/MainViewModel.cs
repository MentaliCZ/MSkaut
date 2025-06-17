using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Windows;
using DatabaseManager;
using MSkaut;
using UserInterface.Commands;
using UserInterface.ViewModels.ModelRepresantations;
using UserManager;

namespace UserInterface.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public User User { get; set; }
        private ConnectionInstance dbConnection;

        public ObservableCollection<EventViewModel> Events { get; set; }

        public ObservableCollection<TransactionTypeViewModel> TransactionTypes { get; set; }
        private Dictionary<long, TransactionTypeViewModel> transactionTypesDict;

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
        public RelayCommand ExportEventCommand { get; set; }

        private EventViewModel _selectedExportEvent;
        public EventViewModel SelectedExportEvent
        {
            get => _selectedExportEvent;
            set
            {
                _selectedExportEvent = value;
                ExportEventCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand DeleteEventCommand { get; set; }
        public RelayCommand DeletePersonCommand { get; set; }
        public RelayCommand DeleteTransactionTypeCommand { get; set; }

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
            ExportEventCommand = new(ExportEvent, x => CanExportEvent);

            DeleteEventCommand = new(DeleteEvent, x => true);
            DeletePersonCommand = new(DeletePerson, x => true);
            DeleteTransactionTypeCommand = new(DeleteTransactionType, x => true);

            ShowEvents(this);
        }

        public static async Task<MainViewModel> CreateMainViewModel(
            Window thisWindow,
            User user,
            ConnectionInstance dbConnection
        )
        {
            MainViewModel mainViewModel = new(thisWindow, user, dbConnection);
            await mainViewModel.InitStructures();
            return mainViewModel;
        }

        public async Task InitStructures()
        {
            TransactionTypes = await TransactionTypeViewModel.GetUsersTransactionTypes(
                User,
                dbConnection.Client
            );

            transactionTypesDict = new();
            foreach (TransactionTypeViewModel transactionType in TransactionTypes)
            {
                transactionTypesDict[(long)transactionType.Id] = transactionType;
            }

            Genders = await Gender.GetAllGendersEN(dbConnection.Client);

            genderDict = new();
            foreach (Gender gender in Genders)
            {
                genderDict[gender.Id] = gender;
            }

            UsersPeople = await PersonViewModel.GetUsersPeople(
                User,
                genderDict,
                dbConnection.Client
            );
            Events = await EventViewModel.GetUserEvents(
                User,
                transactionTypesDict,
                genderDict,
                UsersPeople,
                TransactionTypes,
                dbConnection.Client
            );
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

            Events.Add(
                new EventViewModel(eventClass, UsersPeople, TransactionTypes, dbConnection.Client)
            );
        }

        private void ShowPeople(Object obj)
        {
            HideAllWindows();
            PeopleVisible = Visibility.Visible;
        }

        private void AddPerson(Object obj)
        {
            Person person = new(
                "Insert first name",
                "Insert last name",
                DateTime.Now,
                null,
                User.Id
            );

            UsersPeople.Add(new PersonViewModel(person, dbConnection.Client));
        }

        private void ShowTypes(Object obj)
        {
            HideAllWindows();
            TransactionTypesVisible = Visibility.Visible;
        }

        private void AddType(Object obj)
        {
            TransactionType type = new("Insert name", "...", User.Id);

            TransactionTypes.Add(new TransactionTypeViewModel(type, dbConnection.Client));
        }

        private void ShowExport(Object obj)
        {
            HideAllWindows();
            ExportVisible = Visibility.Visible;
        }

        private bool CanExportEvent => SelectedExportEvent != null;

        private async void ExportEvent(Object obj)
        {
            await EventExporter.Export(SelectedExportEvent);
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void DeleteEvent(Object obj)
        {
            EventViewModel eventViewModel = (EventViewModel)obj;

            if (eventViewModel != null && eventViewModel.CanDeleteRow())
            {
                eventViewModel.DeleteRow(null);
                Events.Remove(eventViewModel);
            }
        }

        private void DeletePerson(Object obj)
        {
            PersonViewModel personViewModel = (PersonViewModel)obj;

            if (personViewModel != null && personViewModel.CanDeleteRow())
            {
                personViewModel.DeleteRow(null);
                UsersPeople.Remove(personViewModel);
            }
        }

        private void DeleteTransactionType(Object obj)
        {
            TransactionTypeViewModel transactionType = (TransactionTypeViewModel)obj;

            if (transactionType != null && transactionType.CanDeleteRow())
            {
                transactionType.DeleteRow(null);
                TransactionTypes.Remove(transactionType);
            }
        }
    }
}
