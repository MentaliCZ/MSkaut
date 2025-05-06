using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MSkaut;
using System.Collections.ObjectModel;
using DatabaseManager;
using Supabase;

namespace UserInterface.ViewModels
{
    public class EditEventViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private EventViewModel eventClass;
        private Client client;

        public string Name
        {
            get => eventClass.Name;

            set
            {
                eventClass.Name = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Transaction> Transactions;
        public ObservableCollection<PersonViewModel> Participants;
        

        public EditEventViewModel(Client client, EventViewModel eventClass)
        {
            this.eventClass = eventClass;
            this.client = client;

            Transactions = new(eventClass.Transactions);
            Participants = new(eventClass.Participants);
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
