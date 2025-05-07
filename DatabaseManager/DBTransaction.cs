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
           .Select(x => new object[] { x.Id, x.Name, x.TypeId, x.Amount, x.Date })
           .Where(x => x.EventId == event_id)
           .Get();

            return result.Models;
        }

        public static async Task<bool> CreateTransaction(string name, long typeId, int amount, bool isExpense,
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

            await client.From<DBTransaction>().Insert(dbTransaction);

            return true;
        }
    }
}
