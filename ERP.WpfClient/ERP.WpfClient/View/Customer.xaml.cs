using ERP.WpfClient.ViewModel;
using ERP.WpfClient.ViewModel.Customer;
using System.Windows.Controls;

namespace ERP.WpfClient.View
{
    /// <summary>
    /// Interaction logic for Customer.xaml
    /// </summary>
    public partial class Customer : UserControl
    {
        public Customer()
        {
            this.DataContext = new CustomerViewModel();
            InitializeComponent();
        }
    }
}
