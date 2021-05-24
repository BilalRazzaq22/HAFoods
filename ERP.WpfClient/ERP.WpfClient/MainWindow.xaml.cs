using ERP.Common;
using ERP.WpfClient.Controls.Helpers;
using ERP.WpfClient.LoadControls;
using ERP.WpfClient.Messages.BusyIndicator;
using ERP.WpfClient.Messages.Popup;
using ERP.WpfClient.Messages.User;
using ERP.WpfClient.View.Customers;
using ERP.WpfClient.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
            Messenger.Default.Register<PopupDialogMessage>(this, ShowPopupDialog);
            Messenger.Default.Register<BusyIndicatorMessage>(this, BusyIndicator);
            Messenger.Default.Register<UserLoginMessage>(this, UserLogin);
        }

        private void ShowPopupDialog(PopupDialogMessage message)
        {
            if (message.UseMessageBox)
            {
                if (message.Show)
                {

                    if (message.ShowLastDialog)
                    {
                        MsgPopupContainer.Visibility = System.Windows.Visibility.Visible;
                        return;
                    }

                    MsgPopupTitle.Text = message.Title;


                    MsgPopupTitleContainer.Visibility = String.IsNullOrEmpty(message.Title)
                        ? Visibility.Collapsed
                        : Visibility.Visible;

                    if (!message.UpdateHeadingOnly)
                    {
                        MsgPopupControlHost.Children.Clear();
                        MsgPopupControlHost.Children.Add(message.ControlToDisplay);

                        MsgPopupContainer.Visibility = System.Windows.Visibility.Visible;

                        MsgBtnClose.Visibility = message.ShowCloseButton ? Visibility.Visible : System.Windows.Visibility.Collapsed;
                    }



                }
                else
                {
                    if (!message.HideOnly)
                    {
                        MsgPopupControlHost.Children.Clear();
                    }
                    MsgPopupContainer.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
            else if (message.UseSecondPopup)
            {
                if (message.Show)
                {

                    if (message.ShowLastDialog)
                    {
                        PopupContainer2.Visibility = System.Windows.Visibility.Visible;
                        return;
                    }

                    PopupTitle2.Text = message.Title;
                    PopupTitleContainer2.Visibility = String.IsNullOrEmpty(message.Title)
                        ? Visibility.Collapsed
                        : Visibility.Visible;

                    if (!message.UpdateHeadingOnly)
                    {
                        PopupControlHost2.Children.Clear();
                        PopupControlHost2.Children.Add(message.ControlToDisplay);

                        PopupContainer2.Visibility = System.Windows.Visibility.Visible;

                        btnClose2.Visibility = message.ShowCloseButton
                            ? Visibility.Visible
                            : System.Windows.Visibility.Collapsed;
                    }
                }
                else
                {
                    PopupContainer2.Visibility = Visibility.Collapsed;

                    if (!message.HideOnly)
                    {
                        PopupContainer2.Children.Clear();
                    }
                }
            }
            else
            {
                if (message.Show)
                {

                    if (message.ShowLastDialog)
                    {
                        PopupContainer.Visibility = System.Windows.Visibility.Visible;
                        return;
                    }

                    PopupTitle.Text = message.Title;
                    PopupTitleContainer.Visibility = String.IsNullOrEmpty(message.Title)
                        ? Visibility.Collapsed
                        : Visibility.Visible;


                    if (!message.UpdateHeadingOnly)
                    {
                        PopupControlHost.Children.Clear();
                        PopupControlHost.Children.Add(message.ControlToDisplay);

                        PopupContainer.Visibility = System.Windows.Visibility.Visible;



                        btnClose.Visibility = message.ShowCloseButton
                            ? Visibility.Visible
                            : System.Windows.Visibility.Collapsed;
                    }
                }
                else
                {
                    PopupContainer.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }

        private void BusyIndicator(BusyIndicatorMessage message)
        {

            Console.WriteLine(String.Format("Received Indicator Message {0}", message.IsBusy));
            if (!message.IsBusy)
            {
                BusyBar.IsBusy = message.IsBusy;
                BusyBar.BusyContent = string.Empty;
                //Utilities.SetTimeout(2000, () =>
                //{
                //    busyIndicator.Visibility = message.IsBusy ? Visibility.Visible : Visibility.Hidden;
                //    Overlay.Visibility = busyIndicator.Visibility;
                //});

            }
            else
            {
                BusyBar.IsBusy = message.IsBusy;


                //busyIndicator.Visibility = message.IsBusy ? Visibility.Visible : Visibility.Hidden;
                //Overlay.Visibility = busyIndicator.Visibility;
            }
            //busyIndicator.Visibility = message.IsBusy ? Visibility.Visible : Visibility.Hidden;
            //Overlay.Visibility = busyIndicator.Visibility;

            BusyBar.BusyContent = !String.IsNullOrEmpty(message.BusyNoticeDetails)
                ? message.BusyNoticeDetails
                : "";

            //Thread.Sleep(300);

            Console.WriteLine(String.Format("Indicator Shown: {0}", message.IsBusy));
            //}


            if (message.IsBusyNotice && !String.IsNullOrEmpty(message.BusyNoticeDetails))
            {

                //BusyMessage.Content = message.BusyNoticeDetails;
                //ProgressNotificationBorder.Style = (Style)FindResource(message.IsErrorNotice ? "RedBGBorder" : "GreenBGBorder");
                //ProgressNotification.Opacity = 0;
                //ProgressNotification.Visibility = Visibility.Visible;
                //ProgressNotification.BeginAnimation(FrameworkElement.OpacityProperty,
                //                                    new DoubleAnimation(0, 1,
                //                                                        new Duration(TimeSpan.FromMilliseconds(400)), FillBehavior.HoldEnd));

                //HideProgressNotification(10);
                //_hideMessageNotificationTimer.Enabled = true;
                // _hideMessageNotificationTimer.Start();
            }
            else
            {
                //ProgressNotification.Visibility = Visibility.Collapsed;
            }
        }

        private void UserLogin(UserLoginMessage userLoginMessage)
        {
            txtLoginUser.Text = userLoginMessage.Username;
        }

        private void RadioButton_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void logout_Click(object sender, RoutedEventArgs e)
        {
            //Application.Current.Shutdown();
            //Application.Current.Exit += Current_Exit;

            ApplicationManager.Instance.ShowConfirmDialog("Are you sure you want to close the application?", () =>
            {
                Environment.Exit(0);
                ApplicationManager.Instance.HideDialog();
            }, () => ApplicationManager.Instance.HideMessageBox(), useYesNo: true);
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key == Key.F7)
            {
                CustomerSaleOrder customerSaleOrder = new CustomerSaleOrder();
                customerSaleOrder.ShowDialog();
            }
        }

        //private void Current_Exit(object sender, ExitEventArgs e)
        //{
        //    e.
        //}

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
