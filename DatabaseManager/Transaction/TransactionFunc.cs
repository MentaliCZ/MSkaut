using System;
using Supabase;
using static Supabase.Postgrest.Constants;
using static Supabase.Postgrest.QueryOptions;

namespace DatabaseManager.Transaction
{
    public static class TransactionFunc
    {
        public static async Task<List<TransactionEntity>> GetEventTransactions(
            long event_id,
            Client client
        )
        {
            try
            {
                var result = await client
                    .From<TransactionEntity>()
                    .Select(x =>
                        new object[]
                        {
                            x.Id,
                            x.DocumentName,
                            x.Name,
                            x.TypeId,
                            x.Amount,
                            x.EventId,
                            x.Date,
                        }
                    )
                    .Where(x => x.EventId == event_id)
                    .Order(x => x.Date, Ordering.Ascending)
                    .Get();

                return result.Models;
            }
            catch (Exception)
            {
                return new List<TransactionEntity>();
            }
        }

        public static async Task<long> CreateTransaction(
            string documentName,
            string name,
            long typeId,
            int amount,
            DateOnly date,
            long eventId,
            Client client
        )
        {
            try
            {
                var dbTransaction = new TransactionEntity
                {
                    DocumentName = documentName,
                    Name = name,
                    TypeId = typeId,
                    Amount = amount,
                    Date = date,
                    EventId = eventId,
                };

                var result = await client
                    .From<TransactionEntity>()
                    .Insert(
                        dbTransaction,
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

        public static async Task<bool> UpdateTransaction(
            long id,
            string documentName,
            string name,
            long typeId,
            int amount,
            DateOnly date,
            long eventId,
            Client client
        )
        {
            try
            {
                var dbTransaction = new TransactionEntity
                {
                    Id = id,
                    DocumentName = documentName,
                    Name = name,
                    TypeId = typeId,
                    Amount = amount,
                    Date = date,
                    EventId = eventId,
                };

                await client.From<TransactionEntity>().Upsert(dbTransaction);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task<bool> DeleteTransaction(long id, Client client)
        {
            try
            {
                await client.From<TransactionEntity>().Where(x => x.Id == id).Delete();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
