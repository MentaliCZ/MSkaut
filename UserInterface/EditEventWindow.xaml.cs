using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UserManager;
using DatabaseManager;
using UserInterface.ViewModels;
using MSkaut;
using Supabase;
using System.Collections.ObjectModel;
using UserInterface.ViewModels.ModelRepresantations;


namespace UserInterface
{

    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class EditEventWindow : Window
    {
        public EditEventWindow(Client client, EventViewModel eventClass, ObservableCollection<PersonViewModel> usersPeople,
            ObservableCollection<TransactionTypeViewModel> transactionTypes)
        {   
            InitializeComponent();
            EditEventViewModel editEventViewModel = new(client, eventClass, usersPeople, transactionTypes);
            DataContext = editEventViewModel;
        }
    }
}