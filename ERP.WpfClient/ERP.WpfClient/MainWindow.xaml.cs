using ERP.Common;
using ERP.WpfClient.LoadControls;
using ERP.WpfClient.ViewModel;
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
        private readonly ViewManagerService _viewManagerService = ViewManagerService.CreateInstance();

        //UserControl objform = null;
        //ILoadControl loadcontrol;
        public MainWindow()
        {
            InitializeComponent();
            _viewManagerService.Add(MainControlsHost, null, "home");
            this.DataContext = new MainViewModel();
            //loadcontrol = new LoadControl();
        }

        //public void LoadForm(UserControl form)
        //{
        //    try
        //    {
        //        UserControl control = new UserControl();
        //        control = form;
        //        MainParent.Content = control.Content;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public void LoadControl(string key)
        //{
        //    objform = new UserControl();
        //    if (key == "Customer")
        //    {
        //        objform = loadcontrol.CustomerForm;
        //        LoadForm(objform);
        //    }
        //}

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    int index = int.Parse(((Button)e.Source).Uid);

        //    switch (index)
        //    {
        //        case 0:
        //            LoadControl("Customer");
        //            break;
        //    }
        //}
    }
}
