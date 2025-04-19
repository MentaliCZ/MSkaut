using System;
using UserManager;

namespace MSkaut 
{
	public class EventManager
	{
		private HashSet<EventClass> events;

		public EventManager()
		{
			events = new();
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

		public void AddEvent(EventClass event)
		{
			events.add(event);
		}
	}
}
