using ERP.WpfClient.ViewModel.Supplier;
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

namespace ERP.WpfClient.View.Suppliers
{
    /// <summary>
    /// Interaction logic for SupplierMarketingBill.xaml
    /// </summary>
    public partial class SupplierMarketingBill : Window
    {
        public SupplierMarketingViewModel Model { get; set; }
        public SupplierMarketingBill()
        {
            InitializeComponent();
            Model = new SupplierMarketingViewModel();
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
    }
}
