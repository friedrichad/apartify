using System.Windows;
using Apartify.Models;

namespace Apartify.Views.resident
{

    public partial class HomeResident : Window
    {
        private readonly UserAccount _user;

        public HomeResident(UserAccount user)
        {
            InitializeComponent();
            _user = user;
        }
        private void ApartmentInfo_Click(object sender, RoutedEventArgs e)
        {
            var win = new ApartmentWindow(_user);
            win.ShowDialog();
        }

        private void Contract_Click(object sender, RoutedEventArgs e)
        {
            var win = new ContractWindow(_user);
            win.Owner = this;
            win.ShowDialog();
        }

        private void Maintenance_Click(object sender, RoutedEventArgs e)
        {
            var win = new RequestWindow(_user);
            win.Owner = this;
            win.ShowDialog();
        }
        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            var win = new ProfileWindow(_user);
            win.Owner = this;
            win.ShowDialog();
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
