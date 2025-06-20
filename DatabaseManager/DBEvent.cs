﻿using System;
using Supabase;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Exceptions;
using Supabase.Postgrest.Models;
using static Supabase.Postgrest.Constants;
using static Supabase.Postgrest.QueryOptions;

namespace DatabaseManager
{
    [Table("Event")]
    public class DBEvent : BaseModel
    {
        [PrimaryKey("event_id")]
        public long Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("start_date")]
        public DateOnly StartDate { get; set; }

        [Column("end_date")]
        public DateOnly EndDate { get; set; }

        [Column("owner_id")]
        public long OwnerId { get; set; }

        public static async Task<List<DBEvent>> GetUserEvents(long id, Client client)
        {
            try
            {
                var result = await client
                    .From<DBEvent>()
                    .Select(x =>
                        new object[]
                        {
                            x.Id,
                            x.Name,
                            x.Description,
                            x.StartDate,
                            x.EndDate,
                            x.OwnerId,
                        }
                    )
                    .Where(x => x.OwnerId == id)
                    .Order(x => x.StartDate, Ordering.Ascending)
                    .Get();

                return result.Models;
            }
            catch (Exception)
            {
                return new List<DBEvent>();
            }
        }

        public static async Task<long> CreateEvent(
            string name,
            string description,
            DateOnly startDate,
            DateOnly endDate,
            long ownerId,
            Client client
        )
        {
            try
            {
                var dbEvent = new DBEvent
                {
                    Name = name,
                    Description = description,
                    StartDate = startDate,
                    EndDate = endDate,
                    OwnerId = ownerId,
                };

                var result = await client
                    .From<DBEvent>()
                    .Insert(
                        dbEvent,
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

        public static async Task<bool> UpdateEvent(
            long id,
            string name,
            string description,
            DateOnly startDate,
            DateOnly endDate,
            long ownerId,
            Client client
        )
        {
            try
            {
                var dbEvent = new DBEvent
                {
                    Id = id,
                    Name = name,
                    Description = description,
                    StartDate = startDate,
                    EndDate = endDate,
                    OwnerId = ownerId,
                };

                await client.From<DBEvent>().Upsert(dbEvent);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task<bool> DeleteEvent(long id, Client client)
        {
            try
            {
                await client.From<DBEvent>().Where(x => x.Id == id).Delete();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
