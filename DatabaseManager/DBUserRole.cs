using System;
using Supabase;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace DatabaseManager
{
    [Table("User_role")]
    public class DBUserRole : BaseModel
    {
        [PrimaryKey("user_role_id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("power_int")]
        public int Hierarchy { get; set; }

        public static async Task<DBUserRole?> GetUserRole(int id, Client client)
        {
            return await client
                .From<DBUserRole>()
                .Select(x => new object[] { x.Id, x.Name, x.Description, x.Hierarchy })
                .Where(x => x.Id == id)
                .Single();
        }

        public static async Task<DBUserRole?> GetUserRole(string name, Client client)
        {
            return await client
                .From<DBUserRole>()
                .Select(x => new object[] { x.Id, x.Name, x.Description, x.Hierarchy })
                .Where(x => x.Name == name)
                .Single();
        }

        public static async Task<Boolean> CreateUserRole(
            string name,
            string description,
            int power_int,
            Client client
        )
        {
            if (await GetUserRole(name, client) != null)
                return false;

            var dbUserRole = new DBUserRole
            {
                Name = name,
                Description = description,
                Hierarchy = power_int,
            };

            await client.From<DBUserRole>().Insert(dbUserRole);

            return true;
        }
    }
}
