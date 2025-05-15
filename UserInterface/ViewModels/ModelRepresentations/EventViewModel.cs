using System;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Windows;
using System.Xml.Linq;
using DatabaseManager;
using MSkaut;
using Supabase;
using UserInterface.Commands;
using UserManager;

namespace UserInterface.ViewModels.ModelRepresantations
{
    public class EventViewModel : EditableClass
    {
        private EventClass eventClass;

        public long? Id
        {
            get => eventClass.Id;
            set
            {
                eventClass.Id = value;
                OpenEditWindowCommand.RaiseCanExecuteChanged();
                OnPropertyChanged();
            }
        }
        public string Name
        {
            get => eventClass.Name;
            set
            {
                eventClass.Name = value;
                IsChanged = true;
                SaveRowCommand.RaiseCanExecuteChanged();
                OnPropertyChanged();
            }
        }
        public string Description
        {
            get => eventClass.Description;
            set
            {
                eventClass.Description = value;
                IsChanged = true;
                SaveRowCommand.RaiseCanExecuteChanged();
                OnPropertyChanged();
            }
        }
        public DateTime StartDate
        {
            get => eventClass.StartDate;
            set
            {
                eventClass.StartDate = value;
                IsChanged = true;
                SaveRowCommand.RaiseCanExecuteChanged();
                OnPropertyChanged();
            }
        }
        public DateTime EndDate
        {
            get => eventClass.EndDate;
            set
            {
                eventClass.EndDate = value;
                IsChanged = true;
                SaveRowCommand.RaiseCanExecuteChanged();
                OnPropertyChanged();
            }
        }

        public ObservableCollection<TransactionViewModel> Transactions { get; set; }
        public ObservableCollection<PersonViewModel> Participants { get; set; }
        public long CreatorId
        {
            get => eventClass.CreatorId;
            set => eventClass.CreatorId = value;
        }

        public RelayCommand OpenEditWindowCommand { get; set; }
        private ObservableCollection<PersonViewModel> usersPeople;
        private ObservableCollection<TransactionTypeViewModel> transactionTypes;

        private EditEventWindow editWindow;

        public EventViewModel(
            EventClass eventClass,
            ObservableCollection<PersonViewModel> usersPeople,
            ObservableCollection<TransactionTypeViewModel> transactionTypes,
            Client client
        )
            : base(client)
        {
            this.eventClass = eventClass;
            this.usersPeople = usersPeople;
            this.transactionTypes = transactionTypes;

            this.Participants = new();
            this.Transactions = new();

            OpenEditWindowCommand = new(OpenEditWindow, x => CanOpenEditWindow);
        }

        public static async Task<EventViewModel> InitEventClass(
            DBEvent dbEvent,
            Dictionary<long, Gender> genderDict,
            Dictionary<long, TransactionTypeViewModel> transactionTypesDict,
            User user,
            ObservableCollection<PersonViewModel> usersPeople,
            ObservableCollection<TransactionTypeViewModel> transactionTypes,
            Client client
        )
        {
            var eventClass = new EventClass(
                dbEvent.Id,
                dbEvent.Name,
                dbEvent.Description,
                dbEvent.StartDate,
                dbEvent.EndDate,
                user.Id
            );
            EventViewModel eventViewModel = new(eventClass, usersPeople, transactionTypes, client);

            List<Task> tasks = new();

            tasks.Add(eventViewModel.LoadParticipants(eventViewModel, genderDict, dbEvent, client));
            tasks.Add(
                eventViewModel.LoadTransactions(
                    eventViewModel,
                    dbEvent,
                    transactionTypesDict,
                    client
                )
            );

            await Task.WhenAll(tasks);

            return eventViewModel;
        }

        public static async Task<ObservableCollection<EventViewModel>> GetUserEvents(
            User user,
            Dictionary<long, TransactionTypeViewModel> transactionTypesDict,
            Dictionary<long, Gender> genderDict,
            ObservableCollection<PersonViewModel> usersPeople,
            ObservableCollection<TransactionTypeViewModel> transactionTypes,
            Client client
        )
        {
            List<DBEvent> dbEvents = await DBEvent.GetUserEvents(user.Id, client);
            ObservableCollection<EventViewModel> events = new();

            foreach (DBEvent dbEvent in dbEvents)
            {
                events.Add(
                    await EventViewModel.InitEventClass(
                        dbEvent,
                        genderDict,
                        transactionTypesDict,
                        user,
                        usersPeople,
                        transactionTypes,
                        client
                    )
                );
            }

            return events;
        }

        private async Task LoadParticipants(
            EventViewModel eventViewModel,
            Dictionary<long, Gender> genderDict,
            DBEvent dbEvent,
            Client client
        )
        {
            eventViewModel.Participants = await PersonViewModel.GetEventParticipants(
                genderDict,
                dbEvent.Id,
                client
            );
        }

        private async Task LoadTransactions(
            EventViewModel eventViewModel,
            DBEvent dbEvent,
            Dictionary<long, TransactionTypeViewModel> transactionTypes,
            Client client
        )
        {
            eventViewModel.Transactions = await TransactionViewModel.GetEventTransactions(
                dbEvent.Id,
                transactionTypes,
                client
            );
        }

        public override async void SaveRow(object obj)
        {
            bool success;
            SaveRowCommand.RaiseCanExecuteChanged();
            IsProcessing = true;

            if (Id == null)
            {
                Id = await DBEvent.CreateEvent(
                    Name,
                    Description,
                    DateOnly.FromDateTime(StartDate),
                    DateOnly.FromDateTime(EndDate),
                    CreatorId,
                    client
                );
                success = Id >= 0;
            }
            else
            {
                success = await DBEvent.UpdateEvent(
                    (long)Id,
                    Name,
                    Description,
                    DateOnly.FromDateTime(StartDate),
                    DateOnly.FromDateTime(EndDate),
                    CreatorId,
                    client
                );
            }

            IsProcessing = false;

            if (success)
                IsChanged = false;
        }

        public override bool CanSaveRow()
        {
            return Name != null
                && Name.Length > 0
                && Description != null
                && StartDate <= EndDate
                && IsChanged
                && Name.Length <= 30;
        }

        public override async void DeleteRow(object obj)
        {
            if (Id == null)
                return;

            var result = MessageBox.Show(
                "Are you sure you want to delete this event?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            IsProcessing = true;

            if (result == MessageBoxResult.Yes)
                await DBEvent.DeleteEvent((long)Id, client);

            IsProcessing = false;
        }

        public override bool CanDeleteRow()
        {
            return true;
        }

        public void OpenEditWindow(object obj)
        {
            editWindow = new(client, this, usersPeople, transactionTypes);
            editWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            editWindow.Show();
        }

        public bool CanOpenEditWindow => Id != null;

        public override string ToString()
        {
            return eventClass.ToString();
        }
    }
}
