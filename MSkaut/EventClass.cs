using System;
using System.ComponentModel.DataAnnotations.Schema;
using Supabase;
using UserManager;
using DatabaseManager;
using XAct.Events;
using System.Collections.ObjectModel;
using MSkaut;
using static Microsoft.IO.RecyclableMemoryStreamManager;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace MSkaut
{
	public class EventClass
	{
		public long? Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }

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

        public EventClass(long id, string name, string description, DateOnly startDate, DateOnly endDate, long creatorId) : this(name, description, creatorId)
        {
            this.Id = id;
			StartDate = startDate.ToDateTime(TimeOnly.Parse("10:00 PM"));
			EndDate = endDate.ToDateTime(TimeOnly.Parse("10:00 PM"));
        }
    }
}
