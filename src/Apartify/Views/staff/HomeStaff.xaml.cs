using System.Windows;

namespace Apartify.Views.staff
{
    /// <summary>
    /// Interaction logic for HomeStaff.xaml
    /// </summary>
    public partial class HomeStaff : Window
    {
        public HomeStaff()
        {
            InitializeComponent();
            BtnRequests.Click += BtnRequests_Click;
            BtnPayments.Click += BtnPayments_Click;
            BtnLogout.Click += BtnLogout_Click;
        }

        private void BtnRequests_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // open WPF requests window for staff
                var win = new RequestsStaff();
                win.Owner = this;
                win.ShowDialog();
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show($"Cannot open Requests window:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnPayments_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var win = new PaymentsStaff();
                win.Owner = this;
                win.ShowDialog();
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show($"Cannot open Payments window:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            // Close the staff window to logout
            this.Close();
        }
    }
}
