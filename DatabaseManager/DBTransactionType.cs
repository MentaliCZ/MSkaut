using System;
using System.Diagnostics;
using Supabase;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System.Linq;
using static Supabase.Postgrest.QueryOptions;
using System.Data.Common;


namespace DatabaseManager
{
    [Table("Transaction_type")]
    public class DBTransactionType : BaseModel
    {
        [PrimaryKey("transaction_type_id")]
        public long Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("is_expense")]
        public bool IsExpense { get; set; }

        [Column("owner_id")]
        public long? OwnerId { get; set; }


        public static async Task<DBTransactionType?> GetTransactionType(long id, Client client)
        {
            return await client
           .From<DBTransactionType>()
           .Select(x => new object[] { x.Id, x.Name, x.Description, x.IsExpense })
           .Where(x => x.Id == id)
           .Single();
        }

        public static async Task<DBTransactionType?> GetTransactionType(string name, Client client)
        {
            return await client
           .From<DBTransactionType>()
           .Select(x => new object[] { x.Id, x.Name, x.Description, x.IsExpense })
           .Where(x => x.Name == name)
           .Single();
        }

        public static async Task<List<DBTransactionType>> GetUsersTransactionTypes(long userId, Client client)
        {
            var result = await client
                .From<DBTransactionType>()
                .Select(x => new object[] { x.Id, x.Name, x.Description, x.IsExpense, x.OwnerId })
                .Get();


            return result.Models
                .Where(x => x.OwnerId == null || x.OwnerId == userId)
                .ToList();
        }

        public static async Task<long> CreateTransactionType(string name, string description, bool isExpense, long userId, Client client)
        {
            var dbTransactionType = new DBTransactionType
            {
                Name = name,
                Description = description,
                OwnerId = userId,
                IsExpense = isExpense
            };
            var result = await client.From<DBTransactionType>()
                .Insert(dbTransactionType, new Supabase.Postgrest.QueryOptions { Returning = ReturnType.Representation });

            return result.Model.Id;
        }

        public static async Task UpdateTransactionType(long id, string name, string description, bool isExpense, long? userId, Client client)
        {
            var dbTransactionType = new DBTransactionType
            {
                Id = id,
                Name = name,
                Description = description,
                OwnerId = userId,
                IsExpense = isExpense
            };

            await client.From<DBTransactionType>().Upsert(dbTransactionType);
        }

        public static async Task DeleteTransactionType(long id, Client client)
        {
            await client
                  .From<DBTransactionType>()
                  .Where(x => x.Id == id)
                  .Delete();
        }
    }
}
