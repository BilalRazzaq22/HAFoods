using ERP.WpfClient.ViewModel.Supplier;
using System.Windows.Controls;

namespace ERP.WpfClient.View.Suppliers
{
    /// <summary>
    /// Interaction logic for Supplier.xaml
    /// </summary>
    public partial class Supplier : UserControl
    {
        public Supplier()
        {
            InitializeComponent();
            this.DataContext = new SupplierViewModel();
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Search.Grid = grdSupplier;
        }
    }
}
