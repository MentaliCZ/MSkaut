using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace DatabaseManager.UserRole
{
    [Table("User_role")]
    public class UserRoleEntity : BaseModel
    {
        [PrimaryKey("user_role_id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("power_int")]
        public int Hierarchy { get; set; }
    }
}
