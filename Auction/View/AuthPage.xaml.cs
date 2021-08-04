using Auction.Model;
using Auction.ViewModel;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (LoginBox.Text == null || LoginBox.Text == "" || Passbox.Password == "" || Passbox.Password == null)
            {
                MessageBox.Show("Enter all fields!", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            Employee employee = (new auctionDBEntities()).Employee.FirstOrDefault(x => x.Auth.login == LoginBox.Text.Trim());

            if (employee == null)
            {
                MessageBox.Show("Incorrect login!", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (employee.Auth.password != Passbox.Password.Trim())
            {
                MessageBox.Show("Incorrect password!", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
                App.Current.Resources.Add("User", employee);
                mainWindow.DataContext = new MainWindowViewModel(employee.Auth.login);
                mainWindow.UserPanel.Visibility = Visibility.Visible;
                mainWindow.MainFrame.Navigate(new MainMenuPage());
            }
        }
    }
}
