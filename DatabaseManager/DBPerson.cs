using System;
using Supabase;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace DatabaseManager
{
    [Table("Person")]
    public class DBPerson : BaseModel
	{
        [PrimaryKey("person_id")]
        public int Id { get; set; }

        [Column("first_name")]
        public string FirstName { get; set; }

        [Column("last_name")]
        public string LastName { get; set; }

        [Column("birth_date")]
        public DateOnly BirthDate { get; set; }

        [Column("gender_id")]
        public int GenderId { get; set; }

        [Column("creator_id")]
        public int CreatorId { get; set; }

        public static async Task<DBPerson?> GetPerson(int id, Client client)
        {
            return await client
           .From<DBPerson>()
           .Select(x => new object[] { x.Id, x.FirstName, x.LastName, x.BirthDate, x.GenderId })
           .Where(x => x.Id == id)
           .Single();
        }

        public static async Task<List<DBPerson>> GetUsersPeople(int creatorId, Client client)
        {
            var result = await client
           .From<DBPerson>()
           .Select(x => new object[] { x.Id, x.FirstName, x.LastName, x.BirthDate, x.GenderId, x.CreatorId })
           .Where(x => x.CreatorId == creatorId)
           .Get();

            return result.Models;
        }

        public static async Task<bool> CreatePerson(string firstName, string lastName, DateOnly birthDate,
                                                    int genderId, int creatorId, Client client)
        {
            var dbPerson = new DBPerson
            {
                FirstName = firstName,
                LastName = lastName,
                BirthDate = birthDate,
                GenderId = genderId,
                CreatorId = creatorId
            };

            await client.From<DBPerson>().Insert(dbPerson);

            return true;
        }


    }
}
