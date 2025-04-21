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

		public EventClass(string name, User owner)
		{
			this.Name = name;
			this.Owner = owner;
			Transactions = new();
			Participants = new();
		}

        public EventClass(string name, string description, User owner) : this(name, owner)
        {
			this.Description = description;
        }

		public static EventClass InitEventClass(DBEvent dbEvent, Client client)
		{



		}

    }
}
