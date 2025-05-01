using System;
using Supabase;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace DatabaseManager
{
    [Table("Event")]
    public class DBEvent : BaseModel
	{
        [PrimaryKey("event_id")]
        public long Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("start_date")]
        public DateOnly StartDate { get; set; }

        [Column("end_date")]
        public DateOnly EndDate { get; set; }

        [Column("owner_id")]
        public long OwnerId { get; set; }

        public static async Task<List<DBEvent>> GetUserEvents(long id, Client client)
        {
            var result = await client
           .From<DBEvent>()
           .Select(x => new object[] { x.Id, x.Name, x.Description, x.StartDate, x.EndDate, x.OwnerId })
           .Where(x => x.OwnerId == id)
           .Get();

            return result.Models;
        }

        public static async Task<bool> CreateEvent(string name, string description, long ownerId, Client client)
        {
            var dbEvent = new DBEvent
            {
                Name = name,
                Description = description,
                OwnerId = ownerId
            };

            await client.From<DBEvent>().Insert(dbEvent);

            return true;
        }
    }
}
