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
        private readonly bool _isEdit;

        public RequestDetailsStaff(int requestId, bool isEdit = false)
        {
            InitializeComponent();
            _requestId = requestId;
            _isEdit = isEdit;

            BtnSave.Click += BtnSave_Click;
            BtnBack.Click += BtnBack_Click;

            LoadDetails();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!_isEdit)
            {
                MessageBox.Show("Not in edit mode.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            using var ctx = new ApartifyContext();
            var entity = ctx.Requests.Find(_requestId);
            if (entity == null)
            {
                MessageBox.Show("Request not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Update editable fields except Status
            entity.Description = TxtDescription.Text ?? string.Empty;
            entity.ApartmentId = CbApartment.SelectedValue as int? ?? (int?)null;
            entity.ResidentId = CbResident.SelectedValue as int? ?? (int?)null;
            entity.RequestDate = DpRequestDate.SelectedDate.HasValue ? DpRequestDate.SelectedDate.Value : entity.RequestDate;

            try
            {
                ctx.Requests.Update(entity);
                ctx.SaveChanges();
                MessageBox.Show("Saved.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadDetails()
        {
            using var ctx = new ApartifyContext();

            // load lists for dropdowns
            var apartments = ctx.Apartments.Include(a => a.Building).ToList();
            var residents = ctx.Residents.ToList();

            CbApartment.ItemsSource = apartments;
            CbResident.ItemsSource = residents;

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

            // set selected apartment/resident
            CbApartment.SelectedValue = r.ApartmentId;
            CbResident.SelectedValue = r.ResidentId;

            // update building and contact textblocks
            TxtBuilding.Text = r.Apartment != null && r.Apartment.Building != null ? r.Apartment.Building.Name ?? string.Empty : string.Empty;
            TxtResidentContact.Text = r.Resident != null ? r.Resident.Phone ?? string.Empty : string.Empty;

            TxtStatus.Text = r.Status ?? string.Empty;
            DpRequestDate.SelectedDate = r.RequestDate;

            // set control editability
            TxtDescription.IsReadOnly = !_isEdit;
            CbApartment.IsEnabled = _isEdit;
            CbResident.IsEnabled = _isEdit;
            DpRequestDate.IsEnabled = _isEdit;
            BtnSave.Visibility = _isEdit ? Visibility.Visible : Visibility.Collapsed;

            // wire apartment change to update building display
            CbApartment.SelectionChanged -= CbApartment_SelectionChanged;
            CbApartment.SelectionChanged += CbApartment_SelectionChanged;
        }

        private void CbApartment_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var ap = CbApartment.SelectedItem as Apartment;
            TxtBuilding.Text = ap != null && ap.Building != null ? ap.Building.Name ?? string.Empty : string.Empty;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
