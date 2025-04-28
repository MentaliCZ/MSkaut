using System;
using DatabaseManager;
using UserManager;
using Supabase;

namespace MSkaut 
{
	public class EventManager
	{
		private List<EventClass> events;

		private EventManager()
		{
			events = new();
		}

		public static async Task<EventManager> InitEventManger(User user, Dictionary<int, TransactionType> transactionTypes,
			Client client) 
		{
			EventManager eventManager = new EventManager();
			await eventManager.Init(user, transactionTypes, client);
			return eventManager;
		}

		private async Task Init(User user, Dictionary<int, TransactionType> transactionTypes, Client client) 
		{
			List<DBEvent> dbEvents = await DBEvent.GetUserEvents(user.Id, client);

			foreach (DBEvent dbEvent in dbEvents)
			{
				events.Add(await EventClass.InitEventClass(dbEvent, transactionTypes, user, client));
			}

		}

		public bool DeleteEvent(string name, User owner)
		{
			foreach (EventClass eventClass in events)
			{
				if (eventClass.Owner == owner && eventClass.Name == name)
				{
					events.Remove(eventClass);
					return true;
				}
			}

			return false;
		}

	}
}
