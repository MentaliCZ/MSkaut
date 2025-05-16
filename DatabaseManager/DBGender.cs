using System;
using Supabase;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace DatabaseManager
{
    [Table("Gender")]
    public class DBGender : BaseModel
    {
        [PrimaryKey("gender_id")]
        public int Id { get; set; }

        [Column("name_cs")]
        public string NameCs { get; set; }

        [Column("name_en")]
        public string NameEn { get; set; }

        [Column("description")]
        public string Description { get; set; }


        public static async Task<List<DBGender>> GetAllGenders(Client client)
        {
            try
            {
                var result = await client
                    .From<DBGender>()
                    .Select(x => new object[] { x.Id, x.NameCs, x.NameEn, x.Description })
                    .Get();

                return result.Models;
            }
            catch (Exception)
            {
                return new List<DBGender>();
            }
        }
    }
}
