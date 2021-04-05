using ERP.WpfClient.ViewModel.Transaction;
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

namespace ERP.WpfClient.View.Transaction
{
    /// <summary>
    /// Interaction logic for CurrentTransaction.xaml
    /// </summary>
    public partial class CurrentTransaction : UserControl
    {
        public CurrentTransaction()
        {
            InitializeComponent();
            this.DataContext = new CurrentTransactionViewModel();
        }
    }
}
