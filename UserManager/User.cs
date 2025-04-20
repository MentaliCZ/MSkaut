using System.Text;
using XAct.Users;
using XSystem.Security.Cryptography;
using DatabaseManager;
using Supabase;

namespace UserManager
{
    public class User
    {
        public string Login { get; set; }
        private Role role;

        private User(string login, string password, Role role) 
        {
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

        public static bool CheckPassowrd(string password, string hashedPassword) 
        {
            return HashPassword(password) == hashedPassword;
        }

        //**************************************************************************************
        // Database methods
        //**************************************************************************************



        public static async Task<User> TryLogin(string login, string password, Client client)
        {
            DatabaseManager.DBUser? dbUser = await DatabaseManager.DBUser.GetUser(login, client);

            if (dbUser == null)
                return null;

            if (dbUser.PasswordHashed == HashPassword(password))
                return new User(login, password, null);

            return null;
        }

    }
}
