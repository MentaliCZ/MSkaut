using System;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using Supabase;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using static Supabase.Postgrest.QueryOptions;

namespace DatabaseManager
{
    [Table("Transaction_type")]
    public class TransactionTypeEntity : BaseModel
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

        public static async Task<List<TransactionTypeEntity>> GetUsersTransactionTypes(
            long userId,
            Client client
        )
        {
            try
            {
                var result = await client
                    .From<TransactionTypeEntity>()
                    .Select(x =>
                        new object[] { x.Id, x.Name, x.Description, x.IsExpense, x.OwnerId }
                    )
                    .Get();

                return result.Models.Where(x => x.OwnerId == null || x.OwnerId == userId).ToList();
            }
            catch (Exception)
            {
                return new List<TransactionTypeEntity>();
            }
        }

        public static async Task<long> CreateTransactionType(
            string name,
            string description,
            bool isExpense,
            long userId,
            Client client
        )
        {
            try
            {
                var dbTransactionType = new TransactionTypeEntity
                {
                    Name = name,
                    Description = description,
                    OwnerId = userId,
                    IsExpense = isExpense,
                };
                var result = await client
                    .From<TransactionTypeEntity>()
                    .Insert(
                        dbTransactionType,
                        new Supabase.Postgrest.QueryOptions
                        {
                            Returning = ReturnType.Representation,
                        }
                    );

                return result.Model.Id;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public static async Task<bool> UpdateTransactionType(
            long id,
            string name,
            string description,
            bool isExpense,
            long? userId,
            Client client
        )
        {
            try
            {
                var dbTransactionType = new TransactionTypeEntity
                {
                    Id = id,
                    Name = name,
                    Description = description,
                    OwnerId = userId,
                    IsExpense = isExpense,
                };

                await client.From<TransactionTypeEntity>().Upsert(dbTransactionType);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task RemoveTransactionTypeReferences(long typeId, Client client)
        {
            await client
                .From<TransactionEntity>()
                .Where(x => x.TypeId == typeId)
                .Set(x => x.TypeId, null)
                .Update();
        }

        public static async Task<bool> DeleteTransactionType(long id, Client client)
        {
            try
            {
                await TransactionTypeEntity.RemoveTransactionTypeReferences(id, client);

                await client.From<TransactionTypeEntity>().Where(x => x.Id == id).Delete();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
