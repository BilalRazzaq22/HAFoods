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

namespace ERP.WpfClient.View.Popups.Payments
{
    /// <summary>
    /// Interaction logic for PurchasePaymentPopup.xaml
    /// </summary>
    public partial class PurchasePaymentPopup : UserControl
    {
        public PurchasePaymentPopup()
        {
            InitializeComponent();
        }

        public PurchasePaymentPopup(PurchaseOrderViewModel purchaseOrderViewModel)
        {
            InitializeComponent();
            this.DataContext = purchaseOrderViewModel;
        }
    }
}
