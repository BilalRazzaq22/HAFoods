using ERP.WpfClient.ViewModel.Popup.Report;
using System.Windows.Controls;

namespace ERP.WpfClient.View.Reports
{
    /// <summary>
    /// Interaction logic for AllReports.xaml
    /// </summary>
    public partial class AllReports : UserControl
    {
        public AllReports()
        {
            InitializeComponent();
            DataContext = new AllReportsViewModel();
        }
    }
}
