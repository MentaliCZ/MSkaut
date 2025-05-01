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
        public long EventId { get; set; }

        [Column("person_id")]
        public long PersonId { get; set; }

        [Column("description")]
        public string Description { get; set; }

        public static async Task<List<DBPerson>> GetEventParticipants(long eventId, Client client)
        {
            var participantsId = (await client
           .From<DBEventPerson>()
           .Select(x => new object[] { x.PersonId })
           .Where(x => x.EventId == eventId)
           .Get()).Models;

            var result = new List<DBPerson?>();

            foreach (DBEventPerson dbEventPerson in participantsId) 
            {
                result.Add(await DBPerson.GetPerson(dbEventPerson.PersonId, client));
            }

            return result;
        }
    }
}
