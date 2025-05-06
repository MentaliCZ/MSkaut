using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MSkaut;
using System.Collections.ObjectModel;
using DatabaseManager;
using Supabase;
using UserInterface.ViewModels.ModelRepresantations;

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

        public ObservableCollection<Transaction> Transactions { get; set; }
        public ObservableCollection<PersonViewModel> Participants { get; set; }

        public ObservableCollection<PersonViewModel> UsersPeople { get; set; }
        

        public EditEventViewModel(Client client, EventViewModel eventClass, ObservableCollection<PersonViewModel> usersPeople)
        {
            this.eventClass = eventClass;
            this.client = client;

            Transactions = eventClass.Transactions;
            Participants = eventClass.Participants;
            UsersPeople = usersPeople;
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
