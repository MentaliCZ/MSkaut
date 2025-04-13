namespace MSkaut
{
    public class Transaction
    {
        private int id = -1;
        private TransactionType? type;
        public uint Amount { get; private set; }
        public Date Date { get; private set; }

        public Transaction(uint amount, Date date)
        {
            this.Amount = amount;
            this.Date = date;
        }

        public void setType(TransactionType type)
        {
            this.type = type;
        }
    }
}
