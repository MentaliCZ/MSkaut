using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using DatabaseManager;
using MSkaut;
using Supabase;
using UserManager;
using XAct.Events;
using static Microsoft.IO.RecyclableMemoryStreamManager;

namespace MSkaut
{
    public class EventClass
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string DocumentPrefix { get; set; }

        public long CreatorId { get; set; }

        public EventClass(string name, string description, long creatorId)
        {
            this.Id = null;
            this.Name = name;
            this.Description = description;
            this.CreatorId = creatorId;

            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
        }

        public EventClass(
            long id,
            string name,
            string description,
            DateOnly startDate,
            DateOnly endDate,
            long creatorId
        )
            : this(name, description, creatorId)
        {
            this.Id = id;
            StartDate = startDate.ToDateTime(TimeOnly.Parse("10:00 PM"));
            EndDate = endDate.ToDateTime(TimeOnly.Parse("10:00 PM"));
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
