using Apartify.BLL;
using Apartify.DAL;
using Apartify.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace Apartify.Views.manager
{
    public partial class RequestManagementWindow : Window
    {
        private readonly IRequestBll _requestBll;
        public ObservableCollection<Request> Requests { get; set; }

        public RequestManagementWindow()
        {
            InitializeComponent();
            var context = new ApartifyContext();
            _requestBll = new RequestBll(new RequestDal(context));
            LoadData();
        }

        private Request SelectedRequest { get; set; }

        private void LoadData()
        {
            Requests = new ObservableCollection<Request>(_requestBll.GetList());
            dgRequest.ItemsSource = Requests;
            SelectedRequest = null;
        }

        private void DgRequest_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            SelectedRequest = dgRequest.SelectedItem as Request;
        }

        private void DoneCommand(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SelectedRequest == null)
                {
                    MessageBox.Show("Please select a request.");
                    return;
                }

                SelectedRequest.Status = "Done";
                SelectedRequest.RequestDate = DateTime.Now;
                
                if (_requestBll.Edit(SelectedRequest))
                {
                    MessageBox.Show("Request marked as done successfully.");
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Failed to update request.");
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CancelRequestCommand(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SelectedRequest == null)
                {
                    MessageBox.Show("Please select a request.");
                    return;
                }

                SelectedRequest.Status = "Canceled";
                
                if (_requestBll.Edit(SelectedRequest))
                {
                    MessageBox.Show("Request canceled successfully.");
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Failed to cancel request.");
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
