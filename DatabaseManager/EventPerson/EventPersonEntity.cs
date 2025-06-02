using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace DatabaseManager.EventPerson
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

    }
}
