using System;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Apartify.Models;

namespace Apartify.Views.staff
{
    public partial class RequestDetailsStaff : Window
    {
        private readonly int _requestId;

        public RequestDetailsStaff(int requestId)
        {
            InitializeComponent();
            _requestId = requestId;

            LoadDetails();
        }

        private void LoadDetails()
        {
            using var ctx = new ApartifyContext();
            var r = ctx.Requests
                .Include(x => x.Resident)
                .Include(x => x.Apartment)
                .ThenInclude(a => a.Building)
                .FirstOrDefault(x => x.RequestId == _requestId);

            if (r == null)
            {
                MessageBox.Show("Request not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return;
            }

            TxtDescription.Text = r.Description ?? string.Empty;
            TxtApartment.Text = r.Apartment != null ? r.Apartment.Number ?? string.Empty : string.Empty;
            TxtBuilding.Text = r.Apartment != null && r.Apartment.Building != null ? r.Apartment.Building.Name ?? string.Empty : string.Empty;
            TxtResident.Text = r.Resident != null ? r.Resident.FullName ?? string.Empty : string.Empty;
            TxtResidentContact.Text = r.Resident != null ? r.Resident.Phone ?? string.Empty : string.Empty;
            TxtStatus.Text = r.Status ?? string.Empty;
            TxtDate.Text = r.RequestDate.HasValue ? r.RequestDate.Value.ToString("g") : string.Empty;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
