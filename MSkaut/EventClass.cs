using System;
using System.ComponentModel.DataAnnotations.Schema;
using Supabase;
using UserManager;
using DatabaseManager;

namespace MSkaut
{
	public class EventClass
	{
		public int Id { get; private set; }
		public string Name { get; private set; }
		public string Description { get; private set; }
		public (DateOnly startDate, DateOnly endDate) Duration { get; private set; }
		public List<Transaction> Transactions { get; private set; }
		public List<Person> Participants { get; private set; }
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

			eventClass.Participants = await Person.GetEventParticipants(dbEvent.Id, client);
			eventClass.Transactions = await Transaction.GetEventTransactions(dbEvent.Id, transactionTypes, client);

			return eventClass;
		}

    }
}
