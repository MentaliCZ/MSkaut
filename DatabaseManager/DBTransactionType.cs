using System;
using Supabase;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;


namespace DatabaseManager
{
    [Table("Transaction_type")]
    public class DBTransactionType : BaseModel
    {
        [PrimaryKey("transaction_type_id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }

        public static async Task<DBTransactionType?> GetTransactionType(int id, Client client)
        {
            return await client
           .From<DBTransactionType>()
           .Select(x => new object[] { x.Id, x.Name, x.Description })
           .Where(x => x.Id == id)
           .Single();
        }

        public static async Task<DBTransactionType?> GetTransactionType(string name, Client client)
        {
            return await client
           .From<DBTransactionType>()
           .Select(x => new object[] { x.Id, x.Name, x.Description })
           .Where(x => x.Name == name)
           .Single();
        }

        public static async Task<bool> CreateTransactionType(string name, string description, Client client)
        {
            if (await GetTransactionType(name, client) != null)
                return false;

            var dbTransactionType = new DBTransactionType
            {
                Name = name,
                Description = description
            };

            await client.From<DBTransactionType>().Insert(dbTransactionType);

            return true;
        }
    }
}
