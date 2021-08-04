using Auction.Model;
using Auction.View.InfoPages;
using Auction.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Auction.View
{
    /// <summary>
    /// Interaction logic for SellersPage.xaml
    /// </summary>
    public partial class SellersPage : Page
    {
        public SellersPage()
        {
            InitializeComponent();
            DataContext = new DataViewModel();
        }
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            SellerViewModel selectedSeller = (SellerViewModel)SellerDataGrid.SelectedItem;
            MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
            var page = new SellerInfoPage();
            page.DataContext = selectedSeller;
            page.CaptionLabel.Content = "Seller update";
            page.SaveButton.Command = selectedSeller.UpdateCommand;
            mainWindow.MainFrame.Navigate(page);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure to Delete selected record from DB?", "Message", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                var selectedSeller = (SellerViewModel)SellerDataGrid.SelectedItem;
                auctionDBEntities db = new auctionDBEntities();
                var removeSeller = db.Seller.FirstOrDefault(x => x.seller_id == selectedSeller.SellerId);
                db.Seller.Remove(removeSeller);
                db.SaveChanges();
                ((DataViewModel)DataContext).Sellers.Remove(selectedSeller);
                SellerDataGrid.ItemsSource = ((DataViewModel)DataContext).Sellers;
                TextBox_KeyUp(this, null);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
            var page = new SellerInfoPage();
            var context = new SellerViewModel();
            page.DataContext = context;
            page.CaptionLabel.Content = "Seller add";
            page.SaveButton.Command = context.AddCommand;
            mainWindow.MainFrame.Navigate(page);
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            auctionDBEntities db = new auctionDBEntities();
            var lotList = ((DataViewModel)this.DataContext).Sellers;
            var nameText = NameTextBox.Text.Trim();
            var surnameText = SurnameTextBox.Text.Trim();
            var numberText = NumberTextBox.Text.Trim();
            var emailText = EmailTextBox.Text.Trim();
            var filtered = lotList.Where(x => x.Name.StartsWith(nameText) && x.Surname.StartsWith(surnameText)
                        && x.Number.StartsWith(numberText) && x.Email.StartsWith(emailText));
            SellerDataGrid.ItemsSource = filtered;
        }
    }
}
