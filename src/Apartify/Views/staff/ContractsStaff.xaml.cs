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

        public ContractsStaff()
        {
            InitializeComponent();

            BtnRefresh.Click += BtnRefresh_Click;
            BtnCancel.Click += BtnCancel_Click; // Delete Contract
            BtnDone.Click += BtnDone_Click; // End Contract
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
                .Include(c => c.Resident)
                .OrderByDescending(c => c.StartDate)
                .ToList();

            _contracts = new ObservableCollection<Contract>(list);
            DataGridContracts.ItemsSource = _contracts;
        }

        private Contract? GetSelectedContract()
        {
            return DataGridContracts.SelectedItem as Contract;
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadContracts();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            // Delete contract
            var contract = GetSelectedContract();
            if (contract == null)
            {
                MessageBox.Show("Select a contract.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete the selected contract?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;

            using var ctx = new ApartifyContext();
            var entity = ctx.Contracts.Find(contract.ContractId);
            if (entity == null)
            {
                MessageBox.Show("Contract not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                ctx.Contracts.Remove(entity);
                ctx.SaveChanges();
                LoadContracts();
                MessageBox.Show("Contract deleted.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnDone_Click(object sender, RoutedEventArgs e)
        {
            // End contract (set EndDate to today)
            var contract = GetSelectedContract();
            if (contract == null)
            {
                MessageBox.Show("Select a contract.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            using var ctx = new ApartifyContext();
            var entity = ctx.Contracts.Find(contract.ContractId);
            if (entity == null)
            {
                MessageBox.Show("Contract not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                entity.EndDate = DateOnly.FromDateTime(DateTime.Today);
                ctx.Contracts.Update(entity);
                ctx.SaveChanges();
                LoadContracts();
                MessageBox.Show("Contract ended (EndDate set to today).", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
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
