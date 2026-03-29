using Apartify.BLL;
using Apartify.DAL;
using Apartify.Models;
using System;
using System.Windows;

namespace Apartify.Views.manager
{
    public partial class CreateContractWindow : Window
    {
        private readonly IContractBll _contractBll;
        private readonly IApartmentBll _apartmentBll;
        private Resident _resident;

        public CreateContractWindow(Resident resident)
        {
            InitializeComponent();
            var context = new ApartifyContext();
            _contractBll = new ContractBll(new ContractDal(context));
            _apartmentBll = new ApartmentBll(new ApartmentDal(context));
            _resident = resident;
            
            txtResidentName.Text = _resident.FullName;
            LoadApartments();
        }

        private void LoadApartments()
        {
            cboApartment.ItemsSource = _apartmentBll.GetList();
        }

        private void SaveCommand(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cboApartment.SelectedValue == null)
                {
                    MessageBox.Show("Please select an apartment.");
                    return;
                }

                if (!dpStartDate.SelectedDate.HasValue || !dpEndDate.SelectedDate.HasValue)
                {
                    MessageBox.Show("Please select start and end dates.");
                    return;
                }

                var contract = new Contract
                {
                    ResidentId = _resident.ResidentId,
                    ApartmentId = (int)cboApartment.SelectedValue,
                    StartDate = DateOnly.FromDateTime(dpStartDate.SelectedDate.Value),
                    EndDate = DateOnly.FromDateTime(dpEndDate.SelectedDate.Value)
                };

                if (_contractBll.Create(contract))
                {
                    MessageBox.Show("Contract created successfully.");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Failed to create contract.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CancelCommand(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
