using System;
using System.Linq;
using System.Windows;
using Apartify.Models;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;

namespace Apartify.Views.staff
{
    public partial class PaymentsStaff : Window
    {
        private ObservableCollection<ServiceFee> _fees = new();

        public PaymentsStaff()
        {
            InitializeComponent();

            BtnRefresh.Click += BtnRefresh_Click;
            BtnCancel.Click += BtnCancel_Click;
            BtnDone.Click += BtnDone_Click;

            try
            {
                LoadFees();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Failed to load payments: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadFees()
        {
            using var ctx = new Apartify.Models.ApartifyContext();
            var list = ctx.ServiceFees
                .OrderByDescending(f => f.Month)
                .ToList();
            _fees = new ObservableCollection<ServiceFee>(list);
            DataGridFees.ItemsSource = _fees;
        }

        private ServiceFee? GetSelectedFee()
        {
            return DataGridFees.SelectedItem as ServiceFee;
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadFees();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            var fee = GetSelectedFee();
            if (fee == null)
            {
                MessageBox.Show("Select a fee.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            using var ctx = new Apartify.Models.ApartifyContext();
            var entity = ctx.ServiceFees.Find(fee.FeeId);
            if (entity == null)
            {
                MessageBox.Show("Fee not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Treat Cancel as Paid = false
            entity.Paid = false;
            try
            {
                ctx.Update(entity);
                ctx.SaveChanges();
                LoadFees();
                MessageBox.Show("Payment marked as canceled.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnDone_Click(object sender, RoutedEventArgs e)
        {
            var fee = GetSelectedFee();
            if (fee == null)
            {
                MessageBox.Show("Select a fee.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            using var ctx = new Apartify.Models.ApartifyContext();
            var entity = ctx.ServiceFees.Find(fee.FeeId);
            if (entity == null)
            {
                MessageBox.Show("Fee not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            entity.Paid = true;
            try
            {
                ctx.Update(entity);
                ctx.SaveChanges();
                LoadFees();
                MessageBox.Show("Payment marked as done.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
