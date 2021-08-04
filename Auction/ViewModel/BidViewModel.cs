using Auction.Commands;
using Auction.Model;
using Auction.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Auction.ViewModel
{
    class BidViewModel : BaseViewModel
    {
        private int _auctionId;
        public int AuctionId
        {
            get => _auctionId;

            set
            {
                _auctionId = value;
                OnPropertyChanged(nameof(AuctionId));
            }
        }

        private string _lotName;
        public string LotName
        {
            get => _lotName;

            set
            {
                _lotName = value;
                OnPropertyChanged(nameof(LotName));
            }
        }

        private string _client;
        public string Client
        {
            get => _client;
            set
            {
                _client = value;
                OnPropertyChanged(nameof(Client));
            }
        }

        private int? _bid;
        public int? Bid
        {
            get => _bid;
            set
            {
                _bid = value;
                OnPropertyChanged(nameof(Bid));
            }
        }

        private DateTime? _time;
        public DateTime? Time
        {
            get => _time;
            set
            {
                _time = value;
                OnPropertyChanged(nameof(Time));
            }
        }
        public RelayCommand UpdateCommand => new RelayCommand(obj =>
        {
            if (!Bid.HasValue || LotName == "" || Client == "" || LotName == null || Client == null)
            {
                MessageBox.Show("Enter all fields!", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            auctionDBEntities db = new auctionDBEntities();
            var lot = db.Lot.FirstOrDefault(x => x.name == LotName);
            var clientInfo = Client.Split(' ');
            var name = clientInfo[0];
            var surname = clientInfo[1];
            var client = db.Client.FirstOrDefault(x => x.name == name && x.surname == surname);
            Bid updatedBid = db.Bid.FirstOrDefault(x => x.auction_id == AuctionId);
            updatedBid.Client = client;
            updatedBid.client_id = client.client_id;
            updatedBid.Lot = lot;
            updatedBid.lot_id = lot.lot_id;
            updatedBid.bid1 = Bid;
            updatedBid.time = Time;
            db.Bid.Attach(updatedBid);
            db.Entry(updatedBid).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
            mainWindow.MainFrame.GoBack();
        });

        public RelayCommand AddCommand => new RelayCommand(obj =>
        {
            if (!Bid.HasValue || LotName == "" || Client == "" || LotName == null || Client == null)
            {
                MessageBox.Show("Enter all fields!", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
                
            auctionDBEntities db = new auctionDBEntities();
            var lot = db.Lot.FirstOrDefault(x => x.name == LotName);
            var clientInfo = Client.Split(' ');
            var name = clientInfo[0];
            var surname = clientInfo[1];
            var client = db.Client.FirstOrDefault(x => x.name == name && x.surname == surname);
            var newBid = new Bid()
            {
                auction_id = db.Bid.Max(x => x.auction_id) + 1,
                bid1 = Bid,
                Client = client,
                client_id = client.client_id,
                Lot = lot,
                lot_id = lot.lot_id,
                time = Time                
            };
            db.Bid.Add(newBid);
            db.SaveChanges();
            MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
            mainWindow.MainFrame.Navigate(new AuctionsPage());
        });

        public RelayCommand BackCommand => new RelayCommand(obj =>
        {
            MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
            mainWindow.MainFrame.GoBack();
        });

        public static explicit operator BidViewModel(Bid bid)
        {          
            return new BidViewModel()
            {
                AuctionId = bid.auction_id,
                LotName = bid.Lot.name,
                Client = bid.Client.name + " " + bid.Client.surname,
                Bid = bid.bid1,
                Time = bid.time,
            };
        }

        public BidViewModel()
        {
            Time = DateTime.Now;
        }
    }
}
