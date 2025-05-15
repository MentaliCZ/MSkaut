using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Supabase;
using UserInterface.Commands;

namespace UserInterface.ViewModels.ModelRepresantations
{
    public abstract class EditableClass : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool isChanged;
        public bool IsChanged
        {
            get => isChanged;
            set
            {
                isChanged = value;
                OnPropertyChanged();
            }
        }

        private bool isProcessing;
        public bool IsProcessing
        {
            get => isProcessing;
            set
            {
                isProcessing = value;
                OnPropertyChanged();
            }
        }

        protected Client client;

        public RelayCommand SaveRowCommand { get; set; }
        public RelayCommand DeleteRowCommand { get; set; }

        protected EditableClass(Client client)
        {
            IsChanged = false;
            IsProcessing = false;
            this.client = client;

            SaveRowCommand = new(SaveRow, x => CanSaveRow());
            DeleteRowCommand = new(DeleteRow, x => CanDeleteRow());
        }

        public abstract bool CanSaveRow();

        public abstract bool CanDeleteRow();

        public abstract void SaveRow(Object obj);

        public abstract void DeleteRow(Object obj);

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
