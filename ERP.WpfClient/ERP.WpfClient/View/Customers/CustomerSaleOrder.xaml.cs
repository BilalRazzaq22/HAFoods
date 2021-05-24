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
    /// Interaction logic for CustomerSaleOrder.xaml
    /// </summary>
    public partial class CustomerSaleOrder : Window
    {
        public CustomerSaleOrder()
        {
            InitializeComponent();
            this.DataContext = new CustomerSaleOrderViewModel();
        }
    }
}
