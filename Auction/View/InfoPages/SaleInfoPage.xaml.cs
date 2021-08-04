using Auction.Model;
using Auction.ViewModel;
using System;
using System.Collections.Generic;
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

namespace Auction.View.InfoPages
{
    /// <summary>
    /// Interaction logic for SaleInfoPage.xaml
    /// </summary>
    public partial class SaleInfoPage : Page
    {
        public SaleInfoPage()
        {
            InitializeComponent();
            var db = new auctionDBEntities();
            LotComboBox.ItemsSource = db.Lot
                                        .Where(x => x.Sale.Count == 0)
                                        .OrderBy(x => x.name)
                                        .Select(x => x.name)
                                        .ToList();            

            ClientComboBox.ItemsSource = db.Client.OrderBy(x => x.name)
                                                  .ThenBy(x => x.surname)
                                                  .Select(x => x.name + " " + x.surname)
                                                  .ToList();
            StatusComboBox.ItemsSource = db.OrderStatus.Select(x => x.status).ToList();
        }

        public SaleInfoPage(string lotName)
        {
            InitializeComponent();
            var db = new auctionDBEntities();
            LotComboBox.ItemsSource = db.Lot
                                        .Where(x => x.Sale.Count == 0 || x.name == lotName)
                                        .OrderBy(x => x.name)
                                        .Select(x => x.name)
                                        .ToList();

            ClientComboBox.ItemsSource = db.Client.OrderBy(x => x.name)
                                                  .ThenBy(x => x.surname)
                                                  .Select(x => x.name + " " + x.surname)
                                                  .ToList();
            StatusComboBox.ItemsSource = db.OrderStatus.Select(x => x.status).ToList();
        }

        private void LotComboBox_Selected(object sender, RoutedEventArgs e)
        {
            var context = (SaleViewModel)DataContext;
            if (LotComboBox.SelectedValue == null) 
            {
                context.Seller = "";
                context.Price = 0;
                return;
            }
            var lot = (new auctionDBEntities()).Lot.FirstOrDefault(x => x.name == LotComboBox.SelectedValue.ToString());
            context.Seller = lot.Seller.name + " " + lot.Seller.surname;

            if (lot.Bid.Count != 0)
                context.Price = lot.Bid.Max(x => x.bid1);
            else
                context.Price = 0;
        }
    }
}
