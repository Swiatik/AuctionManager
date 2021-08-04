using Auction.Commands;
using Auction.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Auction.ViewModel
{
    class MainWindowViewModel: BaseViewModel
    {
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

        public RelayCommand LogoutCommand => new RelayCommand(obj =>
        {
            MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
            App.Current.Resources.Remove("User");
            mainWindow.UserPanel.Visibility = Visibility.Hidden;
            mainWindow.MainFrame.Navigate(new AuthPage());
        });

        public MainWindowViewModel(string user)
        {
            User = user;
        }
    }
}
