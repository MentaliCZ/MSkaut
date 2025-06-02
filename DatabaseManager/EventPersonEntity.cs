using System;
using Supabase;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace DatabaseManager
{
    [Table("Event_person")]
    public class EventPersonEntity : BaseModel
    {
        [PrimaryKey("event_id")]
        public long EventId { get; set; }

        [Column("person_id")]
        public long PersonId { get; set; }

        [Column("description")]
        public string Description { get; set; }

        public static async Task<List<PersonEntity>> GetEventParticipants(long eventId, Client client)
        {
            try
            {
                var participantsId = (
                    await client
                        .From<EventPersonEntity>()
                        .Select(x => new object[] { x.PersonId })
                        .Where(x => x.EventId == eventId)
                        .Get()
                ).Models;

                var result = new List<PersonEntity?>();

                foreach (EventPersonEntity dbEventPerson in participantsId)
                {
                    result.Add(await PersonEntity.GetPerson(dbEventPerson.PersonId, client));
                }

                return result;
            }
            catch (Exception)
            {
                return new List<PersonEntity>();
            }
        }

        public static async Task<bool> AddEventParticipant(
            long eventId,
            long personId,
            Client client
        )
        {
            try
            {
                var dbEventPerson = new EventPersonEntity { EventId = eventId, PersonId = personId };

                await client.From<EventPersonEntity>().Upsert(dbEventPerson);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task<bool> DeleteEventParticipant(
            long eventId,
            long personId,
            Client client
        )
        {
            try
            {
                await client
                    .From<EventPersonEntity>()
                    .Where(x => x.EventId == eventId && x.PersonId == personId)
                    .Delete();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task DeleteAllPersonReferences(long personId, Client client)
        {
            await client.From<EventPersonEntity>().Where(x => x.PersonId == personId).Delete();
        }
    }
}
