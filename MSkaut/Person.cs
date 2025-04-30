using System;
using UserManager;
using DatabaseManager;
using Supabase;
using System.Collections.ObjectModel;

namespace MSkaut
{
	public class Person
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateOnly BirthDate { get; set; }
		public Gender Gender { get; set; }

		public Person(string firstName, string lastName, DateOnly birthDate, Gender gender)
		{
			this.FirstName = firstName;
			this.LastName = lastName;
			this.BirthDate = birthDate;
			this.Gender = gender;
		}


		public static async Task DBPersonToPerson(ObservableCollection<Person> peopleList, DBPerson dbPerson, Client client)
		{
			peopleList.Add(new Person(dbPerson.FirstName, dbPerson.LastName,
				dbPerson.BirthDate, await Gender.GetGender(dbPerson.GenderId, client)));
		}


		public static async Task<ObservableCollection<Person>> GetUsersPeople(User user, Client client)
		{
			List<DBPerson> dbPeople = await DBPerson.GetUsersPeople(user.Id, client);
            ObservableCollection<Person> people = new();

			List<Task> tasks = new(); 

			foreach (DBPerson dbPerson in dbPeople)
			{
				tasks.Add(DBPersonToPerson(people, dbPerson, client));
			}

			await Task.WhenAll();

			return people;
		}

		public static async Task<ObservableCollection<Person>> GetEventParticipants(int eventId, Client client)
		{
			List<DBPerson> dbPeople = await DBEventPerson.GetEventParticipants(eventId, client);
            ObservableCollection<Person> participants = new();
			List<Task> tasks = new();

            foreach (DBPerson dbPerson in dbPeople)
            {
                tasks.Add(DBPersonToPerson(participants, dbPerson, client));
            }

			await Task.WhenAll(tasks);

			return participants;
        }

	}
}
