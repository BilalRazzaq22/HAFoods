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
            InitializeComponent();
            this.DataContext = new CustomerViewModel();
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Search.Grid = grdCustomer;
        }
    }
}
