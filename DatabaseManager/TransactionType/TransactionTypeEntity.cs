using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace DatabaseManager.TransactionType
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
    }
}
