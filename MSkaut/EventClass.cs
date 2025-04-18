using System;
using UserManager;

namespace MSkaut
{
	public class EventClass
	{
		public string Name { get; private set; }
		public string Description { get; private set; }
		public (Date startDate, Date endDate) Duration { get; set; }
		public List<Transaction> Transactions { get; set; }
		public List<Person> Participants { get; set; }
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

    }
}
