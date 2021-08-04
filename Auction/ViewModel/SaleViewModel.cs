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
    class SaleViewModel: BaseViewModel
    {
        private int _orderId;
        public int OrderId
        {
            get => _orderId;

            set
            {
                _orderId = value;
                OnPropertyChanged(nameof(OrderId));
            }
        }

        private string _lot;
        public string LotName
        {
            get => _lot;

            set
            {
                _lot = value;
                OnPropertyChanged(nameof(LotName));
            }
        }

        private string _seller;
        public string Seller
        {
            get => _seller;

            set
            {
                _seller = value;
                OnPropertyChanged(nameof(Seller));
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

        private int? _price;
        public int? Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged(nameof(Price));
            }
        }

        private string _status;
        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        private  DateTime? _date;
        public DateTime? Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        public RelayCommand UpdateCommand => new RelayCommand(obj =>
        {
            if (!Price.HasValue || LotName == "" || Client == "" || LotName == null || Client == null
                || Seller == "" || Status == "" || Seller == null || Status == null)
            {
                MessageBox.Show("Enter all fields!", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            auctionDBEntities db = new auctionDBEntities();
            var sellerInfo = Seller.Split(' ');
            var name = sellerInfo[0];
            var surname = sellerInfo[1];
            var seller = db.Seller.FirstOrDefault(x => x.name == name && x.surname == surname);
            var clientInfo = Client.Split(' ');
            name = clientInfo[0];
            surname = clientInfo[1];
            var client = db.Client.FirstOrDefault(x => x.name == name && x.surname == surname);
            var lot = db.Lot.FirstOrDefault(x => x.name == LotName);
            var updatedSale = db.Sale.FirstOrDefault(x => x.order_id == OrderId);
            updatedSale.Seller = seller;
            updatedSale.seller_id = seller.seller_id;
            updatedSale.Client = client;
            updatedSale.client_id = client.client_id;
            updatedSale.Lot = lot;
            updatedSale.lot_id = lot.lot_id;
            updatedSale.OrderStatus = db.OrderStatus.SingleOrDefault(x => x.status == Status);
            updatedSale.status_id = db.OrderStatus.SingleOrDefault(x => x.status == Status).status_id;
            updatedSale.price = Price;
            updatedSale.date = Date;
            db.Sale.Attach(updatedSale);
            db.Entry(updatedSale).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
            mainWindow.MainFrame.GoBack();
        });

        public RelayCommand AddCommand => new RelayCommand(obj =>
        {
            if (!Price.HasValue || LotName == "" || Client == "" || LotName == null || Client == null
                || Seller == "" || Status == "" || Seller == null || Status == null)
            {
                MessageBox.Show("Enter all fields!", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            auctionDBEntities db = new auctionDBEntities();
            var sellerInfo = Seller.Split(' ');
            var name = sellerInfo[0];
            var surname = sellerInfo[1];
            var seller = db.Seller.FirstOrDefault(x => x.name == name && x.surname == surname);
            var clientInfo = Client.Split(' ');
            name = clientInfo[0];
            surname = clientInfo[1];
            var client = db.Client.FirstOrDefault(x => x.name == name && x.surname == surname);
            var lot = db.Lot.FirstOrDefault(x => x.name == LotName);
            var newSale = new Sale()
            {
                order_id = db.Sale.Max(x => x.order_id) + 1,
                Seller = seller,
                seller_id = seller.seller_id,
                Client = client,
                client_id = client.client_id,
                Lot = lot,
                lot_id = lot.lot_id,
                date = Date,
                price = Price,
                OrderStatus = db.OrderStatus.SingleOrDefault(x => x.status == Status),
                status_id = db.OrderStatus.SingleOrDefault(x => x.status == Status).status_id
            };
            db.Sale.Add(newSale);
            db.SaveChanges();
            MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
            mainWindow.MainFrame.Navigate(new OrdersPage());
        });

        public RelayCommand BackCommand => new RelayCommand(obj =>
        {
            MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
            mainWindow.MainFrame.GoBack();
        });
        
        public static explicit operator SaleViewModel(Sale sale)
        {
            return new SaleViewModel()
            {
                OrderId = sale.order_id,
                LotName = sale.Lot.name,
                Seller = sale.Seller.name + " " + sale.Seller.surname,
                Client = sale.Client.name + " " + sale.Client.surname,
                Price = sale.price,
                Status = sale.OrderStatus.status,
                Date = sale.date
            };
        }

        public SaleViewModel()
        {
            Date = DateTime.Today;
            Seller = "None";
        }

    }
}
