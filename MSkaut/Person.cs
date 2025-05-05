using System;
using UserManager;
using DatabaseManager;
using Supabase;
using System.Collections.ObjectModel;
using MSkaut.Commands;
using System.Xml.Linq;

namespace MSkaut
{
	public class Person : EditableClass
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateOnly BirthDate { get; set; }
		public Gender Gender { get; set; }
		public int CreatorId { get; set; }

		public Person(string firstName, string lastName, DateOnly birthDate, Gender gender, Client client) :base(client)
		{
			this.Id = null;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.BirthDate = birthDate;
            this.Gender = gender;
        }

		private Person(long id, string firstName, string lastName, DateOnly birthDate, Gender gender, Client client) 
			: this(firstName, lastName, birthDate, gender, client)
		{
			this.Id = id;
		}


		public static async Task DBPersonToPerson(ObservableCollection<Person> peopleList, DBPerson dbPerson, Client client)
		{
			peopleList.Add(new Person(dbPerson.Id, dbPerson.FirstName, dbPerson.LastName,
				dbPerson.BirthDate, await Gender.GetGender(dbPerson.GenderId, client), client));
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

		public static async Task<ObservableCollection<Person>> GetEventParticipants(long eventId, Client client)
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

		public override async void SaveRow(Object obj)
		{
			if (Id == null)
				Id = await DBPerson.CreatePerson(FirstName, LastName, BirthDate, Gender.Id, CreatorId, client);
			else
				await DBPerson.UpdatePerson((long)Id, FirstName, LastName, BirthDate, Gender.Id, CreatorId, client);
        }

    }
}
