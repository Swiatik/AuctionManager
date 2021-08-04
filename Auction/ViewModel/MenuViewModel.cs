using Auction.Commands;
using Auction.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Auction.ViewModel
{
    class MenuViewModel:BaseViewModel
    {     
        public RelayCommand LotRedirectCommand => new RelayCommand(obj =>
        {                                                
            MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
            mainWindow.MainFrame.Navigate(new LotsPage());
        });
        public RelayCommand ClientRedirectCommand => new RelayCommand(obj =>
        {
            MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
            mainWindow.MainFrame.Navigate(new ClientsPage());
        });
        public RelayCommand SellerRedirectCommand => new RelayCommand(obj =>
        {
            MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
            mainWindow.MainFrame.Navigate(new SellersPage());
        });
        public RelayCommand LocationRedirectCommand => new RelayCommand(obj =>
        {
            MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
            mainWindow.MainFrame.Navigate(new LotLocationsPage());
        });
        public RelayCommand SaleRedirectCommand => new RelayCommand(obj =>
        {
            MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
            mainWindow.MainFrame.Navigate(new OrdersPage());
        });
        public RelayCommand BidRedirectCommand => new RelayCommand(obj =>
        {
            MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
            mainWindow.MainFrame.Navigate(new AuctionsPage());
        });

    }
}
