using ERP.WpfClient.LoadControls;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ERP.WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        UserControl objform = null;
        ILoadControl loadcontrol;
        public MainWindow()
        {
            InitializeComponent();
            loadcontrol = new LoadControl();
        }

        public void LoadForm(UserControl form)
        {
            try
            {
                UserControl control = new UserControl();
                control = form;
                MainParent.Content = control.Content;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void LoadControl(string key)
        {
            objform = new UserControl();
            if (key == "Customer")
            {
                objform = loadcontrol.CustomerForm;
                LoadForm(objform);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((Button)e.Source).Uid);

            switch (index)
            {
                case 0:
                    LoadControl("Customer");
                    break;
            }
        }
    }
}
