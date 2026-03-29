using System.Windows;

namespace Apartify.Views.manager
{
    /// <summary>
    /// Interaction logic for HomeManager.xaml
    /// </summary>
    public partial class HomeManager : Window
    {
        public HomeManager()
        {
            InitializeComponent();
        }

        private void btnBuilding(object sender, RoutedEventArgs e)
        {
            BuildingManagementWindow buildingManagement = new BuildingManagementWindow();
            buildingManagement.Show();
        }

        private void btnApartment(object sender, RoutedEventArgs e)
        {
            ApartmentManagementWindow apartmentManagement = new ApartmentManagementWindow();
            apartmentManagement.Show();
        }

        private void btnResident(object sender, RoutedEventArgs e)
        {
            ResidentManagementWindow residentManagement = new ResidentManagementWindow();
            residentManagement.Show();
        }

        private void btnRequest(object sender, RoutedEventArgs e)
        {
            RequestManagementWindow requestManagement = new RequestManagementWindow();
            requestManagement.Show();
        }

        private void btnContract(object sender, RoutedEventArgs e)
        {
            ContractManagementWindow contractManagement = new ContractManagementWindow();
            contractManagement.Show();
        }

        private void btnClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
