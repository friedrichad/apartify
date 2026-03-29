using System;
using System.Linq;
using System.Windows;
using Apartify.Models;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;

namespace Apartify.Views.staff
{
    public partial class ContractsStaff : Window
    {
        private ObservableCollection<Contract> _contracts = new();
        private System.Collections.Generic.List<Contract> _allContracts = new();

        public ContractsStaff()
        {
            InitializeComponent();

            BtnRefresh.Click += BtnRefresh_Click;
            BtnSearch.Click += BtnSearch_Click;
          
            TxtSearch.KeyDown += TxtSearch_KeyDown;
            BtnBack.Click += BtnBack_Click;

            try
            {
                LoadContracts();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Failed to load contracts: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void LoadContracts()
        {
            using var ctx = new ApartifyContext();
            var list = ctx.Contracts
                .Include(c => c.Apartment)
                    .ThenInclude(a => a.Building)
                .Include(c => c.Resident)
                .OrderBy(c => c.ContractId)
                .ToList();

            _allContracts = list;
            _contracts = new ObservableCollection<Contract>(list);
            DataGridContracts.ItemsSource = _contracts;

            TxtEmpty.Visibility = _contracts.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private Contract? GetSelectedContract()
        {
            return DataGridContracts.SelectedItem as Contract;
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadContracts();
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            var q = (TxtSearch?.Text ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(q))
            {
                
                _contracts.Clear();
                foreach (var c in _allContracts)
                    _contracts.Add(c);
                TxtEmpty.Visibility = _contracts.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
                return;
            }

            var qNorm = NormalizeForSearch(q);

            System.Collections.Generic.List<Contract> filtered;
            if (qNorm.Length == 1)
            {
                filtered = _allContracts.Where(c => StartsWithNormalized(c.Apartment?.Number, qNorm)).ToList();
            }
            else
            {
                filtered = _allContracts.Where(c =>
                    StartsWithNormalized(c.Resident?.FullName, qNorm)
                    || StartsWithNormalized(c.Apartment?.Number, qNorm)
                    || StartsWithNormalized(c.Apartment?.Building?.Name, qNorm)
                ).ToList();
            }

            _contracts.Clear();
            foreach (var c in filtered)
                _contracts.Add(c);

            TxtEmpty.Visibility = _contracts.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
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



        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TxtSearch_KeyDown(object? sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                BtnSearch_Click(sender, new RoutedEventArgs());
            }
        }
    }
}
