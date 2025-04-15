using System;
namespace MSkaut
{
	public class Date
	{
		public ushort day { get; set; }
		public ushort Month { get; set; }
		public ushort year { get; set; }

		public Date(ushort day, ushort month, ushort year)
		{
			this.day = day;
			this.month = month;
			this.year = year;
		}
	}
}
