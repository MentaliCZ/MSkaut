using System.Text;
using XSystem.Security.Cryptography;

namespace UserManager
{
    public class User
    {
        private string login;
        private string hashedPassword;
        private Role role;

        public User(string login, string password, Role role) 
        {
            this.login = login;
            hashedPassword = HashPassword(password);
            this.role = role;
        }

        private string HashPassword(string password)
        {
            using (var sha256 = new SHA256Managed())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                return Convert.ToBase64String(sha256.ComputeHash(passwordBytes));
            }
        }

        public bool CheckPassowrd(string password) 
        {
            return HashPassword(password) == hashedPassword;
        }

    }
}
