using System;
using Supabase;

namespace DatabaseManager.Gender
{
    public static class GenderFunc
    {
        public static async Task<List<GenderEntity>> GetAllGenders(Client client)
        {
            try
            {
                var result = await client
                    .From<GenderEntity>()
                    .Select(x => new object[] { x.Id, x.NameCs, x.NameEn, x.Description })
                    .Get();

                return result.Models;
            }
            catch (Exception)
            {
                return new List<GenderEntity>();
            }
        }
    }
}
