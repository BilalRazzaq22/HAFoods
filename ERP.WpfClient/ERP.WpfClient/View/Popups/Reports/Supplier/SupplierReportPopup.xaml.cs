using ERP.WpfClient.ViewModel.Reports;
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

namespace ERP.WpfClient.View.Popups.Reports.Supplier
{
    /// <summary>
    /// Interaction logic for SupplierReportPopup.xaml
    /// </summary>
    public partial class SupplierReportPopup : UserControl
    {
        public SupplierReportPopup()
        {
            InitializeComponent();
        }

        public SupplierReportPopup(AllReportsViewModel allReportsViewModel)
        {
            InitializeComponent();
            DataContext = allReportsViewModel;
        }
    }
}
