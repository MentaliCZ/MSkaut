using System.Text;
using XAct.Users;
using XSystem.Security.Cryptography;
using DatabaseManager;
using Supabase;

namespace UserManager
{
    public class User
    {
        public long Id { get; set; }
        public string Login { get; set; }
        private Role role;

        private User(long id, string login, string password, Role role)
        {
            this.Id = id;
            this.Login = login;
            string hashedPassword = HashPassword(password);
            this.role = role;
        }

        public static string HashPassword(string password)
        {
            using (var sha256 = new SHA256Managed())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                return Convert.ToBase64String(sha256.ComputeHash(passwordBytes));
            }
        }

        //**************************************************************************************
        // Database methods
        //**************************************************************************************

        public static async Task<User?> TryLogin(string login, string password, Client client)
        {
            DBUser? dbUser = await DBUser.GetUser(login, HashPassword(password), client);

            if (dbUser == null)
                return null;

            return new User(dbUser.Id, login, password, await Role.GetRole(dbUser.RoleId, client));
        }

        public static async Task<bool> CreateUser(string login, string password, Client client)
        {
            return await DBUser.CreateUser(login, HashPassword(password), client);
        }
    }
}
