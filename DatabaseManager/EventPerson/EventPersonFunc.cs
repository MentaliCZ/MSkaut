using Supabase;
using DatabaseManager.Person;

namespace DatabaseManager.EventPerson
{
    public static class EventPersonFunc
    {
        public static async Task<List<PersonEntity>> GetEventParticipants(
            long eventId,
            Client client
        )
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
                    result.Add(await PersonFunc.GetPerson(dbEventPerson.PersonId, client));
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
                var dbEventPerson = new EventPersonEntity
                {
                    EventId = eventId,
                    PersonId = personId,
                };

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
