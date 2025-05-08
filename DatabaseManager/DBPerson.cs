using System;
using System.Reflection;
using Supabase;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using static Supabase.Postgrest.Constants;
using static Supabase.Postgrest.QueryOptions;

namespace DatabaseManager
{
    [Table("Person")]
    public class DBPerson : BaseModel
	{
        [PrimaryKey("person_id")]
        public long Id { get; set; }

        [Column("first_name")]
        public string FirstName { get; set; }

        [Column("last_name")]
        public string LastName { get; set; }

        [Column("birth_date")]
        public DateOnly BirthDate { get; set; }

        [Column("gender_id")]
        public int GenderId { get; set; }

        [Column("creator_id")]
        public long CreatorId { get; set; }

        public static async Task<DBPerson?> GetPerson(long id, Client client)
        {
            return await client
           .From<DBPerson>()
           .Select(x => new object[] { x.Id, x.FirstName, x.LastName, x.BirthDate, x.GenderId })
           .Where(x => x.Id == id)
           .Single();
        }

        public static async Task<long> GetPerson(string firstName, string lastName, DateOnly birthDate, Client client)
        {
            var result = await client
           .From<DBPerson>()
           .Where(x => x.FirstName == firstName && x.LastName == lastName && x.BirthDate == birthDate)
           .Select(x => new object[] { x.Id})
           .Single();

            return result.Id;
        }

        public static async Task<List<DBPerson>> GetUsersPeople(long creatorId, Client client)
        {
            var result = await client
           .From<DBPerson>()
           .Select(x => new object[] { x.Id, x.FirstName, x.LastName, x.BirthDate, x.GenderId, x.CreatorId })
           .Where(x => x.CreatorId == creatorId)
           .Order(x => x.BirthDate, Ordering.Descending)
           .Get();

            return result.Models;
        }

        public static async Task<long> CreatePerson(string firstName, string lastName, DateOnly birthDate,
                                                    int genderId, long creatorId, Client client)
        {
            var dbPerson = new DBPerson
            {
                FirstName = firstName,
                LastName = lastName,
                BirthDate = birthDate,
                GenderId = genderId,
                CreatorId = creatorId
            };

            var result = await client.From<DBPerson>()
                .Insert(dbPerson, new Supabase.Postgrest.QueryOptions { Returning = ReturnType.Representation });

            return result.Model.Id;
        }

        public static async Task UpdatePerson(long id, string firstName, string lastName, DateOnly birthDate,
                                                    int genderId, long creatorId, Client client)
        {
            var dbPerson = new DBPerson
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                BirthDate = birthDate,
                GenderId = genderId,
                CreatorId = creatorId
            };

            await client.From<DBPerson>().Upsert(dbPerson);
        }


    }
}
