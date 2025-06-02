using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace DatabaseManager.Person
{
    [Table("Person")]
    public class PersonEntity : BaseModel
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
    }
}
