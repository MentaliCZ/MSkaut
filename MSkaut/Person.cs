using System;

namespace MSkaut
{
	public class Person
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public Date BirthDate { get; set; }
		public string Gender { get; set; }

		public Person(string firstName, string lastName, Date birthDate, string gender)
		{
			this.FirstName = firstName;
			this.LastName = lastName;
			this.BirthDate = birthDate;
			this.Gender = gender;
		}
	}
}
