using Apartify.DAL;
using Apartify.Models;
using System.Linq;
using System.Windows;

namespace Apartify.Views.resident
{
    public partial class ContractWindow : Window
    {
        public bool IsValid { get; private set; }
        private readonly UserAccount _user;

        public ContractWindow(UserAccount user)
        {
            InitializeComponent();
            _user = user;
            LoadContracts();
        }

        private void LoadContracts()
        {
            using (var context = new ApartifyContext())
            {
                var resident = context.Residents
                    .FirstOrDefault(r => r.UserId == _user.UserId);

                if (resident == null)
                {
                    MessageBox.Show("Resident profile not found.");
                    IsValid = false;
                    return;
                }

                IsValid = true;
                var contracts = context.Contracts
                    .Where(c => c.ResidentId == resident.ResidentId)
                    .Select(c => new
                    {
                        c.ContractId,
                        ApartmentNumber = c.Apartment.Number,
                        BuildingName = c.Apartment.Building.Name,
                        c.StartDate,
                        c.EndDate
                    })
                    .ToList();

                lvContracts.ItemsSource = contracts;
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}