using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MSkaut;
using System.Collections.ObjectModel;
using DatabaseManager;
using Supabase;
using UserInterface.ViewModels.ModelRepresantations;
using UserInterface.Commands;

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

        public ObservableCollection<TransactionTypeViewModel> TransactionTypes { get; set; }
        public ObservableCollection<TransactionViewModel> Transactions { get; set; }

        public ObservableCollection<PersonViewModel> Participants { get; set; }

        private PersonViewModel selectedParticipant;
        public PersonViewModel SelectedParticipant { get => selectedParticipant; set { selectedParticipant = value; AddParticipantCommand.RaiseCanExecuteChanged(); } }
        public ObservableCollection<PersonViewModel> UsersPeople { get; set; }

        public RelayCommand AddParticipantCommand { get; set; }
        public RelayCommand AddTransactionCommand { get; set; }

        public EditEventViewModel(Client client, EventViewModel eventClass, ObservableCollection<PersonViewModel> usersPeople, 
            ObservableCollection<TransactionTypeViewModel> transactionTypes)
        {
            this.eventClass = eventClass;
            this.client = client;

            AddParticipantCommand = new(AddParticipant, x => CanAddParticipant());
            AddTransactionCommand = new(AddTransaction, x => true);

            TransactionTypes = transactionTypes;
            UsersPeople = usersPeople;
            Transactions = eventClass.Transactions;
            Participants = eventClass.Participants;
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public bool CanAddParticipant()
        {
            if (SelectedParticipant == null || Participants == null)
                return false;

            foreach (PersonViewModel person in Participants)
            {
                if (person.Id == SelectedParticipant.Id)
                    return false;
            }

            return true;
        }

        public async void AddParticipant(Object obj)
        {
            Participants.Add(SelectedParticipant);
            AddParticipantCommand.RaiseCanExecuteChanged();
            await DBEventPerson.AddEventParticipant((long)eventClass.Id, (long)SelectedParticipant.Id, client);
        }

        public void AddTransaction(Object obj)
        {
            Transaction transaction = new("...", 0, DateTime.Now, null, (long)eventClass.Id);
            Transactions.Add(new (transaction, client));
        }

    }
}
