using System;

namespace MSkaut
{
	public class EventClass
	{
		public string Name { get; set; }
		private (Date startDate, Date endDate) duration;
		public List<Transaction> Transactions { get; set; }
		public List<Person> Participants { get; set; }

		public EventClass(string name)
		{
			this.Name = name;
			Transactions = new();
			Participants = new();
		}

	}
}
