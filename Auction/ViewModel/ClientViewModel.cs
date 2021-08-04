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
    class ClientViewModel: BaseViewModel
    {
        private int _clientId;
        public int ClientId
        {
            get => _clientId;
            set
            {
                _clientId = value;
                OnPropertyChanged(nameof(ClientId));
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
            Client updatedClient = db.Client.FirstOrDefault(x => x.client_id == ClientId);
            updatedClient.name = Name;
            updatedClient.surname = Surname;
            updatedClient.number = Number;
            updatedClient.email = Email;
            db.Client.Attach(updatedClient);
            db.Entry(updatedClient).State = System.Data.Entity.EntityState.Modified;
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
            var newClient = new Client()
            {
                 client_id = db.Client.Max(x => x.client_id) + 1,
                 name = Name,
                 surname = Surname,
                 email = Email,
                 number = Number
            };
            db.Client.Add(newClient);
            db.SaveChanges();
            MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
            mainWindow.MainFrame.Navigate(new ClientsPage());
        });

        public RelayCommand BackCommand => new RelayCommand(obj =>
        {
            MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
            mainWindow.MainFrame.GoBack();
        });

        public static explicit operator ClientViewModel(Client client)
        {
            return new ClientViewModel()
            {
                ClientId = client.client_id,
                Name = client.name,
                Surname = client.surname,
                Number = client.number,
                Email = client.email
            };
        }
    }
}
