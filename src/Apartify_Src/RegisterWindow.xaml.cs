using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Apartify.BLL;
using Apartify.DAL;
using Apartify.Models;

namespace Apartify.Views
{
    public partial class RegisterWindow : Window
    {
        private readonly UserAccountBLL _userAccountBLL;
        private readonly ResidentService _residentService;

        public RegisterWindow()
        {
            InitializeComponent();
            _userAccountBLL = new UserAccountBLL();
            
            var context = new ApartifyContext();
            var residentDal = new ResidentDal(context);
            _residentService = new ResidentService(residentDal);
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            // Account Info
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password.Trim();
            string confirmPassword = txtConfirmPassword.Password.Trim();

            // Resident Info
            string fullName = txtFullName.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string email = txtEmail.Text.Trim();

            // Role selection
            bool isResident = chkResident.IsChecked ?? false;
            bool isManager = chkManager.IsChecked ?? false;
            bool isStaff = chkStaff.IsChecked ?? false;

            // Validation
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                lblStatus.Text = "Please fill in all account fields";
                return;
            }

            if (isResident && (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(email)))
            {
                lblStatus.Text = "Please fill in all resident information fields";
                return;
            }

            if (password != confirmPassword)
            {
                lblStatus.Text = "Passwords do not match";
                return;
            }

            if (!isResident && !isManager && !isStaff)
            {
                lblStatus.Text = "Please select at least one role";
                return;
            }

            try
            {
                // Determination of Status: Resident = Active (1), others = Pending (0)
                // If multiple roles are selected, we follow the user's rule: 
                // "Resident will be created immediately... other positions will be in approval state"
                int status = isResident ? 1 : 0;

                // 1. Create UserAccount
                UserAccount newUser = new UserAccount
                {
                    Username = username,
                    Password = password,
                    Status = status
                };

                // Assign Roles (assuming Role IDs correspond to checkboxes)
                // In a production app, these IDs should be fetched from the database
                using (var context = new ApartifyContext())
                {
                    if (isResident) 
                    {
                        var residentRole = context.Roles.FirstOrDefault(r => r.RoleName == "Resident");
                        if (residentRole != null) newUser.Roles.Add(residentRole);
                    }
                    if (isManager)
                    {
                        var managerRole = context.Roles.FirstOrDefault(r => r.RoleName == "Manager");
                        if (managerRole != null) newUser.Roles.Add(managerRole);
                    }
                    if (isStaff)
                    {
                        var staffRole = context.Roles.FirstOrDefault(r => r.RoleName == "Staff");
                        if (staffRole != null) newUser.Roles.Add(staffRole);
                    }
                }

                _userAccountBLL.CreateUser(newUser);

                // 2. Create Resident record if Resident role is selected
                if (isResident)
                {
                    Resident newResident = new Resident
                    {
                        FullName = fullName,
                        Phone = phone,
                        Email = email,
                        UserId = newUser.UserId
                    };
                    _residentService.AddResident(newResident);
                }

                string message = isResident 
                    ? "Resident account registered and activated successfully!" 
                    : "Account registered successfully! Waiting for manager approval.";
                
                MessageBox.Show(message, "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error: {ex.Message}";
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
