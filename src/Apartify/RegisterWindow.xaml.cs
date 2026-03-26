using System;
using System.Linq;
using System.Windows;
using Apartify.BLL;
using Apartify.DAL;
using Apartify.Models;

namespace Apartify
{
    public partial class RegisterWindow : Window
    {
        private readonly IUserAccountBll _userAccountBll;
        private readonly IResidentBll _residentBll;

        public RegisterWindow()
        {
            InitializeComponent();
            var context = new ApartifyContext();
            var userAccountDal = new UserAccountDal(context);
            _userAccountBll = new UserAccountBll(userAccountDal);
            
            var residentDal = new ResidentDal(context);
            _residentBll = new ResidentBll(residentDal);
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

            if (!isResident && !isManager)
            {
                lblStatus.Text = "Please select at least one role";
                return;
            }

            try
            {
                // Determination of Status: Resident = Active (1), others = Pending (0)
                int status = isResident ? 1 : 0;
                
                // Assign role (Resident gets priority if both checked)
                string role = isResident ? "Resident" : "Manager";

                // 1. Create UserAccount
                UserAccount newUser = new UserAccount
                {
                    Username = username,
                    Password = password,
                    Status = status,
                    Role = role
                };

                _userAccountBll.Create(newUser);

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
                    _residentBll.Create(newResident);
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
