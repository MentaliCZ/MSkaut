using System;
using System.ComponentModel.DataAnnotations.Schema;
using Supabase;
using UserManager;
using DatabaseManager;
using XAct.Events;
using System.Collections.ObjectModel;
using MSkaut;
using static Microsoft.IO.RecyclableMemoryStreamManager;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using MSkaut.Commands;

namespace MSkaut
{
	public class EventClass : EditableClass
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }

		public ObservableCollection<Transaction> Transactions { get; set; }
		public ObservableCollection<Person> Participants { get; set; }
		public long CreatorId { get; set; }

        public EventClass(string name, string description, long creatorId, Client client) : base(client)
		{
			this.Id = null;
			this.Name = name;
			this.Description = description;
			this.CreatorId = creatorId;
			this.client = client;

			StartDate = DateTime.Now;
			EndDate = DateTime.Now;

            Transactions = new();
			Participants = new();
		}

        private EventClass(long id, string name, string description, DateOnly startDate, DateOnly endDate, long creatorId, Client client) : this(name, description, creatorId, client)
        {
            this.Id = id;
			StartDate = startDate.ToDateTime(TimeOnly.Parse("10:00 PM"));
			EndDate = endDate.ToDateTime(TimeOnly.Parse("10:00 PM"));
        }

        public static async Task<EventClass> InitEventClass(DBEvent dbEvent, Dictionary<long, Gender> genderDict, Dictionary<long, TransactionType> transactionTypes,
			User user, Client client)
		{
			var eventClass = new EventClass(dbEvent.Id, dbEvent.Name, dbEvent.Description,
				dbEvent.StartDate, dbEvent.EndDate, user.Id, client);

            List<Task> tasks = new();

			tasks.Add(eventClass.LoadParticipants(eventClass, genderDict, dbEvent, client));
			tasks.Add(eventClass.LoadTransactions(eventClass, dbEvent, transactionTypes, client));

			await Task.WhenAll(tasks);

			return eventClass;
		}


        public static async Task<ObservableCollection<EventClass>> GetUserEvents (User user, Dictionary<long, TransactionType> transactionTypes,
			Dictionary<long, Gender> genderDict, Client client) 
		{
			List<DBEvent> dbEvents = await DBEvent.GetUserEvents(user.Id, client);
			ObservableCollection<EventClass> events = new();

			List<Task> tasks = new();

			foreach (DBEvent dbEvent in dbEvents)
			{
				tasks.Add(AddEvent(events, dbEvent, transactionTypes, genderDict, user, client));
			}

			await Task.WhenAll(tasks);

			return events;
		}

        private static async Task AddEvent(ObservableCollection<EventClass> events, DBEvent dbEvent,
			Dictionary<long, TransactionType> transactionTypes, Dictionary<long, Gender> genderDict,
			User user, Client client)
		{
			EventClass eventClass = await EventClass.InitEventClass(dbEvent, genderDict, transactionTypes, user, client);

			events.Add(eventClass);
		}


		private async Task LoadParticipants(EventClass eventClass, Dictionary<long, Gender> genderDict, DBEvent dbEvent, Client client)
		{
            eventClass.Participants = await Person.GetEventParticipants(genderDict, dbEvent.Id, client);
        }

        private async Task LoadTransactions(EventClass eventClass, DBEvent dbEvent,
			Dictionary<long, TransactionType> transactionTypes, Client client)
        {
            eventClass.Transactions = await Transaction.GetEventTransactions(dbEvent.Id, transactionTypes, client);
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
    }
}
