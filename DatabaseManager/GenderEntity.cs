using System;
using Supabase;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace DatabaseManager
{
    [Table("Gender")]
    public class GenderEntity : BaseModel
    {
        [PrimaryKey("gender_id")]
        public int Id { get; set; }

        [Column("name_cs")]
        public string NameCs { get; set; }

        [Column("name_en")]
        public string NameEn { get; set; }

        [Column("description")]
        public string Description { get; set; }


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
