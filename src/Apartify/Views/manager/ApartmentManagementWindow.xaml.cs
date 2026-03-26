using Apartify.BLL;
using Apartify.DAL;
using Apartify.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Apartify.Views.manager
{
    public partial class ApartmentManagementWindow : Window
    {
        private readonly IApartmentBll _apartmentBll;
        private readonly IBuildingBll _buildingBll;

        public ObservableCollection<Apartment> Apartments { get; set; }
        public Apartment SelectedApartment { get; set; }

        public ApartmentManagementWindow()
        {
            InitializeComponent();
            var context = new ApartifyContext();
            _apartmentBll = new ApartmentBll(new ApartmentDal(context));
            _buildingBll = new BuildingBll(new BuildingDal(context));
            this.DataContext = this;
            LoadData();
        }

        private void LoadData()
        {
            Apartments = new ObservableCollection<Apartment>(_apartmentBll.GetList());
            dgApartment.ItemsSource = null;
            dgApartment.ItemsSource = Apartments;
            
            cboBuilding.ItemsSource = _buildingBll.GetList();
            
            SelectedApartment = null;
            txtNumber.Text = "";
            txtFloor.Text = "";
            txtArea.Text = "";
            cboBuilding.SelectedValue = null;
        }

        private void DgApartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedApartment = dgApartment.SelectedItem as Apartment;
            if (SelectedApartment != null)
            {
                txtNumber.Text = SelectedApartment.Number;
                txtFloor.Text = SelectedApartment.Floor?.ToString() ?? "";
                txtArea.Text = SelectedApartment.Area?.ToString() ?? "";
                cboBuilding.SelectedValue = SelectedApartment.BuildingId;
            }
        }

        private void AddCommand(object sender, RoutedEventArgs e)
        {
            try
            {
                var apartment = new Apartment
                {
                    Number = txtNumber.Text,
                    Floor = int.TryParse(txtFloor.Text, out int floor) ? floor : null,
                    Area = decimal.TryParse(txtArea.Text, out decimal area) ? area : null,
                    BuildingId = (int?)cboBuilding.SelectedValue
                };

                if (_apartmentBll.Create(apartment))
                {
                    MessageBox.Show("Add success");
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void EditCommand(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SelectedApartment == null) return;

                SelectedApartment.Number = txtNumber.Text;
                SelectedApartment.Floor = int.TryParse(txtFloor.Text, out int floor) ? floor : null;
                SelectedApartment.Area = decimal.TryParse(txtArea.Text, out decimal area) ? area : null;
                SelectedApartment.BuildingId = (int?)cboBuilding.SelectedValue;

                if (_apartmentBll.Edit(SelectedApartment))
                {
                    MessageBox.Show("Edit success");
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Edit failed");
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
                if (SelectedApartment == null) return;

                if (_apartmentBll.Remove(SelectedApartment.ApartmentId))
                {
                    MessageBox.Show("Delete success");
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Delete failed. Apartment may have contracts.");
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
