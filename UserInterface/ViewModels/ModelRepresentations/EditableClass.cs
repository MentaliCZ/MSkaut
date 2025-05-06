using System;
using UserInterface.Commands;
using Supabase;

namespace UserInterface.ViewModels.ModelRepresantations {
	public abstract class EditableClass
	{
		public bool IsChanged { get; set; }
		protected Client client;

        public RelayCommand SaveRowCommand { get; set; }

        protected EditableClass(Client client)
        {
            IsChanged = false;
            this.client = client;

			SaveRowCommand = new(SaveRow, _ => true);
        }

		public abstract void SaveRow(Object obj);

		public abstract void DeleteRow(Object obj);
    }
}
