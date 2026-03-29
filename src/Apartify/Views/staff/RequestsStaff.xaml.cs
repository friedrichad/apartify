using System.Linq;
using System.Windows;
using Apartify.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace Apartify.Views.staff
{
    public partial class RequestsStaff : Window
    {
        private ObservableCollection<Request> _requests = new();

        public RequestsStaff()
        {
            InitializeComponent();

            BtnRefresh.Click += BtnRefresh_Click;
            BtnView.Click += BtnView_Click;
            BtnDone.Click += BtnDone_Click;
            BtnCancel.Click += BtnCancel_Click;
            BtnBack.Click += BtnBack_Click;

            try
            {
                LoadRequests();
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show($"Failed to load requests: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                // keep window open but list will be empty
            }
        }

        private void LoadRequests()
        {
            using var ctx = new ApartifyContext();
            var list = ctx.Requests
                .Include(r => r.Resident)
                .Include(r => r.Apartment)
                .OrderByDescending(r => r.RequestDate)
                .ToList();

            _requests = new ObservableCollection<Request>(list);
            DataGridRequests.ItemsSource = _requests;
        }

        private Request? GetSelectedRequest()
        {
            return DataGridRequests.SelectedItem as Request;
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadRequests();
        }

        private void BtnView_Click(object sender, RoutedEventArgs e)
        {
            var req = GetSelectedRequest();
            if (req == null)
            {
                MessageBox.Show("Please select a request.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var win = new RequestDetailsStaff(req.RequestId);
            win.Owner = this;
            win.ShowDialog();
        }

        private void BtnDone_Click(object sender, RoutedEventArgs e)
        {
            UpdateSelectedStatus("Done");
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            UpdateSelectedStatus("Canceled");
        }

        private void UpdateSelectedStatus(string status)
        {
            var req = GetSelectedRequest();
            if (req == null)
            {
                MessageBox.Show("Please select a request.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            using var ctx = new ApartifyContext();
            var entity = ctx.Requests.Find(req.RequestId);
            if (entity == null)
            {
                MessageBox.Show("Request not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            entity.Status = status;
            try
            {
                ctx.Update(entity);
                ctx.SaveChanges();
                LoadRequests();
                MessageBox.Show($"Status updated to '{status}'.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
