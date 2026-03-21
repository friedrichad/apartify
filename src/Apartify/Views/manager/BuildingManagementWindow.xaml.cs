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
        public void LoadData()
        {
            Buildings = new ObservableCollection<Building>(_buildingBll.GetList());
            dgBuilding.ItemsSource= Buildings;
            SelectedBuilding = new Building();
        }
        //add
        private void AddCommand(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_buildingBll.Create(SelectedBuilding))
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
    }
}
