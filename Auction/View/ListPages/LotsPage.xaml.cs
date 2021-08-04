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
    /// Interaction logic for LotsPage.xaml
    /// </summary>
    public partial class LotsPage : Page
    {
        public LotsPage()
        {
            InitializeComponent();
            DataContext = new DataViewModel();
        }
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            LotViewModel selectedLot = (LotViewModel)LotDataGrid.SelectedItem;
            MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
            var page = new LotInfoPage();
            page.DataContext = selectedLot;
            page.SaveButton.Command = selectedLot.UpdateCommand;
            page.CaptionLabel.Content = "Lot update";
            mainWindow.MainFrame.Navigate(page);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure to delete selected record from DB?", "Message", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                LotViewModel selectedLot = (LotViewModel)LotDataGrid.SelectedItem;
                auctionDBEntities db = new auctionDBEntities();
                var removeLot = db.Lot.FirstOrDefault(x => x.lot_id == selectedLot.LotId);
                db.Lot.Remove(removeLot);
                db.SaveChanges();
                ((DataViewModel)DataContext).Lots.Remove(selectedLot);                
                LotDataGrid.ItemsSource = ((DataViewModel)DataContext).Lots;
                TextBox_KeyUp(this, null);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
            var page = new LotInfoPage();
            var context = new LotViewModel();
            page.DataContext = context;
            page.CaptionLabel.Content = "Lot add";            
            page.SaveButton.Command = context.AddCommand;
            mainWindow.MainFrame.Navigate(page);
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            auctionDBEntities db = new auctionDBEntities();
            var lotList = ((DataViewModel)this.DataContext).Lots;
            var nameText = NameTextBox.Text.Trim();
            var sellerText = SellerTextBox.Text.Trim();
            var minPriceText = MinPriceTextBox.Text.Trim();
            var preferredPriceText = PreferredPriceTextBox.Text.Trim();
            var typeText = TypeTextBox.Text.Trim();
            var filtered = lotList.Where(x => x.Name.StartsWith(nameText) && x.Seller.StartsWith(sellerText) 
                        && x.MinPrice.ToString().StartsWith(minPriceText) && x.PreferredPrice.ToString().StartsWith(preferredPriceText)
                        && x.Type.StartsWith(typeText));
            LotDataGrid.ItemsSource = filtered;
        }
    }
}
