using System;
using UserManager;

namespace MSkaut {
	public class EventManager
	{
		private HashSet<EventClass> events;

		public EventManager()
		{
			events = new();
		}

		public void addEvent(EventClass event)
		{
			events.add(event);
		}

		public bool deleteEvent(string name, User owner) 
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
