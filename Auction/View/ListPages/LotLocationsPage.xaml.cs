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
    /// Interaction logic for LotLocationsPage.xaml
    /// </summary>
    public partial class LotLocationsPage : Page
    {
        public LotLocationsPage()
        {
            InitializeComponent();
            DataContext = new DataViewModel();
        }
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedLocation = (LotLocationViewModel)LocationDataGrid.SelectedItem;
            MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
            var page = new LotLocationInfoPage();
            page.DataContext = selectedLocation;
            page.SaveButton.Command = selectedLocation.UpdateCommand;
            page.CaptionLabel.Content = "Location update";
            mainWindow.MainFrame.Navigate(page);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure to Delete selected record from DB?", "Message", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                var selectedLocation = (LotLocationViewModel)LocationDataGrid.SelectedItem;                
                auctionDBEntities db = new auctionDBEntities();
                var removeLocation = db.LotLocation.FirstOrDefault(x => x.location_id == selectedLocation.LocationId);                
                db.LotLocation.Remove(removeLocation);
                db.SaveChanges();
                ((DataViewModel)DataContext).LotLocations.Remove(selectedLocation);
                LocationDataGrid.ItemsSource = ((DataViewModel)DataContext).LotLocations;
                TextBox_KeyUp(this, null);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
            var page = new LotLocationInfoPage();
            var context = new LotLocationViewModel();
            page.DataContext = context;
            page.CaptionLabel.Content = "Location add";
            page.LotNamePanel.Visibility = Visibility.Collapsed;
            page.SaveButton.Command = context.AddCommand;
            mainWindow.MainFrame.Navigate(page);
        }
        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            auctionDBEntities db = new auctionDBEntities();
            var lotList = ((DataViewModel)this.DataContext).LotLocations;
            var cityText = CityTextBox.Text.Trim();
            var streetText = StreetTextBox.Text.Trim();
            var numberText = NumberTextBox.Text.Trim();            
            var filtered = lotList.Where(x => x.City.StartsWith(cityText) && x.Street.StartsWith(streetText)
                        && x.HouseNumber.ToString().StartsWith(numberText));
            LocationDataGrid.ItemsSource = filtered;
        }
    }
}
