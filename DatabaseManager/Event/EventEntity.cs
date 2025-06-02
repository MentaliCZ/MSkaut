using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace DatabaseManager.Event
{
    [Table("Event")]
    public class EventEntity : BaseModel
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

    }
}
