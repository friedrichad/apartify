using Apartify.BLL;
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
        }
    }
}
