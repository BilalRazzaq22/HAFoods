using ERP.WpfClient.ViewModel;
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
    }
}
