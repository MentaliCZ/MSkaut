using System;
using UserInterface.Commands;
using Supabase;

namespace UserInterface.ViewModels {
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

			SaveRowCommand = new(SaveRow, _ => true);
        }

		public abstract void SaveRow(Object obj);

		public abstract void DeleteRow(Object obj);
    }
}
