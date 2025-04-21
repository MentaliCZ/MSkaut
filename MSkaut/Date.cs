using System;

namespace MSkaut
{
	public class Date
	{
		public ushort Day { get; set; }
		public ushort Month { get; set; }
		public ushort Year { get; set; }

		public Date(ushort day, ushort month, ushort year)
		{
			this.Day = day;
			this.Month = month;
			this.Year = year;
		}
	}
}
