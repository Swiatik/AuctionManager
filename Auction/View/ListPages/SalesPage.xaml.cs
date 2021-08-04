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
    /// Interaction logic for OrdersPage.xaml
    /// </summary>
    public partial class OrdersPage : Page
    {
        public OrdersPage()
        {
            InitializeComponent();
            DataContext = new DataViewModel();
            PeriodComboBox.ItemsSource = new List<string>() { "Day", "Week", "Month", "Year", "All time" };
            PeriodComboBox.SelectedIndex = 4;
    }
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            SaleViewModel selectedSale = (SaleViewModel)SaleDataGrid.SelectedItem;
            MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
            var page = new SaleInfoPage(selectedSale.LotName);
            page.DataContext = selectedSale;
            page.SaveButton.Command = selectedSale.UpdateCommand;
            page.CaptionLabel.Content = "Sale update";
            mainWindow.MainFrame.Navigate(page);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure to Delete selected record from DB?", "Message", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                var selectedSale = (SaleViewModel)SaleDataGrid.SelectedItem;                
                auctionDBEntities db = new auctionDBEntities();
                var removeSale = db.Sale.FirstOrDefault(x => x.order_id == selectedSale.OrderId);                
                db.Sale.Remove(removeSale);
                db.SaveChanges();
                ((DataViewModel)DataContext).Orders.Remove(selectedSale);
                SaleDataGrid.ItemsSource = ((DataViewModel)DataContext).Orders;
                TextBox_KeyUp(this, null);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
            var page = new SaleInfoPage();
            var context = new SaleViewModel();
            page.DataContext = context;
            page.CaptionLabel.Content = "Sale add";
            page.SaveButton.Command = context.AddCommand;
            mainWindow.MainFrame.Navigate(page);
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            auctionDBEntities db = new auctionDBEntities();
            var lotList = ((DataViewModel)this.DataContext).Orders;
            var lotText = LotTextBox.Text.Trim();            
            var sellerText = SellerTextBox.Text.Trim();
            var clientText = ClientTextBox.Text.Trim();
            var priceText = PriceTextBox.Text.Trim();
            var statusText = StatusTextBox.Text.Trim();
            var filtered = lotList.Where(x => x.LotName.StartsWith(lotText) && x.Seller.StartsWith(sellerText)
                        && x.Price.ToString().StartsWith(priceText) && x.Client.StartsWith(clientText)
                        && x.Status.StartsWith(statusText));
            SaleDataGrid.ItemsSource = filtered;
        }

        private void PeriodComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var period = ((ComboBox)sender).SelectedValue.ToString();
            var newStat = new StatViewModel();
            var db = new auctionDBEntities();
            DateTime startDate;

            switch (period)
            {
                case "Day": startDate = DateTime.Today;                    
                    break;
                case "Week": startDate = DateTime.Today.AddDays(-6);
                    break;
                case "Month": startDate = DateTime.Today.AddMonths(-1);
                    break;
                case "Year": startDate = DateTime.Today.AddYears(-1);
                    break;
                default: startDate = DateTime.Today.AddYears(-2);
                    break;                                   
            }

            newStat.SoldCount = db.Sale.Count(x => x.date >= startDate);
            if (db.Sale.Count(x => x.date >= startDate) > 0)
                newStat.TotalPrice = db.Sale.Where(x => x.date >= startDate).Sum(x => x.price).Value;
            else
                newStat.TotalPrice = 0;
            newStat.BidCount = db.Bid.Count(x => x.time >= startDate);
            newStat.ClientCount = db.Client.Count(x => x.Bid.Any(y => y.time >= startDate));
            ((DataViewModel)DataContext).Stat[0] = newStat;
        }
    }
}
