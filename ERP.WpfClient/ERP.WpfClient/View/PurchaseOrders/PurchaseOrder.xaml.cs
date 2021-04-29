using ERP.WpfClient.ViewModel.PurchaseOrders;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ERP.WpfClient.View.PurchaseOrders
{
    /// <summary>
    /// Interaction logic for PurchaseOrder.xaml
    /// </summary>
    public partial class PurchaseOrder : UserControl
    {
        public PurchaseOrder()
        {
            InitializeComponent();
            DataContext = new PurchaseOrderViewModel();
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
            //Model.OrderNumber = _txtSearch.Text;
        }

    }
}
