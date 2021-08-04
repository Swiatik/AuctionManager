using Auction.Model;
using Auction.View.InfoPages;
using Auction.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Auction.View
{
    /// <summary>
    /// Interaction logic for ClientsPage.xaml
    /// </summary>
    public partial class ClientsPage : Page
    {
        public ClientsPage()
        {
            InitializeComponent();
            DataContext = new DataViewModel();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            ClientViewModel selectedClient = (ClientViewModel)ClientDataGrid.SelectedItem;
            MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
            var page = new ClientInfoPage();
            page.DataContext = selectedClient;
            page.SaveButton.Command = selectedClient.UpdateCommand;
            page.CaptionLabel.Content = "Client update";
            mainWindow.MainFrame.Navigate(page);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure to Delete selected record from DB?", "Message", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                var selectedClient = (ClientViewModel)ClientDataGrid.SelectedItem;
                auctionDBEntities db = new auctionDBEntities();
                var removeClient = db.Client.FirstOrDefault(x => x.client_id == selectedClient.ClientId);
                db.Client.Remove(removeClient);
                db.SaveChanges();
                ((DataViewModel)DataContext).Clients.Remove(selectedClient);
                ClientDataGrid.ItemsSource = ((DataViewModel)DataContext).Clients;
                TextBox_KeyUp(this, null);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
            var page = new ClientInfoPage();
            var context = new ClientViewModel();
            page.DataContext = context;
            page.CaptionLabel.Content = "Client add";
            page.SaveButton.Command = context.AddCommand;
            mainWindow.MainFrame.Navigate(page);
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            auctionDBEntities db = new auctionDBEntities();
            var lotList = ((DataViewModel)this.DataContext).Clients;
            var nameText = NameTextBox.Text.Trim();
            var surnameText = SurnameTextBox.Text.Trim();
            var numberText = NumberTextBox.Text.Trim();
            var emailText = EmailTextBox.Text.Trim();            
            var filtered = lotList.Where(x => x.Name.StartsWith(nameText) && x.Surname.StartsWith(surnameText)                         
                        && x.Number.StartsWith(numberText) && x.Email.StartsWith(emailText));
            ClientDataGrid.ItemsSource = filtered;
        }
    }
}
