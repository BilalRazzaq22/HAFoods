using ERP.WpfClient.Controls.Helpers;
using ERP.WpfClient.ViewModel.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ERP.WpfClient.View.Customers
{
    /// <summary>
    /// Interaction logic for CustomerMarketingBill.xaml
    /// </summary>
    public partial class CustomerMarketingBill : Window
    {
        public CustomerMarketingViewModel Model { get; set; }
        public CustomerMarketingBill()
        {
            InitializeComponent();
            Model = new CustomerMarketingViewModel();
            this.DataContext = Model;
        }

        private void _txtSearch_MouseEnter(object sender, MouseEventArgs e)
        {
            if (_txtSearch.Text == "Search Order")
            {
                _txtSearch.Text = "";
            }
        }

        private void _txtSearch_MouseLeave(object sender, MouseEventArgs e)
        {
            if (_txtSearch.Text == "")
            {
                _txtSearch.Text = "Search Order";
            }
        }

        private void _imgClear_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _txtSearch.Text = "Search Order";
        }

        private void _txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Model.OrderNumber = _txtSearch.Text;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ApplicationManager.IsCustomerBillOpen = false;
        }
    }
}
