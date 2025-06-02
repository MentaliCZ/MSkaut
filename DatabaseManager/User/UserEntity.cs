using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace DatabaseManager.User
{
    [Table("User")]
    public class UserEntity : BaseModel
    {
        [PrimaryKey("user_id")]
        public long Id { get; set; }

        [Column("login")]
        public string Login { get; set; }

        [Column("password_hashed")]
        public string PasswordHashed { get; set; }

        [Column("role_id")]
        public int RoleId { get; set; }
    }
}
