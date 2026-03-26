using Apartify.BLL;
using Apartify.DAL;
using Apartify.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Apartify.Views.manager
{
    /// <summary>
    /// Interaction logic for BuildingManagementWindow.xaml
    /// </summary>
    public partial class BuildingManagementWindow : Window
    {
        private readonly IBuildingBll _buildingBll;

        public ObservableCollection<Building> Buildings { get; set; }
        public Building SelectedBuilding { get; set; }

        public BuildingManagementWindow()
        {
            InitializeComponent();
            var context = new ApartifyContext();
            _buildingBll = new BuildingBll(new BuildingDal(context));
            this.DataContext = this;
            LoadData();
        }
        private void LoadData()
        {
            Buildings = new ObservableCollection<Building>(_buildingBll.GetList());
            dgBuilding.ItemsSource = null;
            dgBuilding.ItemsSource = Buildings;
            SelectedBuilding = null;

            txtName.Text = "";
            txtAddress.Text = "";
        }
        private void DgBuilding_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedBuilding = dgBuilding.SelectedItem as Building;
            if (SelectedBuilding != null)
            {
                txtName.Text = SelectedBuilding.Name;
                txtAddress.Text = SelectedBuilding.Address;
            }
        }
        // Add
        private void AddCommand(object sender, RoutedEventArgs e)
        {
            try
            {
                var building = new Building
                {
                    Name = txtName.Text,
                    Address = txtAddress.Text
                };

                if (_buildingBll.Create(building))
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

        // Edit
        private void EditCommand(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SelectedBuilding == null) return;

                SelectedBuilding.Name = txtName.Text;
                SelectedBuilding.Address = txtAddress.Text;

                if (_buildingBll.Edit(SelectedBuilding))
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
        // Delete
        private void DeleteCommand(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SelectedBuilding == null) return;

                if (_buildingBll.Remove(SelectedBuilding.BuildingId))
                {
                    MessageBox.Show("Delete success");
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Delete failed. Building may have apartments.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //Close
        private void CloseCommand(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NavApartment_Click(object sender, RoutedEventArgs e)
        {
            ApartmentManagementWindow aptWin = new ApartmentManagementWindow();
            aptWin.Show();
        }

        private void NavResident_Click(object sender, RoutedEventArgs e)
        {
            ResidentManagementWindow resWin = new ResidentManagementWindow();
            resWin.Show();
        }
    }
}
