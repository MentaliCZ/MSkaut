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

namespace MSkaut
{
	public class EventClass
	{
		public int Id { get; private set; }
		public string Name { get; private set; }
		public string Description { get; private set; }
		public (DateOnly startDate, DateOnly endDate) Duration { get; private set; }
		public ObservableCollection<Transaction> Transactions { get; private set; }
		public ObservableCollection<Person> Participants { get; private set; }
		public User Owner { get; private set; }

		public EventClass(int id, string name, User owner)
		{
			this.Id = id;
			this.Name = name;
			this.Owner = owner;

			Transactions = new();
			Participants = new();
		}

		public EventClass(int id, string name, string description, User owner) : this(id, name, owner)
		{
			this.Description = description;
		}

		public static async Task<EventClass> InitEventClass(DBEvent dbEvent, Dictionary<int, TransactionType> transactionTypes, User user, Client client)
		{
			var eventClass = new EventClass(dbEvent.Id, dbEvent.Name, dbEvent.Description, user);

            List<Task> tasks = new();

			tasks.Add(eventClass.LoadParticipants(eventClass, dbEvent, client));
			tasks.Add(eventClass.LoadTransactions(eventClass, dbEvent, transactionTypes, client));

			await Task.WhenAll(tasks);

			return eventClass;
		}


        public static async Task<ObservableCollection<EventClass>> GetUserEvents (User user, Dictionary<int, TransactionType> transactionTypes, Client client) 
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

        private static async Task AddEvent(ObservableCollection<EventClass> events, DBEvent dbEvent, Dictionary<int, TransactionType> transactionTypes, User user, Client client)
		{
			EventClass eventClass = await EventClass.InitEventClass(dbEvent, transactionTypes, user, client);

			events.Add(eventClass);
		}


private async Task LoadParticipants(EventClass eventClass, DBEvent dbEvent, Client client)
		{
            eventClass.Participants = await Person.GetEventParticipants(dbEvent.Id, client);
        }

        private async Task LoadTransactions(EventClass eventClass, DBEvent dbEvent,
			Dictionary<int, TransactionType> transactionTypes, Client client)
        {
            eventClass.Transactions = await Transaction.GetEventTransactions(dbEvent.Id, transactionTypes, client);
        }

    }
}
