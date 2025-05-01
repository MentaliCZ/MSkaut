using System;
using Supabase;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;


namespace DatabaseManager
{
    [Table("Transaction")]
    public class DBTransaction : BaseModel
    {
        [PrimaryKey("transaction_id")]
        public long Id { get; set; }

        [Column("transaction_type_id")]
        public long TypeId { get; set; }

        [Column("amount")]
        public int Amount { get; set; }

        [Column("is_expense")]
        public bool IsExpense { get; set; }

        [Column("date")]
        public DateOnly Date { get; set; }

        [Column("event_id")]
        public long EventId { get; set; }

        public static async Task<DBTransaction?> GetTransaction(long id, Client client)
        {
            return await client
           .From<DBTransaction>()
           .Select(x => new object[] { x.Id, x.TypeId, x.Amount, x.IsExpense, x.Date, x.EventId })
           .Where(x => x.Id == id)
           .Single();
        }

        public static async Task<List<DBTransaction>> GetEventTransactions(long event_id, Client client)
        {
            var result = await client
           .From<DBTransaction>()
           .Select(x => new object[] { x.Id, x.TypeId, x.Amount, x.IsExpense, x.Date })
           .Where(x => x.EventId == event_id)
           .Get();

            return result.Models;
        }

        public static async Task<bool> CreateTransaction(long typeId, int amount, bool isExpense,
            DateOnly date, long eventId, Client client)
        {
            var dbTransaction = new DBTransaction
            {
                TypeId = typeId,
                Amount = amount,
                IsExpense = isExpense,
                Date = date,
                EventId = eventId
            };

            await client.From<DBTransaction>().Insert(dbTransaction);

            return true;
        }
    }
}
