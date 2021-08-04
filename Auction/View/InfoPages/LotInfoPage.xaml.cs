using Auction.Model;
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
    /// Interaction logic for LotInfoPage.xaml
    /// </summary>
    public partial class LotInfoPage : Page
    {
        public LotInfoPage()
        {
            InitializeComponent();
            var db = new auctionDBEntities();
            TypeComboBox.ItemsSource = db.LotType.Select(x => x.type).ToList();
            SellerComboBox.ItemsSource = db.Seller.OrderBy(x => x.name)
                                                  .ThenBy(x => x.surname)
                                                  .Select(x => x.name + " " + x.surname)
                                                  .ToList();
            LocationComboBox.ItemsSource = db.LotLocation.Select(x => (x.city + " " + x.street + " " + x.house_number)
                                                         .ToString())
                                                         .ToList();
        }
    }
}
