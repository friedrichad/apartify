using System;
using System.Windows;
using Apartify.BLL;
using Apartify.DAL;
using Apartify.Models;

namespace Apartify
{
    public partial class RegisterWindow : Window
    {
        private readonly IUserAccountBll _userAccountBll;

        public RegisterWindow()
        {
            InitializeComponent();
            var context = new ApartifyContext();
            var userAccountDal = new UserAccountDal(context);
            _userAccountBll = new UserAccountBll(userAccountDal);
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            // Account Info
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password.Trim();
            string confirmPassword = txtConfirmPassword.Password.Trim();

            // Role selection
            bool isManager = rbManager.IsChecked ?? false;
            
            // Validation
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                lblStatus.Text = "Please fill in all account fields";
                return;
            }

            if (password != confirmPassword)
            {
                lblStatus.Text = "Passwords do not match";
                return;
            }

            // Assign role value: "1" for Manager, "2" for Staff
            string role = isManager ? "1" : "2";

            try
            {
                // Create UserAccount
                UserAccount newUser = new UserAccount
                {
                    Username = username,
                    Password = password,
                    Role = role,
                    Status = 0
                };

                _userAccountBll.Create(newUser);

                MessageBox.Show("Account registered successfully!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
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
