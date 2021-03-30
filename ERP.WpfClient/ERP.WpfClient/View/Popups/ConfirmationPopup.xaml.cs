using ERP.WpfClient.ViewModel.Popup;
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

namespace ERP.WpfClient.View.Popups
{
    /// <summary>
    /// Interaction logic for ConfirmationPopup.xaml
    /// </summary>
    public partial class ConfirmationPopup : UserControl
    {
        public ConfirmationPopup()
        {
            InitializeComponent();
        }
        public ConfirmationPopup(ConfirmPopupViewModel model)
        {
            InitializeComponent();
            DataContext = model;
        }
    }
}
