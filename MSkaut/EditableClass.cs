using System;
using MSkaut.Commands;
using Supabase;

namespace MSkaut {
	public abstract class EditableClass
	{
		public bool IsChanged { get; set; }
		public long? Id { get; set; }
		protected Client client;

        public RelayCommand SaveRowCommand { get; set; }

        protected EditableClass(Client client)
        {
			this.Id = null;
            IsChanged = false;
            this.client = client;

			SaveRowCommand = new(SaveRow, x => true);
        }

		public abstract void SaveRow(Object obj);

    }
}
