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
    /// Interaction logic for AuctionsPage.xaml
    /// </summary>
    public partial class AuctionsPage : Page
    {
        public AuctionsPage()
        {
            InitializeComponent();
            DataContext = new DataViewModel();
        }
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            BidViewModel selectedBid = (BidViewModel)BidDataGrid.SelectedItem;
            MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
            var page = new BidInfoPage();
            page.DataContext = selectedBid;
            page.CaptionLabel.Content = "Bid update";
            page.SaveButton.Command = selectedBid.UpdateCommand;
            mainWindow.MainFrame.Navigate(page);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure to Delete selected record from DB?", "Message", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                BidViewModel selectedBid = (BidViewModel)BidDataGrid.SelectedItem;                
                auctionDBEntities db = new auctionDBEntities();
                var removeBid = db.Bid.FirstOrDefault(x => x.auction_id == selectedBid.AuctionId);                
                db.Bid.Remove(removeBid);
                db.SaveChanges();
                ((DataViewModel)DataContext).Auctions.Remove(selectedBid);
                BidDataGrid.ItemsSource = ((DataViewModel)DataContext).Auctions;
                TextBox_KeyUp(this, null);
            }            
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {            
            MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
            var page = new BidInfoPage();
            var context = new BidViewModel();
            page.DataContext = context;
            page.CaptionLabel.Content = "Bid add";
            page.SaveButton.Command = context.AddCommand;
            mainWindow.MainFrame.Navigate(page);
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            auctionDBEntities db = new auctionDBEntities();
            var lotList = ((DataViewModel)this.DataContext).Auctions;
            var lotText = LotTextBox.Text.Trim();
            var bidText = BidTextBox.Text.Trim();
            var clientText = ClientTextBox.Text.Trim();            
            var filtered = lotList.Where(x => x.LotName.StartsWith(lotText) && x.Client.StartsWith(clientText)
                        && x.Bid.ToString().StartsWith(bidText));
            BidDataGrid.ItemsSource = filtered;
        }
    }
}
