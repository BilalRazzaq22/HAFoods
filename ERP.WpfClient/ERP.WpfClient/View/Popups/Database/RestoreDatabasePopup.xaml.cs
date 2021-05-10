using ERP.WpfClient.ViewModel;
using ERP.WpfClient.ViewModel.Database;
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

namespace ERP.WpfClient.View.Popups.Database
{
    /// <summary>
    /// Interaction logic for RestoreDatabasePopup.xaml
    /// </summary>
    public partial class RestoreDatabasePopup : UserControl
    {
        //public RestoreDatabasePopup(DatabaseViewModel model)
        //{
        //    InitializeComponent();
        //    this.DataContext = model;
        //}

        public RestoreDatabasePopup(MainViewModel model)
        {
            InitializeComponent();
            this.DataContext = model;
        }
    }
}
