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
    /// Interaction logic for BidInfoPage.xaml
    /// </summary>
    public partial class BidInfoPage : Page
    {
        public BidInfoPage()
        {
            InitializeComponent();
            var db = new auctionDBEntities();
            LotComboBox.ItemsSource = db.Lot
                                        .OrderBy(x => x.name)
                                        .Select(x => x.name)
                                        .ToList();
            
            ClientComboBox.ItemsSource = db.Client.OrderBy(x => x.name)
                                                  .ThenBy(x => x.surname)
                                                  .Select(x => x.name + " " + x.surname)
                                                  .ToList();            
        }
    }
}
