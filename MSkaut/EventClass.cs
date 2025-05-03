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
	public class EventClass
	{
		public long Id { get; private set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public (DateOnly startDate, DateOnly endDate) Duration { get; set; }
		public ObservableCollection<Transaction> Transactions { get; set; }
		public ObservableCollection<Person> Participants { get; set; }
		public User Owner { get; set; }

        public RelayCommand SaveRowCommand { get; }
		private Client client;

        public EventClass(long id, string name, string description, User owner, Client client)
		{
			this.Id = id;
			this.Name = name;
			this.Description = description;
			this.Owner = owner;
			this.client = client;

            SaveRowCommand = new(SaveRow, _ => true);

            Transactions = new();
			Participants = new();
		}

		public static async Task<EventClass> InitEventClass(DBEvent dbEvent, Dictionary<long, TransactionType> transactionTypes,
			User user, Client client)
		{
			var eventClass = new EventClass(dbEvent.Id, dbEvent.Name, dbEvent.Description, user, client);

            List<Task> tasks = new();

			tasks.Add(eventClass.LoadParticipants(eventClass, dbEvent, client));
			tasks.Add(eventClass.LoadTransactions(eventClass, dbEvent, transactionTypes, client));

			await Task.WhenAll(tasks);

			return eventClass;
		}


        public static async Task<ObservableCollection<EventClass>> GetUserEvents (User user, Dictionary<long, TransactionType> transactionTypes, Client client) 
		{
			List<DBEvent> dbEvents = await DBEvent.GetUserEvents(user.Id, client);
			ObservableCollection<EventClass> events = new();

			List<Task> tasks = new();

			foreach (DBEvent dbEvent in dbEvents)
			{
				tasks.Add(AddEvent(events, dbEvent, transactionTypes, user, client));
			}

			await Task.WhenAll(tasks);

			return events;
		}

        private static async Task AddEvent(ObservableCollection<EventClass> events, DBEvent dbEvent,
			Dictionary<long, TransactionType> transactionTypes, User user, Client client)
		{
			EventClass eventClass = await EventClass.InitEventClass(dbEvent, transactionTypes, user, client);

			events.Add(eventClass);
		}


		private async Task LoadParticipants(EventClass eventClass, DBEvent dbEvent, Client client)
		{
            eventClass.Participants = await Person.GetEventParticipants(dbEvent.Id, client);
        }

        private async Task LoadTransactions(EventClass eventClass, DBEvent dbEvent,
			Dictionary<long, TransactionType> transactionTypes, Client client)
        {
            eventClass.Transactions = await Transaction.GetEventTransactions(dbEvent.Id, transactionTypes, client);
        }


        public async void SaveRow(Object obj)
		{
			await DBEvent.UpdateEvent(Id, Name, Description, Owner.Id, client);
		}
    }
}
