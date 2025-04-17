using System;
using UserManager;

namespace MSkaut
{
	public class EventClass
	{
		public string Name { get; set; }
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

	}
}
