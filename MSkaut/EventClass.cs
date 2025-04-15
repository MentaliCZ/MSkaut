using System;

namespace MSkaut
{
	public class EventClass
	{
		private (Date startDate, Date endDate) duration;
		private List<Transaction> transactions;

		public EventClass()
		{
			transactions = new();
		}
	}
}
