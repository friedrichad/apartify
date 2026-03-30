using Apartify.BLL;
using Apartify.DAL;
using Apartify.Models;
using System;
using System.Linq;
using System.Windows;

namespace Apartify.Views.resident
{
    public partial class RequestWindow : Window
    {
        private readonly UserAccount _user;
        private readonly IRequestBll _requestBll;

        public RequestWindow(UserAccount user)
        {
            InitializeComponent();
            _user = user;
            var context = new ApartifyContext();
            _requestBll = new RequestBll(new RequestDal(context));
            LoadRequests();
        }

        private void LoadRequests()
        {
            var context = new ApartifyContext();

            var resident = context.Residents
                .FirstOrDefault(r => r.UserId == _user.UserId);

            if (resident == null) return;

            var requests = context.Requests
                .Where(r => r.ResidentId == resident.ResidentId)
                .OrderByDescending(r => r.RequestDate)
                .Select(r => new
                {
                    r.RequestDate,
                    ApartmentNumber = r.Apartment != null ? r.Apartment.Number : "N/A",
                    r.Description,
                    r.Status
                })
                .ToList();

            lvRequests.ItemsSource = requests;

            // Load apartments for this resident into combo box
            var apartments = context.Contracts
                .Where(c => c.ResidentId == resident.ResidentId)
                .Select(c => c.Apartment)
                .Where(a => a != null)
                .Select(a => new { a.ApartmentId, Display = (a.Number ?? "N/A") + " - " + (a.Building != null ? a.Building.Name : "N/A") })
                .ToList();

            cbApartments.ItemsSource = apartments;
            cbApartments.DisplayMemberPath = "Display";
            cbApartments.SelectedValuePath = "ApartmentId";
            if (apartments.Count > 0) cbApartments.SelectedIndex = 0;
        }

        private void CreateRequest_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbDescription.Text))
            {
                MessageBox.Show("Please enter description.");
                return;
            }

            var context = new ApartifyContext();

            var resident = context.Residents
                .FirstOrDefault(r => r.UserId == _user.UserId);

            if (resident == null)
            {
                MessageBox.Show("Resident not found.");
                return;
            }

            if (cbApartments.SelectedItem == null)
            {
                MessageBox.Show("Please select an apartment.");
                return;
            }

            var apartmentId = (int)cbApartments.SelectedValue;

            var request = new Request
            {
                ResidentId = resident.ResidentId,
                ApartmentId = apartmentId,
                Description = tbDescription.Text.Trim(),
                RequestDate = DateTime.Now,
                Status = "Pending"
            };

            context.Requests.Add(request);
            context.SaveChanges();

            MessageBox.Show("Request created.");

            tbDescription.Clear();
            LoadRequests();
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
