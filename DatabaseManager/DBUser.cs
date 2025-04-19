using Supabase.Postgrest.Attributes;
using System;
using Supabase;

namespace DatabaseManager
{
    [Table("User")]
    public class DBUser : BaseModel
	{
        [PrimaryKey("user_id")]
        public int Id { get; set; }

        [Column("login")]
        public string Login { get; set; }

        [Column("password_hashed")]
        public string PasswordHashed { get; set; }

        [Column("role_id")]
        public int RoleId { get; set; }
    }
}
