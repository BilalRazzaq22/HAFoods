using ERP.WpfClient.Controls.Helpers;
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
    /// Interaction logic for MessageBoxPopup.xaml
    /// </summary>
    public partial class MessageBoxPopup : UserControl
    {
        public MessageBoxPopup()
        {
            InitializeComponent();
        }

        public MessageBoxPopup(String message, string okButtonText = "OKAY")
        {

            InitializeComponent();

            txtMessage.Text = message;
            btnOk.Content = okButtonText;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            ApplicationManager.Instance.HideMessageBox();
        }
    }
}
