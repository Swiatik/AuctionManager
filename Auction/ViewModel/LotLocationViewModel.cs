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
    class LotLocationViewModel: BaseViewModel
    {
        private int _locationId;
        public int LocationId
        {
            get => _locationId;

            set
            {
                _locationId = value;
                OnPropertyChanged(nameof(LocationId));
            }
        }

        private string _city;
        public string City
        {
            get => _city;

            set
            {
                _city = value;
                OnPropertyChanged(nameof(City));
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

        private string _street;
        public string Street
        {
            get => _street;

            set
            {
                _street = value;
                OnPropertyChanged(nameof(Street));
            }
        }

        private int? _houseNumber;
        public int? HouseNumber
        {
            get => _houseNumber;

            set
            {
                _houseNumber = value;
                OnPropertyChanged(nameof(HouseNumber));
            }
        }

        public RelayCommand UpdateCommand => new RelayCommand(obj =>
        {
            if (City == "" || Street == "" || !HouseNumber.HasValue || City == null || Street == null)
            {
                MessageBox.Show("Enter all fields!", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            auctionDBEntities db = new auctionDBEntities();
            var lot = db.Lot.FirstOrDefault(x => x.name == LotName);           
            var updatedLocation = db.LotLocation.FirstOrDefault(x => x.location_id == LocationId);                        
            updatedLocation.city = City;
            updatedLocation.street = Street;
            updatedLocation.house_number = HouseNumber;
            db.LotLocation.Attach(updatedLocation);
            db.Entry(updatedLocation).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
            mainWindow.MainFrame.GoBack();
        });

        public RelayCommand AddCommand => new RelayCommand(obj =>
        {
            if (City == "" || Street == "" || !HouseNumber.HasValue || City == null || Street == null)
            {
                MessageBox.Show("Enter all fields!", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            auctionDBEntities db = new auctionDBEntities();
            var newLocation = new LotLocation()
            {
                location_id = db.LotLocation.Max(x => x.location_id) + 1,
                city = City,
                street = Street,
                house_number = HouseNumber
            };
            db.LotLocation.Add(newLocation);
            db.SaveChanges();
            MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
            mainWindow.MainFrame.Navigate(new LotLocationsPage());
        });

        public RelayCommand BackCommand => new RelayCommand(obj =>
        {
            MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
            mainWindow.MainFrame.GoBack();
        });

        public static explicit operator LotLocationViewModel(LotLocation location)
        {
            string lotName = "No lot";
            Lot lot = location.Lot.FirstOrDefault(x => x.location_id == location.location_id);
            if (lot != null)
                lotName = lot.name;

            return new LotLocationViewModel()
            {
                LocationId = location.location_id,
                LotName = lotName,
                City = location.city,
                Street = location.street,
                HouseNumber = location.house_number
            };
        }

    }
}
