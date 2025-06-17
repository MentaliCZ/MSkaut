using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using DatabaseManager;
using MSkaut;
using Supabase;
using UserInterface.Commands;
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
        }

        public String StartDate
        {
            get => DateOnly.FromDateTime(eventClass.StartDate).ToShortDateString();
        }

        public String EndDate
        {
            get => DateOnly.FromDateTime(eventClass.EndDate).ToShortDateString();
        }

        public ObservableCollection<TransactionTypeViewModel> TransactionTypes { get; set; }

        public ObservableCollection<TransactionViewModel> Transactions { get; set; }
        public ObservableCollection<PersonViewModel> Participants { get; set; }

        private PersonViewModel selectedParticipant;
        public PersonViewModel SelectedParticipant
        {
            get => selectedParticipant;
            set
            {
                selectedParticipant = value;
                AddParticipantCommand.RaiseCanExecuteChanged();
            }
        }
        public ObservableCollection<PersonViewModel> UsersPeople { get; set; }

        public RelayCommand AddParticipantCommand { get; set; }
        public RelayCommand DeleteParticipantCommand { get; set; }

        public RelayCommand AddTransactionCommand { get; set; }
        public RelayCommand DeleteTransactionCommand { get; set; }

        public EditEventViewModel(
            Client client,
            EventViewModel eventClass,
            ObservableCollection<PersonViewModel> usersPeople,
            ObservableCollection<TransactionTypeViewModel> transactionTypes
        )
        {
            this.eventClass = eventClass;
            this.client = client;

            AddParticipantCommand = new(AddParticipant, x => CanAddParticipant());
            DeleteParticipantCommand = new(DeleteParticipant, x => true);

            AddTransactionCommand = new(AddTransaction, x => CanAddTransaction());
            DeleteTransactionCommand = new(DeleteTransaction, x => true);

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
            if (eventClass.Id == null || SelectedParticipant == null || Participants == null)
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
            await DBEventPerson.AddEventParticipant(
                (long)eventClass.Id,
                (long)SelectedParticipant.Id,
                client
            );
        }

        public bool CanAddTransaction()
        {
            return eventClass.Id != null;
        }

        public void AddTransaction(Object obj)
        {
            Transaction transaction = new(
                "...",
                "...",
                0,
                eventClass.StartDate,
                null,
                (long)eventClass.Id
            );
            Transactions.Add(new TransactionViewModel(transaction, client));
        }

        private async void DeleteParticipant(Object obj)
        {
            PersonViewModel person = (PersonViewModel)obj;

            if (person != null)
            {
                await DBEventPerson.DeleteEventParticipant((long)eventClass.Id, (long)person.Id, client);
                Participants.Remove(person);
            }
        }

        private void DeleteTransaction(Object obj)
        {
            TransactionViewModel transaction = (TransactionViewModel)obj;

            if (transaction != null && transaction.CanDeleteRow())
            {
                transaction.DeleteRow(null);
                Transactions.Remove(transaction);
            }
        }
    }
}
