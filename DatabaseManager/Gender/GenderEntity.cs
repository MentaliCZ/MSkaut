using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace DatabaseManager.Gender
{
    [Table("Gender")]
    public class GenderEntity : BaseModel
    {
        [PrimaryKey("gender_id")]
        public int Id { get; set; }

        [Column("name_cs")]
        public string NameCs { get; set; }

        [Column("name_en")]
        public string NameEn { get; set; }

        [Column("description")]
        public string Description { get; set; }
    }
}
