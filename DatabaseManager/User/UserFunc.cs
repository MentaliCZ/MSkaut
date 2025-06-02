using System;
using Supabase;

namespace DatabaseManager.User
{
    public static class UserFunc
    {
        public static async Task<UserEntity?> GetUser(
            string login,
            string hashedPassword,
            Client client
        )
        {
            return await client
                .From<UserEntity>()
                .Select(x => new object[] { x.Id, x.Login, x.PasswordHashed })
                .Where(x => x.Login == login && x.PasswordHashed == hashedPassword)
                .Single();
        }

        public static async Task<UserEntity?> GetUserByLogin(string login, Client client)
        {
            return await client
                .From<UserEntity>()
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

            var dbUser = new UserEntity
            {
                Login = login,
                PasswordHashed = hashedPassword,
                RoleId = 3,
            };

            await client.From<UserEntity>().Insert(dbUser);

            return true;
        }
    }
}
