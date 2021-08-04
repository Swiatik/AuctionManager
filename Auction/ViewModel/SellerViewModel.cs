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
    class SellerViewModel: BaseViewModel
    {
        private int _sellerId;
        public int SellerId
        {
            get => _sellerId;
            set
            {
                _sellerId = value;
                OnPropertyChanged(nameof(SellerId));
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

        private string _surname;
        public string Surname
        {
            get => _surname;
            set
            {
                _surname = value;
                OnPropertyChanged(nameof(Surname));
            }
        }

        private string _number;
        public string Number
        {
            get => _number;
            set
            {
                _number = value;
                OnPropertyChanged(nameof(Number));
            }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public RelayCommand UpdateCommand => new RelayCommand(obj =>
        {
            if (Name == "" || Surname == "" || Email == "" || Number == "" || Name == null
                 || Surname == null || Email == null || Number == null)
            {
                MessageBox.Show("Enter all fields!", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            auctionDBEntities db = new auctionDBEntities();
            Seller updatedSeller = db.Seller.FirstOrDefault(x => x.seller_id == SellerId);
            updatedSeller.name = Name;
            updatedSeller.surname = Surname;
            updatedSeller.number = Number;
            updatedSeller.email = Email;
            db.Seller.Attach(updatedSeller);
            db.Entry(updatedSeller).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
            mainWindow.MainFrame.GoBack();
        });

        public RelayCommand AddCommand => new RelayCommand(obj =>
        {
            if (Name == "" || Surname == "" || Email == "" || Number == "" || Name == null
                || Surname == null || Email == null || Number == null)
            {
                MessageBox.Show("Enter all fields!", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            auctionDBEntities db = new auctionDBEntities();
            var newSeller = new Seller()
            {
                seller_id = db.Seller.Max(x => x.seller_id) + 1,
                name = Name,
                surname = Surname,
                email = Email,
                number = Number
            };
            db.Seller.Add(newSeller);
            db.SaveChanges();
            MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
            mainWindow.MainFrame.Navigate(new SellersPage());
        });

        public RelayCommand BackCommand => new RelayCommand(obj =>
        {
            MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
            mainWindow.MainFrame.GoBack();
        });
        public static explicit operator SellerViewModel(Seller seller)
        {
            return new SellerViewModel()
            {
                SellerId = seller.seller_id,
                Name = seller.name,
                Surname = seller.surname,
                Number = seller.number,
                Email = seller.email
            };
        }
    }
}
