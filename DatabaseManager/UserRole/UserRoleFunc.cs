using System;
using Supabase;

namespace DatabaseManager.UserRole
{
    public static class UserRoleFunc
    {
        public static async Task<UserRoleEntity?> GetUserRole(int id, Client client)
        {
            return await client
                .From<UserRoleEntity>()
                .Select(x => new object[] { x.Id, x.Name, x.Description, x.Hierarchy })
                .Where(x => x.Id == id)
                .Single();
        }

        public static async Task<UserRoleEntity?> GetUserRole(string name, Client client)
        {
            return await client
                .From<UserRoleEntity>()
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

            var dbUserRole = new UserRoleEntity
            {
                Name = name,
                Description = description,
                Hierarchy = power_int,
            };

            await client.From<UserRoleEntity>().Insert(dbUserRole);

            return true;
        }
    }
}
