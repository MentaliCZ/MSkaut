using System;
using Supabase;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using static Supabase.Postgrest.Constants;
using static Supabase.Postgrest.QueryOptions;


namespace DatabaseManager
{
    [Table("Transaction")]
    public class DBTransaction : BaseModel
    {
        [PrimaryKey("transaction_id")]
        public long Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("transaction_type_id")]
        public long TypeId { get; set; }

        [Column("amount")]
        public int Amount { get; set; }

        [Column("date")]
        public DateOnly Date { get; set; }

        [Column("event_id")]
        public long EventId { get; set; }

        public static async Task<List<DBTransaction>> GetEventTransactions(long event_id, Client client)
        {
            var result = await client
           .From<DBTransaction>()
           .Select(x => new object[] { x.Id, x.Name, x.TypeId, x.Amount, x.EventId, x.Date })
           .Where(x => x.EventId == event_id)
           .Order(x => x.Date, Ordering.Ascending)
           .Get();

            return result.Models;
        }

        public static async Task<long> CreateTransaction(string name, long typeId, int amount,
            DateOnly date, long eventId, Client client)
        {
            var dbTransaction = new DBTransaction
            {
                Name = name,
                TypeId = typeId,
                Amount = amount,
                Date = date,
                EventId = eventId
            };

            var result = await client.From<DBTransaction>()
                .Insert(dbTransaction, new Supabase.Postgrest.QueryOptions { Returning = ReturnType.Representation });

            return result.Model.Id;
        }

        public static async Task UpdateTransaction(long id, string name, long typeId, int amount,
            DateOnly date, long eventId, Client client)
        {
            var dbTransaction = new DBTransaction
            {
                Id = id,
                Name = name,
                TypeId = typeId,
                Amount = amount,
                Date = date,
                EventId = eventId
            };

            await client.From<DBTransaction>().Upsert(dbTransaction);

        }
    }
}
