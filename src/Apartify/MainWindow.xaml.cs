using Apartify.BLL;
using Apartify.Models;
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

namespace Apartify.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UserAccountBLL _userAccountBLL;
        public MainWindow()
        {
            InitializeComponent();
            _userAccountBLL = new UserAccountBLL();
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

            var user = _userAccountBLL.Login(username, password);

            if (user != null)
            {
                MessageBox.Show("Login succeed!");

                // mở cửa sổ chính
 

                this.Close();
            }
            else
            {
                lblError.Text = "Wrong username or password";
            }
        }
        private void Register_Click(object sender, MouseButtonEventArgs e)
        {
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.ShowDialog();
        }

    }
}