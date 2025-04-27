using System;
using UserManager;
using DatabaseManager;
using Supabase;

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


		public static async Task<Person> DBPersonToPerson(DBPerson dbPerson, Client client)
		{
			return new Person(dbPerson.FirstName, dbPerson.LastName,
				dbPerson.BirthDate, await Gender.GetGender(dbPerson.GenderId, client));
		}


		public static async Task<List<Person>> GetUsersPeople(User user, Client client)
		{
			List<DBPerson> dbPeople = await DBPerson.GetUsersPeople(user.Id, client);
			List<Person> people = new();

			foreach (DBPerson dbPerson in dbPeople)
			{
				people.Add(new Person(dbPerson.FirstName, dbPerson.LastName, dbPerson.BirthDate, await Gender.GetGender(dbPerson.GenderId, client)));
			}

			return people;
		}

		public static async Task<List<Person>> GetEventParticipants(int eventId, Client client)
		{
			List<Person> participants = new();

            foreach (DBPerson dbPerson in (await DBEventPerson.GetEventParticipants(eventId, client)))
            {
                participants.Add(await Person.DBPersonToPerson(dbPerson, client));
            }

			return participants;
        }

	}
}
