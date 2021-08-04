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
    class LotViewModel: BaseViewModel
    {
        private int _lotId;
        public int LotId
        {
            get => _lotId;

            set
            {
                _lotId = value;
                OnPropertyChanged(nameof(LotId));
            }
        }

        private string _name;
        public string Name
        {
            get => _name;

            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
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

        private int? _minPrice;
        public int? MinPrice
        {
            get => _minPrice;

            set
            {
                _minPrice = value;
                OnPropertyChanged(nameof(MinPrice));
            }
        }

        private int? _preferredPrice;
        public int? PreferredPrice
        {
            get => _preferredPrice;

            set
            {
                _preferredPrice = value;
                OnPropertyChanged(nameof(PreferredPrice));
            }
        }

        private string _type;
        public string Type
        {
            get => _type;

            set
            {
                _type = value;
                OnPropertyChanged(nameof(Type));
            }
        }

        public string _location;
        public string Location
        {
            get => _location;

            set
            {
                _location = value;
                OnPropertyChanged(nameof(Location));
            }
        }

        private DateTime? _startDate;
        public DateTime? StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }

        private DateTime? _plannedCloseDate;
        public DateTime? PlannedCloseDate
        {
            get => _plannedCloseDate;
            set
            {
                _plannedCloseDate = value;
                OnPropertyChanged(nameof(PlannedCloseDate));
            }
        }

        public RelayCommand UpdateCommand => new RelayCommand(obj =>
        {
            if (!MinPrice.HasValue || !PreferredPrice.HasValue || Name == "" || Name == null || Seller == "" 
                    || Type == "" || Seller == null || Type == null || Location == "" || Location == null)
            {
                MessageBox.Show("Enter all fields!", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (MinPrice > PreferredPrice)
            {
                MessageBox.Show("Preferred price must be grater than minimal price!", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (StartDate > PlannedCloseDate)
            {
                MessageBox.Show("Start date must be grater than close date!", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            auctionDBEntities db = new auctionDBEntities();            
            var sellerInfo = Seller.Split(' ');
            var name = sellerInfo[0];
            var surname = sellerInfo[1];
            var seller = db.Seller.FirstOrDefault(x => x.name == name && x.surname == surname);            
            var locationId = db.LotLocation.Select(x => new
            {
                Id = x.location_id,
                Location = (x.city + " " + x.street + " " + x.house_number)
            }).FirstOrDefault(x => x.Location == Location).Id;
            var location = db.LotLocation.FirstOrDefault(x => x.location_id == locationId);
            var updatedLot = db.Lot.FirstOrDefault(x => x.lot_id == LotId);
            updatedLot.Seller = seller;
            updatedLot.seller_id = seller.seller_id;
            updatedLot.min_price = MinPrice;
            updatedLot.preferred_price = PreferredPrice;
            updatedLot.name = Name;
            updatedLot.LotLocation = location;
            updatedLot.location_id = location.location_id;
            updatedLot.LotType = db.LotType.SingleOrDefault(x => x.type == Type);
            updatedLot.type_id = db.LotType.SingleOrDefault(x => x.type == Type).type_id;
            db.Lot.Attach(updatedLot);
            db.Entry(updatedLot).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
            mainWindow.MainFrame.GoBack();
        });

        public RelayCommand AddCommand => new RelayCommand(obj =>
        {
            if (!MinPrice.HasValue || !PreferredPrice.HasValue || Name == "" || Name == null || Seller == ""
                 || Type == "" || Seller == null || Type == null || Location == "" || Location == null)
            {
                MessageBox.Show("Enter all fields!", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if(MinPrice > PreferredPrice)
            {
                MessageBox.Show("Preferred price must be grater than minimal price!", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (StartDate > PlannedCloseDate)
            {
                MessageBox.Show("Start date must be grater than close date!", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            auctionDBEntities db = new auctionDBEntities();
            var sellerInfo = Seller.Split(' ');
            var name = sellerInfo[0];
            var surname = sellerInfo[1];
            var seller = db.Seller.FirstOrDefault(x => x.name == name && x.surname == surname);
            var locationId = db.LotLocation.Select(x => new
            {
                Id = x.location_id,
                Location = (x.city + " " + x.street + " " + x.house_number)
            }).FirstOrDefault(x => x.Location == Location).Id;
            var location = db.LotLocation.FirstOrDefault(x => x.location_id == locationId);
            var newLot = new Lot()
            {
                lot_id = db.Lot.Max(x => x.lot_id) + 1,
                name = Name,
                LotLocation = location,
                location_id = location.location_id,
                Seller = seller,
                seller_id = seller.seller_id,
                min_price = MinPrice,
                preferred_price = PreferredPrice,
                LotType = db.LotType.SingleOrDefault(x => x.type == Type),
                type_id = db.LotType.SingleOrDefault(x => x.type == Type).type_id,
                start_day = StartDate,
                planned_close_day = PlannedCloseDate,
                create_date = DateTime.Today,
                update_date = DateTime.Today,
                Bid = null,
                Sale = null
            };
            db.Lot.Add(newLot);
            db.SaveChanges();
            MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
            mainWindow.MainFrame.Navigate(new LotsPage());
        });

        public RelayCommand BackCommand => new RelayCommand(obj =>
        {

            MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
            mainWindow.MainFrame.GoBack();
        });

        public static explicit operator LotViewModel(Lot lot)
        {
            return new LotViewModel()
            {
                LotId = lot.lot_id,
                Name = lot.name,
                Seller = lot.Seller.name + " " + lot.Seller.surname,
                MinPrice = lot.min_price,
                PreferredPrice = lot.preferred_price,
                Type = lot.LotType.type,
                Location = lot.LotLocation.city + " " + lot.LotLocation.street + " " + lot.LotLocation.house_number,
                StartDate = lot.start_day,
                PlannedCloseDate = lot.planned_close_day
            };
        }

        public LotViewModel()
        {
            StartDate = DateTime.Today;
            PlannedCloseDate = DateTime.Today;
        }
    }
}
