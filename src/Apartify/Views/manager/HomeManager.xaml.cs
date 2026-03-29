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

        private void btnResident(object sender, EventArgs e)
        {
            ResidentManagementWindow residentManagement = new ResidentManagementWindow();
            residentManagement.Show();
        }
    }
}
