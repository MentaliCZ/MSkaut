using System.Collections.ObjectModel;
using DatabaseManager.Person;
using DatabaseManager.EventPerson;
using MSkaut;
using Supabase;
using UserManager;

namespace UserInterface.ViewModels.ModelRepresantations
{
    public class PersonViewModel : EditableClass
    {
        private Person person;

        public long? Id
        {
            get => person.Id;
            set => person.Id = value;
        }

        public string FirstName
        {
            get => person.FirstName;
            set
            {
                person.FirstName = value;
                IsChanged = true;
                SaveRowCommand.RaiseCanExecuteChanged();
            }
        }
        public string LastName
        {
            get => person.LastName;
            set
            {
                person.LastName = value;
                IsChanged = true;
                SaveRowCommand.RaiseCanExecuteChanged();
            }
        }

        public DateTime BirthDate
        {
            get => person.BirthDate;
            set
            {
                person.BirthDate = value;
                IsChanged = true;
                SaveRowCommand.RaiseCanExecuteChanged();
            }
        }
        public Gender Gender
        {
            get => person.Gender;
            set
            {
                person.Gender = value;
                IsChanged = true;
                SaveRowCommand.RaiseCanExecuteChanged();
            }
        }

        public long CreatorId
        {
            get => person.CreatorId;
            set => person.CreatorId = value;
        }

        public PersonViewModel(Person person, Client client)
            : base(client)
        {
            this.person = person;
        }

        public static void DBPersonToPerson(
            ObservableCollection<PersonViewModel> peopleList,
            Dictionary<long, Gender> genderDict,
            PersonEntity dbPerson,
            Client client
        )
        {
            Person person = new Person(
                dbPerson.Id,
                dbPerson.FirstName,
                dbPerson.LastName,
                dbPerson.BirthDate,
                genderDict[dbPerson.GenderId],
                dbPerson.CreatorId
            );

            peopleList.Add(new PersonViewModel(person, client));
        }

        public static async Task<ObservableCollection<PersonViewModel>> GetUsersPeople(
            User user,
            Dictionary<long, Gender> genderDict,
            Client client
        )
        {
            List<PersonEntity> dbPeople = await PersonFunc.GetUsersPeople(user.Id, client);
            ObservableCollection<PersonViewModel> people = new();

            foreach (PersonEntity dbPerson in dbPeople)
            {
                DBPersonToPerson(people, genderDict, dbPerson, client);
            }

            return people;
        }

        public static async Task<ObservableCollection<PersonViewModel>> GetEventParticipants(
            Dictionary<long, Gender> genderDict,
            long eventId,
            Client client
        )
        {
            List<PersonEntity> dbPeople = await EventPersonFunc.GetEventParticipants(eventId, client);
            ObservableCollection<PersonViewModel> participants = new();

            foreach (PersonEntity dbPerson in dbPeople)
            {
                DBPersonToPerson(participants, genderDict, dbPerson, client);
            }

            return participants;
        }

        public override async void SaveRow(Object obj)
        {
            IsChanged = false;
            IsProcessing = true;
            SaveRowCommand.RaiseCanExecuteChanged();

            if (Id == null)
                Id = await PersonFunc.CreatePerson(
                    FirstName,
                    LastName,
                    DateOnly.FromDateTime(BirthDate),
                    Gender.Id,
                    CreatorId,
                    client
                );
            else
                await PersonFunc.UpdatePerson(
                    (long)Id,
                    FirstName,
                    LastName,
                    DateOnly.FromDateTime(BirthDate),
                    Gender.Id,
                    CreatorId,
                    client
                );

            IsProcessing = false;
        }

        public override async void DeleteRow(object obj)
        {
            if (Id == null)
                return;
            
            IsProcessing = true;

            await PersonFunc.DeletePerson((long)Id, client);

            IsProcessing = false;
        }

        public override string ToString()
        {
            return FirstName + " " + LastName;
        }

        public override bool CanSaveRow()
        {
            return FirstName != null
                && FirstName.Length > 0
                && LastName != null
                && LastName.Length > 0
                && Gender != null
                && IsChanged
                && FirstName.Length <= 100
                && LastName.Length <= 100
                && !IsProcessing;
        }

        public override bool CanDeleteRow()
        {
            return !IsProcessing;
        }
    }
}
