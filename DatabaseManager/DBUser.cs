using System;
using System.Reflection;
using Supabase;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace DatabaseManager
{
    [Table("User")]
    public class DBUser : BaseModel
    {
        [PrimaryKey("user_id")]
        public long Id { get; set; }

        [Column("login")]
        public string Login { get; set; }

        [Column("password_hashed")]
        public string PasswordHashed { get; set; }

        [Column("role_id")]
        public int RoleId { get; set; }

        public static async Task<DBUser?> GetUser(
            string login,
            string hashedPassword,
            Client client
        )
        {
            return await client
                .From<DBUser>()
                .Select(x => new object[] { x.Id, x.Login, x.PasswordHashed })
                .Where(x => x.Login == login && x.PasswordHashed == hashedPassword)
                .Single();
        }

        public static async Task<DBUser?> GetUserByLogin(string login, Client client)
        {
            return await client
                .From<DBUser>()
                .Select(x => new object[] { x.Id, x.Login, x.PasswordHashed })
                .Where(x => x.Login == login)
                .Single();
        }

        public static async Task<bool> CreateUser(
            string login,
            string hashedPassword,
            Client client
        )
        {
            if (await GetUserByLogin(login, client) != null)
                return false;

            var dbUser = new DBUser
            {
                Login = login,
                PasswordHashed = hashedPassword,
                RoleId = 3,
            };

            await client.From<DBUser>().Insert(dbUser);

            return true;
        }
    }
}
