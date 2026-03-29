using Apartify.DAL;
using Apartify.Models;
using System.Linq;
using System.Windows;

namespace Apartify.Views.resident
{
    public partial class ApartmentWindow : Window
    {
        private readonly UserAccount _user;

        public ApartmentWindow(UserAccount user)
        {
            InitializeComponent();
            _user = user;
            LoadApartment();
        }

        private void LoadApartment()
        {
            using (var context = new ApartifyContext())
            {
                var resident = context.Residents
                    .FirstOrDefault(r => r.UserId == _user.UserId);

                if (resident == null)
                {
                    MessageBox.Show("Resident not found");
                    return;
                }

                var contract = context.Contracts
                    .FirstOrDefault(c => c.ResidentId == resident.ResidentId);

                if (contract == null)
                {
                    MessageBox.Show("No apartment assigned");
                    return;
                }

                var apartment = context.Apartments
                   .Where(a => a.ApartmentId == contract.ApartmentId)
                    .Select(a => new
       {
                    a.ApartmentId,
                    Building = a.Building != null ? a.Building.Name : "N/A",
                    Number = a.Number ?? "N/A",
                    Floor = a.Floor ?? 0,
                   Area = a.Area ?? 0
                })
       .FirstOrDefault();

                if (apartment == null)
                {
                    MessageBox.Show("Apartment details not found");
                    return;
                }

                lvApartment.ItemsSource = new[] { apartment };
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}