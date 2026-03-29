using Apartify.BLL;
using Apartify.DAL;
using Apartify.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Apartify.Views.manager
{
    public partial class ContractManagementWindow : Window
    {
        private readonly IContractBll _contractBll;
        public ObservableCollection<Contract> Contracts { get; set; }
        public Contract SelectedContract { get; set; }

        public ContractManagementWindow()
        {
            InitializeComponent();
            var context = new ApartifyContext();
            _contractBll = new ContractBll(new ContractDal(context));
            LoadData();
        }

        private void LoadData()
        {
            Contracts = new ObservableCollection<Contract>(_contractBll.GetList());
            dgContract.ItemsSource = Contracts;

            SelectedContract = null;
            dpStartDate.SelectedDate = null;
            dpEndDate.SelectedDate = null;
        }

        private void DgContract_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedContract = dgContract.SelectedItem as Contract;
            if (SelectedContract != null)
            {
                if (SelectedContract.StartDate.HasValue)
                {
                    dpStartDate.SelectedDate = SelectedContract.StartDate.Value.ToDateTime(TimeOnly.MinValue);
                }
                
                if (SelectedContract.EndDate.HasValue)
                {
                    dpEndDate.SelectedDate = SelectedContract.EndDate.Value.ToDateTime(TimeOnly.MinValue);
                }
            }
        }

        private void EditCommand(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SelectedContract == null) return;
                
                if (dpStartDate.SelectedDate.HasValue)
                {
                    SelectedContract.StartDate = DateOnly.FromDateTime(dpStartDate.SelectedDate.Value);
                }

                if (dpEndDate.SelectedDate.HasValue)
                {
                    SelectedContract.EndDate = DateOnly.FromDateTime(dpEndDate.SelectedDate.Value);
                }

                if (_contractBll.Edit(SelectedContract))
                {
                    MessageBox.Show("Sửa hợp đồng thành công");
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Sửa hợp đồng thất bại");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DeleteCommand(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SelectedContract == null) return;

                if (_contractBll.Remove(SelectedContract.ContractId))
                {
                    MessageBox.Show("Xóa hợp đồng thành công");
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Xóa hợp đồng thất bại");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CloseCommand(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
