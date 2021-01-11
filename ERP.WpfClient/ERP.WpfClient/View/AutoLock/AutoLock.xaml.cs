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

namespace ERP.WpfClient.View.AutoLock
{
    /// <summary>
    /// Interaction logic for AutoLock.xaml
    /// </summary>
    public partial class AutoLock : UserControl
    {
        ApplicationManager ApplicationManager;
        public AutoLock(ApplicationManager applicationManager)
        {
            ApplicationManager = applicationManager;
            InitializeComponent();
        }
        public AutoLock()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ApplicationManager.Instance.ExitApplication();
        }
    }
}
