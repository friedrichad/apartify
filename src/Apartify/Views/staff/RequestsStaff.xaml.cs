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
        private System.Collections.Generic.List<Request> _allRequests = new();

        public RequestsStaff()
        {
            InitializeComponent();

            BtnRefresh.Click += BtnRefresh_Click;
            BtnView.Click += BtnView_Click;
            BtnUpdate.Click += BtnUpdate_Click;
            BtnDone.Click += BtnDone_Click;
            BtnCancel.Click += BtnCancel_Click;
            BtnBack.Click += BtnBack_Click;
            BtnSearch.Click += BtnSearch_Click;
            TxtSearch.KeyDown += TxtSearch_KeyDown;

            try
            {
                LoadRequests();
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show($"Failed to load requests: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadRequests()
        {
            using var ctx = new ApartifyContext();
            var list = ctx.Requests
                .Include(r => r.Resident)
                .Include(r => r.Apartment)
                    .ThenInclude(a => a.Building)
                .OrderByDescending(r => r.RequestDate)
                .ToList();

            _allRequests = list;
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

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            var q = (TxtSearch?.Text ?? string.Empty).Trim();

            if (_allRequests == null || _allRequests.Count == 0)
            {
                MessageBox.Show("No requests loaded to search. Try Refresh first.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (string.IsNullOrEmpty(q))
            {
                // reset
                _requests.Clear();
                foreach (var r in _allRequests)
                    _requests.Add(r);
                DataGridRequests.ItemsSource = _requests;
                return;
            }

            var qNorm = NormalizeForSearch(q);

            // If single-character query, restrict to apartment number prefix only to avoid broad matches
            System.Collections.Generic.List<Request> filtered;
            if (qNorm.Length == 1)
            {
                filtered = _allRequests.Where(r => StartsWithNormalized(r.Apartment?.Number, qNorm)).ToList();
            }
            else
            {
                filtered = _allRequests.Where(r =>
                    // prefer prefix match to reduce unrelated results
                    StartsWithNormalized(r.Description, qNorm)
                    || StartsWithNormalized(r.Resident?.FullName, qNorm)
                    || StartsWithNormalized(r.Apartment?.Number, qNorm)
                    || StartsWithNormalized(r.Apartment?.Building?.Name, qNorm)
                    || StartsWithNormalized(r.Status, qNorm)
                ).ToList();
            }

            if (filtered.Count == 0)
            {
                MessageBox.Show($"No matches found for '{q}'.", "Search", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            _requests.Clear();
            foreach (var r in filtered)
                _requests.Add(r);
            DataGridRequests.ItemsSource = _requests;
        }

        private void TxtSearch_KeyDown(object? sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
                BtnSearch_Click(sender, new RoutedEventArgs());
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
            LoadRequests();
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var req = GetSelectedRequest();
            if (req == null)
            {
                MessageBox.Show("Please select a request.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var win = new RequestDetailsStaff(req.RequestId, isEdit: true);
            win.Owner = this;
            win.ShowDialog();
            // refresh list to reflect edits
            LoadRequests();
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

        private static string NormalizeForSearch(string? s)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            var normalized = s.Normalize(System.Text.NormalizationForm.FormD);
            var sb = new System.Text.StringBuilder();
            foreach (var ch in normalized)
            {
                var uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(ch);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                    sb.Append(ch);
            }
            return sb.ToString().Normalize(System.Text.NormalizationForm.FormC).ToLowerInvariant();
        }

        private static int IndexOfNormalized(string? field, string qNorm)
        {
            if (string.IsNullOrEmpty(field)) return -1;
            var f = NormalizeForSearch(field);
            return f.IndexOf(qNorm, StringComparison.Ordinal);
        }

        private static bool StartsWithNormalized(string? field, string qNorm)
        {
            if (string.IsNullOrEmpty(field)) return false;
            var f = NormalizeForSearch(field);
            return f.StartsWith(qNorm, StringComparison.Ordinal);
        }
    }
}
