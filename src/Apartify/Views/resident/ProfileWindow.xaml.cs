using Apartify.DAL;
using Apartify.Models;
using System.Linq;
using System.Windows;

namespace Apartify.Views.resident
{
    public partial class ProfileWindow : Window
    {
        private readonly UserAccount _user;
        private Resident _resident;
        private bool isEditing = false;

        public ProfileWindow(UserAccount user)
        {
            InitializeComponent();
            _user = user;
            LoadProfile();
        }

        private void LoadProfile()
        {
            var context = new ApartifyContext();

            _resident = context.Residents
                .FirstOrDefault(r => r.UserId == _user.UserId);

            if (_resident == null)
            {
                MessageBox.Show("Resident not found");
                return;
            }

            tbFullName.Text = _resident.FullName;
            tbPhone.Text = _resident.Phone;
            tbEmail.Text = _resident.Email;

            // 🔥 khóa lại ban đầu
            tbFullName.IsReadOnly = true;
            tbPhone.IsReadOnly = true;
            tbEmail.IsReadOnly = true;
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (!isEditing) return;

            var context = new ApartifyContext();

            var resident = context.Residents
                .FirstOrDefault(r => r.ResidentId == _resident.ResidentId);

            if (resident == null) return;

            resident.FullName = tbFullName.Text;
            resident.Phone = tbPhone.Text;
            resident.Email = tbEmail.Text;

            context.SaveChanges();

            MessageBox.Show("Profile updated successfully!");

            // 🔥 khóa lại sau khi save
            tbFullName.IsReadOnly = true;
            tbPhone.IsReadOnly = true;
            tbEmail.IsReadOnly = true;

            btnEdit.Visibility = Visibility.Visible;
            btnSave.Visibility = Visibility.Collapsed;

            isEditing = false;
        }
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            isEditing = true;

            tbFullName.IsReadOnly = false;
            tbPhone.IsReadOnly = false;
            tbEmail.IsReadOnly = false;

            btnEdit.Visibility = Visibility.Collapsed;
            btnSave.Visibility = Visibility.Visible;
        }
    }
}