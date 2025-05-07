using System;
using UserInterface.Commands;
using Supabase;

namespace UserInterface.ViewModels.ModelRepresantations {
	public abstract class EditableClass
	{
		public bool IsChanged { get; set; }
		protected Client client;

        public RelayCommand SaveRowCommand { get; set; }
        public RelayCommand DeleteRowCommand { get; set; }

        protected EditableClass(Client client)
        {
            IsChanged = false;
            this.client = client;

            SaveRowCommand = new(SaveRow, x => CanSaveRow());
            DeleteRowCommand = new(DeleteRow, x => CanDeleteRow());
       
        }

        public abstract bool CanSaveRow();

        public abstract bool CanDeleteRow();

		public abstract void SaveRow(Object obj);

		public abstract void DeleteRow(Object obj);
    }
}
