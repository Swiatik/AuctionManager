using Auction.Commands;
using Auction.Model;
using Auction.View;
using Auction.View.InfoPages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Auction.ViewModel
{
    class DataViewModel : BaseViewModel
    {
        private ObservableCollection<BidViewModel> _bids;
        public ObservableCollection<BidViewModel> Auctions
        {
            get => _bids;
            set
            {
                _bids = value;
                OnPropertyChanged(nameof(Auction));
            }
        }

        private ObservableCollection<ClientViewModel> _clients;
        public ObservableCollection<ClientViewModel> Clients
        {
            get => _clients;
            set
            {
                _clients = value;
                OnPropertyChanged(nameof(Clients));
            }
        }

        private ObservableCollection<LotViewModel> _lots;
        public ObservableCollection<LotViewModel> Lots
        {
            get => _lots;
            set
            {
                _lots = value;
                OnPropertyChanged(nameof(Lots));
            }
        }

        private ObservableCollection<LotLocationViewModel> _lotLocations;
        public ObservableCollection<LotLocationViewModel> LotLocations
        {
            get => _lotLocations;
            set
            {
                _lotLocations = value;
                OnPropertyChanged(nameof(LotLocations));
            }
        }

        private ObservableCollection<SaleViewModel> _sales;
        public ObservableCollection<SaleViewModel> Orders
        {
            get => _sales;
            set
            {
                _sales = value;
                OnPropertyChanged(nameof(Orders));
            }
        }

        private ObservableCollection<SellerViewModel> _sellers;
        public ObservableCollection<SellerViewModel> Sellers
        {
            get => _sellers;
            set
            {
                _sellers = value;
                OnPropertyChanged(nameof(Sellers));
            }
        }
        public RelayCommand BackCommand => new RelayCommand(obj =>
        {
            MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
            mainWindow.MainFrame.Navigate(new MainMenuPage());
        });

        private ObservableCollection<StatViewModel> _stat;
        public ObservableCollection<StatViewModel> Stat
        {
            get => _stat;
            set
            {
                _stat = value;
                OnPropertyChanged(nameof(Stat));
            }
        }

        private string _user;
        public string User

        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged(nameof(User));
            }
        }
        public DataViewModel()
        {
            _stat = new ObservableCollection<StatViewModel>();
            _stat.Add(new StatViewModel(1, 1, 1, 1));

            User = ((Employee)App.Current.Resources["User"]).Post.post1;
            var db = new auctionDBEntities();

            _bids = new ObservableCollection<BidViewModel>();
            var post = db.Post.First().post1;
            var bidList = new ObservableCollection<Bid>(db.Bid.ToList());
                        
            foreach (var bid in bidList.OrderBy(x => x.Lot.name).ThenByDescending(x => x.create_date))
            {
                _bids?.Add((BidViewModel)bid);
            }

            _clients = new ObservableCollection<ClientViewModel>();
            var clientList = new ObservableCollection<Client>(db.Client);            

            foreach(var client in clientList.OrderBy(x => x.name).ThenBy(x => x.surname))
            {                
                _clients.Add((ClientViewModel)client);
            }

                _lots = new ObservableCollection<LotViewModel>();
                var lotList = new ObservableCollection<Lot>(db.Lot.ToList());            

                foreach (var lot in lotList.OrderBy(x => x.name))
                {
                    _lots.Add((LotViewModel)lot);
                }

            _lotLocations = new ObservableCollection<LotLocationViewModel>();
            var lotLocationList = new ObservableCollection<LotLocation>(db.LotLocation.ToList());            

            foreach (var lotLocation in lotLocationList)
            {
                _lotLocations.Add((LotLocationViewModel)lotLocation);
            }
            LotLocations.OrderBy(x => x.LotName);          

            _sellers = new ObservableCollection<SellerViewModel>();
            var sellerList = new ObservableCollection<Seller>(db.Seller.ToList());            

            foreach (var seller in sellerList.OrderBy(x => x.name).ThenBy(x => x.surname))
            {
                _sellers.Add((SellerViewModel)seller);
            }

            _sales = new ObservableCollection<SaleViewModel>();
            var orderList = new ObservableCollection<Sale>(db.Sale.ToList());

            foreach (var order in orderList.OrderByDescending(x => x.date))
            {
                _sales.Add((SaleViewModel)order);
            }
        }

    }
}
