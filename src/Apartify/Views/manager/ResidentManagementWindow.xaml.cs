using Apartify.BLL;
using Apartify.DAL;
using Apartify.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Apartify.Views.manager
{
    public partial class ResidentManagementWindow : Window
    {
        private readonly IResidentBll _residentBll;

        public ObservableCollection<Resident> Residents { get; set; }
        public Resident SelectedResident { get; set; }

        public ResidentManagementWindow()
        {
            InitializeComponent();
            var context = new ApartifyContext();
            _residentBll = new ResidentBll(new ResidentDal(context));
            this.DataContext = this;
            LoadData();
        }

        private void LoadData()
        {
            Residents = new ObservableCollection<Resident>(_residentBll.GetList());
            dgResident.ItemsSource = null;
            dgResident.ItemsSource = Residents;
            
            SelectedResident = null;
            txtFullName.Text = "";
            txtPhone.Text = "";
            txtEmail.Text = "";
        }

        private void DgResident_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedResident = dgResident.SelectedItem as Resident;
            if (SelectedResident != null)
            {
                txtFullName.Text = SelectedResident.FullName;
                txtPhone.Text = SelectedResident.Phone;
                txtEmail.Text = SelectedResident.Email;
            }
        }

        private void AddCommand(object sender, RoutedEventArgs e)
        {
            try
            {
                var resident = new Resident
                {
                    FullName = txtFullName.Text,
                    Phone = txtPhone.Text,
                    Email = txtEmail.Text
                };

                if (_residentBll.Create(resident))
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
                if (SelectedResident == null) return;

                SelectedResident.FullName = txtFullName.Text;
                SelectedResident.Phone = txtPhone.Text;
                SelectedResident.Email = txtEmail.Text;

                if (_residentBll.Edit(SelectedResident))
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
                if (SelectedResident == null) return;

                if (_residentBll.Remove(SelectedResident.ResidentId))
                {
                    MessageBox.Show("Delete success");
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Delete failed. Resident may have contracts.");
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
