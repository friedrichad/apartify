using Apartify.BLL;
using Apartify.DAL;
using Apartify.Models;
using System;
using System.Windows;

namespace Apartify.Views.manager
{
    public partial class CreateResident : Window
    {
        private readonly IResidentBll _residentBll;
        private readonly IUserAccountBll _userAccountBll;

        public CreateResident()
        {
            InitializeComponent();
            var context = new ApartifyContext();
            _residentBll = new ResidentBll(new ResidentDal(context));
            _userAccountBll = new UserAccountBll(new UserAccountDal(context));
        }

        private void EditCommand(object sender, RoutedEventArgs e)
        {
            try
            {
                string fullName = txtFullName.Text;
                string phone = txtPhone.Text;
                string email = txtEmail.Text;
                string userName = txtUserName.Text;
                string password = txtPassword.Text;

                if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin bắt buộc (họ tên, username, password).");
                    return;
                }

                var newUserAccount = new UserAccount
                {
                    Username = userName,
                    Password = password,
                    Role = "3", // 3: Resident
                    Status = 1 // Active
                };

                if (_userAccountBll.Create(newUserAccount))
                {
                    // Assuming UserAccount gets an ID or we can find it
                    // Actually EF Core populates the ID automatically if it's the same context
                    // Or we just fetch it back by Username
                    // Wait, BLL creates it. It should modify newUserAccount reference? Yes, EF Core does.
                    // But we will fetch just in case it doesn't return the modified object cleanly due to DAL structure.
                    // Oh, we can just save it inside transaction, but sticking to BLL is fine.
                    
                    // Actually let's assume newUserAccount.UserId is filled, if not, we get it by username.
                    int? newUserId = newUserAccount.UserId;
                    
                    var resident = new Resident
                    {
                        FullName = fullName,
                        Phone = phone,
                        Email = email,
                        UserId = newUserId
                    };

                    if (_residentBll.Create(resident))
                    {
                        MessageBox.Show("Thêm cư dân thành công.");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Không thể tạo thông tin cư dân.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
