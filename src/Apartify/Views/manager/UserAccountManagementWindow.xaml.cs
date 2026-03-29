using Apartify.BLL;
using Apartify.DAL;
using Apartify.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Apartify.Views.manager
{
    public partial class UserAccountManagementWindow : Window
    {
        private readonly IUserAccountBll _userAccountBll;
        public ObservableCollection<UserAccount> UserAccounts { get; set; }

        public UserAccountManagementWindow()
        {
            InitializeComponent();
            var context = new ApartifyContext();
            _userAccountBll = new UserAccountBll(new UserAccountDal(context));
            LoadData();
        }

        private UserAccount SelectedAccount { get; set; }

        private void LoadData()
        {
            UserAccounts = new ObservableCollection<UserAccount>(_userAccountBll.GetList());
            dgAccounts.ItemsSource = UserAccounts;
            SelectedAccount = null;
        }

        private void DgAccounts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedAccount = dgAccounts.SelectedItem as UserAccount;
        }

        private void DeactivateCommand(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SelectedAccount == null)
                {
                    MessageBox.Show("Please select an account to deactivate.");
                    return;
                }

                if (_userAccountBll.ChangeStatus(SelectedAccount.UserId, 1))
                {
                    MessageBox.Show("Account deactivated successfully.");
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Failed to deactivate account.");
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ActivateCommand(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SelectedAccount == null)
                {
                    MessageBox.Show("Please select an account to activate.");
                    return;
                }

                if (_userAccountBll.ChangeStatus(SelectedAccount.UserId, 0))
                {
                    MessageBox.Show("Account activated successfully.");
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Failed to activate account.");
                }
            }
            catch (System.Exception ex)
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
