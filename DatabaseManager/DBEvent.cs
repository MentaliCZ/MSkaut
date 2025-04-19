using System;
using Supabase;
using Supabase.Postgrest.Attributes;

namespace DatabaseManager
{
    [Table("Event")]
    public class DBEvent : BaseModel
	{
        [PrimaryKey("event_id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("owner_id")]
        public int OwnerId { get; set; }
    }
}
