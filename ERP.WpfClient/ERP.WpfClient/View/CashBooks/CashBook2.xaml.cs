using ERP.WpfClient.ViewModel.CashBooks;
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

namespace ERP.WpfClient.View.CashBooks
{
    /// <summary>
    /// Interaction logic for CashBook2.xaml
    /// </summary>
    public partial class CashBook2 : UserControl
    {
        public CashBook2()
        {
            InitializeComponent();
            DataContext = new CashBookTwoViewModel();
        }
    }
}
