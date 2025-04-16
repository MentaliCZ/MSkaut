using System;

namespace MSkaut
{
	public class EventClass
	{
		public string Name { get; set; }
		public (Date startDate, Date endDate) Duration { get; set; }
		public List<Transaction> Transactions { get; set; }
		public List<Person> Participants { get; set; }

		public EventClass(string name)
		{
			Name = name;
			Transactions = new();
			Participants = new();
		}

	}
}
