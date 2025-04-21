using System;
using Supabase;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace DatabaseManager
{
    [Table("Event_person")]
    public class DBEventPerson : BaseModel
    {
        [PrimaryKey("event_id")]
        public int EventId { get; set; }

        [Column("person_id")]
        public string PersonId { get; set; }

        [Column("description")]
        public string Description { get; set; }

        public static async Task<List<DBEventPerson>> GetEventParticipants(int eventId, Client client)
        {
            var result = await client
           .From<DBEventPerson>()
           .Select(x => new object[] { x.EventId, x.PersonId })
           .Where(x => x.EventId == eventId)
           .Get();

            return result.Models;
        }
    }
}
