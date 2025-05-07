using DatabaseManager;
using MSkaut;
using System;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using Supabase;
using UserManager;
using System.Data.Common;
using System.Windows;
using UserInterface.Commands;


namespace UserInterface.ViewModels.ModelRepresantations
{
    public class EventViewModel : EditableClass
    {
        private EventClass eventClass;

        public long? Id { get => eventClass.Id; set => eventClass.Id = value; }
        public string Name { get => eventClass.Name; set { eventClass.Name = value; SaveRowCommand.RaiseCanExecuteChanged(); } }
        public string Description { get => eventClass.Description; set { eventClass.Description = value; SaveRowCommand.RaiseCanExecuteChanged(); } }
        public DateTime StartDate { get => eventClass.StartDate; set { eventClass.StartDate = value; SaveRowCommand.RaiseCanExecuteChanged(); } }
        public DateTime EndDate { get => eventClass.EndDate; set { eventClass.EndDate = value; SaveRowCommand.RaiseCanExecuteChanged(); } }

        public ObservableCollection<TransactionViewModel> Transactions { get; set; }
        public ObservableCollection<PersonViewModel> Participants { get; set;}
        public long CreatorId { get => eventClass.CreatorId; set => eventClass.CreatorId = value; }

        public RelayCommand OpenEditWindowCommand { get; set; }
        private ObservableCollection<PersonViewModel> usersPeople;
        private ObservableCollection<TransactionTypeViewModel> transactionTypes;

        private EditEventWindow editWindow;

        public EventViewModel(EventClass eventClass, ObservableCollection<PersonViewModel> usersPeople, 
            ObservableCollection<TransactionTypeViewModel> transactionTypes, Client client) : base(client)
        {
            this.eventClass = eventClass;
            this.usersPeople = usersPeople;
            this.transactionTypes = transactionTypes;
            OpenEditWindowCommand = new(OpenEditWindow, _ => true);
        }

        public static async Task<EventViewModel> InitEventClass(DBEvent dbEvent, Dictionary<long, Gender> genderDict, 
            Dictionary<long, TransactionType> transactionTypesDict, User user,
            ObservableCollection<PersonViewModel> usersPeople, ObservableCollection<TransactionTypeViewModel> transactionTypes,
            Client client)
        {
            var eventClass = new EventClass(dbEvent.Id, dbEvent.Name, dbEvent.Description,
                dbEvent.StartDate, dbEvent.EndDate, user.Id);
            EventViewModel eventViewModel = new(eventClass, usersPeople, transactionTypes, client);

            List<Task> tasks = new();

            tasks.Add(eventViewModel.LoadParticipants(eventViewModel, genderDict, dbEvent, client));
            tasks.Add(eventViewModel.LoadTransactions(eventViewModel, dbEvent, transactionTypesDict, client));

            await Task.WhenAll(tasks);

            return eventViewModel;
        }


        public static async Task<ObservableCollection<EventViewModel>> GetUserEvents(User user, Dictionary<long, TransactionType> transactionTypesDict,
            Dictionary<long, Gender> genderDict, ObservableCollection<PersonViewModel> usersPeople, ObservableCollection<TransactionTypeViewModel> transactionTypes,
            Client client)
        {
            List<DBEvent> dbEvents = await DBEvent.GetUserEvents(user.Id, client);
            ObservableCollection<EventViewModel> events = new();

            foreach (DBEvent dbEvent in dbEvents)
            {
                events.Add(await EventViewModel.InitEventClass(dbEvent, genderDict, transactionTypesDict, 
                    user, usersPeople, transactionTypes, client));
            }

            return events;
        }


        private async Task LoadParticipants(EventViewModel eventViewModel, Dictionary<long, Gender> genderDict, DBEvent dbEvent, Client client)
        {
            eventViewModel.Participants = await PersonViewModel.GetEventParticipants(genderDict, dbEvent.Id, client);
        }

        private async Task LoadTransactions(EventViewModel eventViewModel, DBEvent dbEvent,
            Dictionary<long, TransactionType> transactionTypes, Client client)
        {
            eventViewModel.Transactions = await TransactionViewModel.GetEventTransactions(dbEvent.Id, transactionTypes, client);
        }


        public override async void SaveRow(object obj)
        {
            if (Id == null)
                Id = await DBEvent.CreateEvent(Name, Description, DateOnly.FromDateTime(StartDate), DateOnly.FromDateTime(EndDate), CreatorId, client);
            else
                await DBEvent.UpdateEvent((long)Id, Name, Description, DateOnly.FromDateTime(StartDate), DateOnly.FromDateTime(EndDate), CreatorId, client);
        }

        public override async void DeleteRow(object obj)
        {
            throw new NotImplementedException();
        }

        public void OpenEditWindow(object obj)
        {
            editWindow = new(client, this, usersPeople, transactionTypes);
            editWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            editWindow.Show();
        }

        public override bool CanSaveRow()
        {
            return Name != null && Name.Length > 0 && Description != null && StartDate <= EndDate;
        }

        public override bool CanDeleteRow()
        {
            return true;
        }
    }
}
