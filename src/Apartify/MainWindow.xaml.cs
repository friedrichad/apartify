using Apartify.BLL;
using Apartify.Models;
using Apartify.DAL;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Apartify.Views.manager;
using Apartify.Views.staff;
using Apartify.Views.resident;
using Apartify.Views;

namespace Apartify
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IUserAccountBll _userAccountBll;
        public MainWindow()
        {
            InitializeComponent();
            var context = new ApartifyContext();
            var dal = new UserAccountDal(context);
            _userAccountBll = new UserAccountBll(dal);
        }
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                lblError.Text = "Please, Enter Username and Password";
                return;
            }

            try
            {
                var user = _userAccountBll.Login(username, password);
                if (user != null)
                {
                    if (user.Status == 1)
                    {
                        lblError.Text = "Your account is locked";
                        return;
                    }
                    MessageBox.Show("Login succeed!");

                    // Mở cửa sổ dựa trên role
                    if (user.Role == "1") // Manager
                    {
                        HomeManager managerHome = new HomeManager();
                        managerHome.Show();
                    }
                    else if (user.Role == "2") // Staff
                    {
                        HomeStaff staffHome = new HomeStaff();
                        staffHome.Show();
                    }
                    else if (user.Role == "3" || user.Role == "Resident") // Resident (keeping "Resident" for backward compatibility if needed)
                    {
                        HomeResident residentHome = new HomeResident();
                        residentHome.Show();
                    }
                    else
                    {
                        MessageBox.Show("Role is not assigned yet or pending approval.");
                        return;
                    }

                    this.Close();
                }
                else
                {
                    lblError.Text = "Wrong username or password";
                }
            }
            catch (System.Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
        private void Register_Click(object sender, MouseButtonEventArgs e)
        {
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.ShowDialog();
        }

    }
}