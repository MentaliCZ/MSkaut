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


namespace UserInterface.ViewModels
{
    public class EventViewModel : EditableClass
    {
        private EventClass eventClass;

        public long? Id { get => eventClass.Id; set => eventClass.Id = value; }
        public string Name { get => eventClass.Name; set => eventClass.Name = value; }
        public string Description { get => eventClass.Description; set => eventClass.Description = value; }
        public DateTime StartDate { get => eventClass.StartDate; set => eventClass.StartDate = value; }
        public DateTime EndDate { get => eventClass.EndDate; set => eventClass.EndDate = value; }

        public ObservableCollection<Transaction> Transactions { get; set; }
        public ObservableCollection<PersonViewModel> Participants { get; set;}
        public long CreatorId { get => eventClass.CreatorId; set => eventClass.CreatorId = value; }

        public RelayCommand OpenEditWindowCommand { get; set; }

        private EditEventWindow editWindow;

        public EventViewModel(EventClass eventClass, Client client) : base(client)
        {
            this.eventClass = eventClass;
            OpenEditWindowCommand = new(OpenEditWindow, _ => true);
        }

        public static async Task<EventViewModel> InitEventClass(DBEvent dbEvent, Dictionary<long, Gender> genderDict, 
            Dictionary<long, TransactionType> transactionTypes, User user, Client client)
        {
            var eventClass = new EventClass(dbEvent.Id, dbEvent.Name, dbEvent.Description,
                dbEvent.StartDate, dbEvent.EndDate, user.Id);
            EventViewModel eventViewModel = new(eventClass, client);

            List<Task> tasks = new();

            tasks.Add(eventViewModel.LoadParticipants(eventViewModel, genderDict, dbEvent, client));
            tasks.Add(eventViewModel.LoadTransactions(eventViewModel, dbEvent, transactionTypes, client));

            await Task.WhenAll(tasks);

            return eventViewModel;
        }


        public static async Task<ObservableCollection<EventViewModel>> GetUserEvents(User user, Dictionary<long, TransactionType> transactionTypes,
            Dictionary<long, Gender> genderDict, Client client)
        {
            List<DBEvent> dbEvents = await DBEvent.GetUserEvents(user.Id, client);
            ObservableCollection<EventViewModel> events = new();

            List<Task> tasks = new();

            foreach (DBEvent dbEvent in dbEvents)
            {
                tasks.Add(AddEvent(events, dbEvent, transactionTypes, genderDict, user, client));
            }

            await Task.WhenAll(tasks);

            return events;
        }

        private static async Task AddEvent(ObservableCollection<EventViewModel> events, DBEvent dbEvent,
            Dictionary<long, TransactionType> transactionTypes, Dictionary<long, Gender> genderDict,
            User user, Client client)
        {
            EventViewModel eventViewModel = await EventViewModel.InitEventClass(dbEvent, genderDict, transactionTypes, user, client);

            events.Add(eventViewModel);
        }


        private async Task LoadParticipants(EventViewModel eventViewModel, Dictionary<long, Gender> genderDict, DBEvent dbEvent, Client client)
        {
            eventViewModel.Participants = await PersonViewModel.GetEventParticipants(genderDict, dbEvent.Id, client);
        }

        private async Task LoadTransactions(EventViewModel eventViewModel, DBEvent dbEvent,
            Dictionary<long, TransactionType> transactionTypes, Client client)
        {
            eventViewModel.Transactions = await Transaction.GetEventTransactions(dbEvent.Id, transactionTypes, client);
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
            editWindow = new(client, this);
            editWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            editWindow.Show();
        }
    }
}
