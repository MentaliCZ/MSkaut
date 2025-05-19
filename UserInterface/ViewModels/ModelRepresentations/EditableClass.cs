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

        private bool _isChanged;
        public bool IsChanged
        {
            get => _isChanged;
            set
            {
                _isChanged = value;
                SaveRowCommand.RaiseCanExecuteChanged();
                DeleteRowCommand.RaiseCanExecuteChanged();
                OnPropertyChanged();
            }
        }

        private bool _isProcessing;
        public bool IsProcessing
        {
            get => _isProcessing;
            set
            {
                _isProcessing = value;
                SaveRowCommand.RaiseCanExecuteChanged();
                DeleteRowCommand.RaiseCanExecuteChanged();
                OnPropertyChanged();
            }
        }

        protected Client client;

        public RelayCommand SaveRowCommand { get; set; }
        public RelayCommand DeleteRowCommand { get; set; }

        protected EditableClass(Client client)
        {
            this.client = client;

            SaveRowCommand = new(SaveRow, x => CanSaveRow());
            DeleteRowCommand = new(DeleteRow, x => CanDeleteRow());

            IsChanged = false;
            IsProcessing = false;
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
