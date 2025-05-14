using System;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using DatabaseManager;
using Supabase;
using UserManager;

namespace MSkaut
{
    public class Person
    {
        public long? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public long CreatorId { get; set; }

        public Person(
            string firstName,
            string lastName,
            DateTime birthDate,
            Gender gender,
            long creatorId
        )
        {
            this.Id = null;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.BirthDate = birthDate;
            this.Gender = gender;
            this.CreatorId = creatorId;
        }

        public Person(
            long id,
            string firstName,
            string lastName,
            DateOnly birthDate,
            Gender gender,
            long creatorId
        )
            : this(
                firstName,
                lastName,
                birthDate.ToDateTime(TimeOnly.Parse("10:00 PM")),
                gender,
                creatorId
            )
        {
            this.Id = id;
        }
    }
}
