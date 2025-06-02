using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace DatabaseManager.Transaction
{
    [Table("Transaction")]
    public class TransactionEntity : BaseModel
    {
        [PrimaryKey("transaction_id")]
        public long Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("doc_name")]
        public string DocumentName { get; set; }

        [Column("transaction_type_id")]
        public long? TypeId { get; set; }

        [Column("amount")]
        public int Amount { get; set; }

        [Column("date")]
        public DateOnly Date { get; set; }

        [Column("event_id")]
        public long EventId { get; set; }
    }
}
