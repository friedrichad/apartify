using Apartify.BLL;
using Apartify.DAL;
using Apartify.Models;
using System.Linq;
using System.Windows;

namespace Apartify.Views
{
    public partial class ApartmentWindow : Window
    {
        private readonly UserAccount _user;
        private readonly IResidentBll _residentBll;
        private readonly IContractBll _contractBll;

        public ApartmentWindow(UserAccount user)
        {
            InitializeComponent();
            _user = user;
            var context = new ApartifyContext();
            _residentBll = new ResidentBll(new ResidentDal(context));
            _contractBll = new ContractBll(new ContractDal(context));
            LoadResidentInfo();
        }

        private void LoadResidentInfo()
        {
            var resident = _residentBll.GetList().FirstOrDefault(r => r.UserId == _user.UserId);
            if (resident != null)
            {
                tbResidentName.Text = resident.FullName;
                
                // Get active contract
                var contracts = _contractBll.GetList().Where(c => c.ResidentId == resident.ResidentId).ToList();
                var activeContract = contracts.FirstOrDefault();
                
                if (activeContract != null)
                {
                    tbApartmentNo.Text = activeContract.Apartment?.Number ?? "N/A";
                    tbBuildingName.Text = activeContract.Apartment?.Building?.Name ?? "N/A";
                    tbContractPeriod.Text = $"{activeContract.StartDate?.ToString("MM/dd/yyyy")} to {activeContract.EndDate?.ToString("MM/dd/yyyy")}";
                }
                else
                {
                    tbApartmentNo.Text = "No active contract";
                    tbContractPeriod.Text = "N/A";
                }
            }
            else
            {
                MessageBox.Show("Resident information not found.");
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            MainWindow login = new MainWindow();
            login.Show();
            this.Close();
        }
    }
}
