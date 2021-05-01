using ERP.Common;
using ERP.Common.NotifyProperty;
using ERP.WpfClient.View.Home;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ERP.WpfClient.ViewModel
{
    public class MainViewModel : ViewModelBase, INotifyOnBringIntoView
    {
        #region Fields

        private bool _isUserLoggedIn;
        private bool _showFloorPlanButton;
        private readonly ViewManagerService _viewManagerService;
        //private readonly IViewModelUIService _viewModelUIService;
        //private readonly IApplicationService _applicationService;
        //private readonly IOrderSourceService _orderSourceService;
        FrameworkElement _element;
        //private AppSetting _appSettings;
        private bool _showCreateButton;
        private bool _showTrackButton;
        private bool IsTrackEnabled { get; set; }
        private string _currentStorePicture;
        private string _topViewName;
        private bool _isCurrentTransactionEnable;
        private bool _isInventoryLookUpEnable;
        private bool _isReserveItemsEnable;
        private bool _isReleaseItemsEnable;
        private bool _isCustomerEnable;
        //public MainViewCommand MainViewCommand { get; set; }
        #endregion

        public MainViewModel()
        {
            MainViewCommand = new RelayCommand<string>(ExecuteMainViewCommand);
            //MainViewCommand = new MainViewCommand(this);
            _viewManagerService = ViewManagerService.CreateInstance();
            InitApp();
        }

        //public MainViewModel(FrameworkElement element)
        //{
        //    ////_applicationService = App.Resolve<IApplicationService>();
        //    //_viewModelUIService = ViewModelUIService.CreateInstance();
        //    //// _orderSourceService = App.Resolve<IOrderSourceService>();
        //    //ApplicationExitCommand = new RelayCommand(ExitApplication);
        //    //LoadViewCommand = new RelayCommand<ViewTypes>(LoadView);
        //    //ShowViewCommand = new RelayCommand<string>(LoadViewAsync);
        //    //ClosePopupCommand = new RelayCommand(ClosePopup);
        //    //LoadOrderCommand = new RelayCommand<int>(LoadOrder);
        //    //// ShowWebOrderCommand = new RelayCommand(ShowWebOrder);
        //    //// ShowCancelledWebOrderCommand = new RelayCommand(ShowCancelledWebOrder);
        //    //Messenger.Default.Register<ShowViewMessage>(this, ShowView);
        //    //_element = element;
        //    //IsTrackEnabled = true;
        //    //ShowFloorPlanButton = true; // TODO: Enable this only if Store type is Fine Dining
        //    //InitApp();
        //    //TopViewName = "";
        //    //CurrentStorePicture = ApplicationManager.CurrentStorePicture;
        //    //IsCurrentTransactionEnable = false;
        //    //IsInventoryLookUpEnable = false;
        //    //IsReleaseItemsEnable = false;
        //    //IsReserveItemsEnable = false;
        //    //IsCustomerEnable = false;



        //}

        #region Properties

        public RelayCommand<string> MainViewCommand { get; set; }

        //public bool IsCustomerEnable
        //{
        //    get { return _isCustomerEnable; }
        //    set { _isCustomerEnable = value; RaisePropertyChanged("IsCustomerEnable"); }
        //}

        //public bool IsReleaseItemsEnable
        //{
        //    get { return _isReleaseItemsEnable; }
        //    set { _isReleaseItemsEnable = value; RaisePropertyChanged("IsReleaseItemsEnable"); }
        //}

        //public bool IsReserveItemsEnable
        //{
        //    get { return _isReserveItemsEnable; }
        //    set { _isReserveItemsEnable = value; RaisePropertyChanged("IsReserveItemsEnable"); }
        //}

        //public bool IsInventoryLookUpEnable
        //{
        //    get { return _isInventoryLookUpEnable; }
        //    set { _isInventoryLookUpEnable = value; RaisePropertyChanged("IsInventoryLookUpEnable"); }
        //}

        //public bool IsCurrentTransactionEnable
        //{
        //    get { return _isCurrentTransactionEnable; }
        //    set { _isCurrentTransactionEnable = value; RaisePropertyChanged("IsCurrentTransactionEnable"); }
        //}


        //public string TopViewName
        //{
        //    get { return _topViewName; }
        //    set { _topViewName = value; RaisePropertyChanged("TopViewName"); }
        //}

        //public string CurrentStorePicture
        //{
        //    get { return _currentStorePicture; }
        //    set { _currentStorePicture = value; RaisePropertyChanged("CurrentStorePicture"); }
        //}

        //public bool ShowTrackButton
        //{
        //    get { return _showTrackButton; }
        //    set { _showTrackButton = value; RaisePropertyChanged("ShowTrackButton"); }
        //}

        //public bool ShowCreateButton
        //{
        //    get { return _showCreateButton; }
        //    set { _showCreateButton = value; RaisePropertyChanged("ShowCreateButton"); }
        //}

        //public bool ShowFloorPlanButton
        //{
        //    get { return _showFloorPlanButton; }
        //    set { _showFloorPlanButton = value; RaisePropertyChanged("ShowFloorPlanButton"); }
        //}

        //#endregion

        //#region Relay Commands


        //public RelayCommand<string> SearchCommand
        //{
        //    get;
        //    private set;
        //}

        //public RelayCommand<string> ShowViewCommand
        //{
        //    get;
        //    private set;
        //}

        //public RelayCommand<int> LoadOrderCommand
        //{
        //    get;
        //    private set;
        //}

        //public RelayCommand ShowWebOrderCommand { get; private set; }
        //public RelayCommand ShowCancelledWebOrderCommand { get; private set; }



        //public RelayCommand ApplicationExitCommand { get; private set; }
        //public RelayCommand ApplicationMinimizeCommand { get; private set; }

        //public RelayCommand<ViewTypes> LoadViewCommand { get; private set; }

        //public RelayCommand ClosePopupCommand { get; protected set; }
        #endregion

        #region Methods

        //private void ShowWebOrder()
        //{
        //    if (ApplicationManager.Instance.WebOrders.Count > 0)
        //    {
        //        var order = ApplicationManager.Instance.WebOrders[0];
        //        var ordersourcename = _orderSourceService.GetOrderSourceById(Convert.ToInt32(order.order_source_id));
        //        order.order_source_name = ordersourcename != null ? ordersourcename.name : "";


        //        ApplicationManager.Instance.ShowDialog(string.Empty, new ViewWebOrderPopUp(new WebOrderDetailsViewModel(order, string.Empty)));
        //    }
        //}

        //private void ShowCancelledWebOrder()
        //{
        //    if (ApplicationManager.Instance.CancelledWebOrders.Count > 0)
        //    {
        //        var order = ApplicationManager.Instance.CancelledWebOrders[0];
        //        ApplicationManager.Instance.ShowDialog(string.Empty, new ViewWebOrderPopUp(new WebOrderDetailsViewModel(order, string.Empty, false, showCancelOrder: true)));
        //    }
        //}

        private void ExecuteMainViewCommand(string str)
        {
            switch (str)
            {
                case "Home":
                    LoadViewAsync(ViewTypes.Home.ToString());
                    break;
                case "Current Transaction":
                    LoadViewAsync(ViewTypes.CurrentTransaction.ToString());
                    break;
                case "Customer":
                    LoadViewAsync(ViewTypes.Customer.ToString());
                    break;

                case "Supplier":
                    LoadViewAsync(ViewTypes.Supplier.ToString());
                    break;
                case "Stock":
                    LoadViewAsync(ViewTypes.Stock.ToString());
                    break;

                case "Purchase Order":
                    LoadViewAsync(ViewTypes.Customer.ToString());
                    break;

                case "Sales Order":
                    LoadViewAsync(ViewTypes.Customer.ToString());
                    break;
                     
                case "Cash Book":
                    LoadViewAsync(ViewTypes.CashBookOne.ToString());
                    break;

                case "Cash Book 2":
                    LoadViewAsync(ViewTypes.CashBookTwo.ToString());
                    break;

                case "Reports":
                    LoadViewAsync(ViewTypes.Reports.ToString());
                    break;

                case "User":
                    LoadViewAsync(ViewTypes.User.ToString());
                    break;
            }
        }

        public void ClosePopup()
        {
            // Messenger.Default.Send<PopupDialogMessage>(new PopupDialogMessage() { Show = false });
        }

        //public void ShowView(ShowViewMessage msg)
        //{
        //    Console.WriteLine("{0} ShowView Messsage Received", DateTime.Now);

        //    //if (ApplicationManager.Instance.IsNavigatedToEditOrder && !msg.UseAdminVerification && !msg.ViewName.Equals("EditOrder")
        //    //   /*&& ApplicationManager.StoreDetails.park_enable && !ApplicationManager.IsFineDining*/)
        //    //{
        //    //    Console.WriteLine("{0} Sending Navigate Away Messsage ", DateTime.Now);
        //    //    Messenger.Default.Send<NavigateAwayFromOrderMessage>(new NavigateAwayFromOrderMessage
        //    //    {
        //    //        TargetView = msg.ViewName,
        //    //        Action = () =>
        //    //        {
        //    //            NavigateToView(msg.ViewName, msg.PrimaryKey, objectDataContext: msg.DataContext);

        //    //            if (msg.ActionToExecute != null)
        //    //            {
        //    //                msg.ActionToExecute();
        //    //            }
        //    //        }
        //    //    });
        //    //}
        //    //else 
        //    if (ApplicationManager.Instance.IsNavigatedToCurrentTransaction)
        //    {

        //        Console.WriteLine("{0} Sending Navigate Away Messsage ", DateTime.Now);
        //        Messenger.Default.Send<NavigateAwayFromCurrentTransactionMessage>(new NavigateAwayFromCurrentTransactionMessage
        //        {
        //            TargetView = msg.ViewName,
        //            Action = () =>
        //            {
        //                NavigateToView(msg.ViewName, msg.PrimaryKey, objectDataContext: msg.DataContext);

        //                if (msg.ActionToExecute != null)
        //                {
        //                    msg.ActionToExecute();
        //                }
        //            }
        //        });

        //        //if (ApplicationManager.Instance.IsAddedFromProductOperations)
        //        //{
        //        //    NavigateToView(msg.ViewName, msg.PrimaryKey, msg.ActionToExecute, msg.DataContext);
        //        //}
        //        //else
        //        //{
        //        //    Console.WriteLine("{0} Sending Navigate Away Messsage ", DateTime.Now);
        //        //    Messenger.Default.Send<NavigateAwayFromCurrentTransactionMessage>(new NavigateAwayFromCurrentTransactionMessage
        //        //    {
        //        //        TargetView = msg.ViewName,
        //        //        Action = () =>
        //        //        {
        //        //            NavigateToView(msg.ViewName, msg.PrimaryKey, objectDataContext: msg.DataContext);

        //        //            if (msg.ActionToExecute != null)
        //        //            {
        //        //                msg.ActionToExecute();
        //        //            }
        //        //        }
        //        //    });
        //        //}
        //    }
        //    else
        //    {
        //        NavigateToView(msg.ViewName, msg.PrimaryKey, msg.ActionToExecute, msg.DataContext);
        //    }

        //}


        //private void MinimizeApplication()
        //{
        //    ApplicationManager.MinimizeApplication();
        //}

        private void ExitApplication()
        {

            //if (ApplicationManager.Instance.IsNavigatedToEditOrder /*&& ApplicationManager.StoreDetails.park_enable && !ApplicationManager.IsFineDining*/)
            //{
            //    Messenger.Default.Send<NavigateAwayFromOrderMessage>(new NavigateAwayFromOrderMessage
            //    {
            //        Action = () => ApplicationManager.ExitApplication()
            //    });
            //}
            //else
            //if (ApplicationManager.Instance.IsNavigatedToCurrentTransaction)
            //{

            //    Console.WriteLine("{0} Sending Navigate Away Messsage ", DateTime.Now);
            //    Messenger.Default.Send<NavigateAwayFromCurrentTransactionMessage>(new NavigateAwayFromCurrentTransactionMessage
            //    {

            //        Action = () => ApplicationManager.ExitApplication()
            //    });
            //}
            //else
            //{

            //    ApplicationManager.ExitApplication();
            //}

        }

        //private void LoadCase(int caseId)
        //{
        //    _viewManagerService.Select().CaseTransition<EditCase>(new NavigateKey(ViewTypes.EditCase, caseId));
        //}

        //private async void LoadViewAsync(string viewName)
        //{
        //    await RunAsyncTask(_element, () => NavigateToView(viewName));
        //}

        public void LoadOrder(int orderId)
        {
            // _viewManagerService.Select("home").OrderTransition<NewCustomerOrder>(new NavigateKey(ViewTypes.EditOrder, orderId));
        }

        internal void NavigateToView(string viewName, object primaryKey = null, Action actionToExecute = null, object objectDataContext = null)
        {
            var viewType = (ViewTypes)Enum.Parse(typeof(ViewTypes), viewName);

            Console.WriteLine("{0} Navigating To View ", DateTime.Now);

            switch (viewType)
            {
                //case ViewTypes.UserLogin:
                //    _viewManagerService.Select().Transition<View.Users.UserLogin>(viewType);
                //    break;
                case ViewTypes.Home:
                    _viewManagerService.Select("home").Transition<View.Home.Home>(viewType);
                    break;
                case ViewTypes.Customer:
                    _viewManagerService.Select("home").Transition<View.Customers.Customer>(viewType);
                    break;
                case ViewTypes.Supplier:
                    _viewManagerService.Select("home").Transition<View.Suppliers.Supplier>(viewType);
                    break;
                case ViewTypes.Stock:
                    _viewManagerService.Select("home").Transition<View.Stock.Stock>(viewType);
                    break;
                case ViewTypes.CurrentTransaction:
                    _viewManagerService.Select("home").Transition<View.Transaction.CurrentTransaction>(viewType);
                    break;
                case ViewTypes.CashBookOne:
                    _viewManagerService.Select("home").Transition<View.CashBooks.CashBook1>(viewType);
                    break;
                case ViewTypes.CashBookTwo:
                    _viewManagerService.Select("home").Transition<View.CashBooks.CashBook2>(viewType);
                    break;
                case ViewTypes.Reports:
                    _viewManagerService.Select("home").Transition<View.Reports.AllReports>(viewType);
                    break;
<<<<<<< HEAD
=======
                case ViewTypes.PurchaseOrder:
                    //_viewManagerService.Select("home").Transition<View.PurchaseOrders.PurchaseOrder>(viewType);
                    break;
>>>>>>> main
                case ViewTypes.User:
                    _viewManagerService.Select("home").Transition<View.Users.User>(viewType);
                    break;
                default:
                    break;
            }
        }

        private bool AuthenticateAdmin(ViewTypes type, Action executeAction)
        {
            //if (!ApplicationManager.Instance.CurrentUser.IsAdmin)
            //{

            //    var navKey = new NavigateKey(ViewTypes.ManagerLogin, null);
            //    navKey.ExecuteAction = executeAction;

            //    _viewManagerService.Select().Transition<AuthenticateAdmin>(navKey, null);

            //    return false;
            //}
            //else
            //{
            if (executeAction != null)
                executeAction();
            //}

            return true;
        }

        public void LoadViewAsync(string viewName)
        {

            //if (!viewName.Equals("Registration") && ApplicationManager.Instance.IsNavigatedToCurrentTransaction)
            //{

            //    Console.WriteLine("{0} Sending Navigate Away Messsage ", DateTime.Now);
            //    Messenger.Default.Send<NavigateAwayFromCurrentTransactionMessage>(new NavigateAwayFromCurrentTransactionMessage
            //    {
            //        TargetView = viewName,
            //        Action = () => RunAsyncTask(_element, () => NavigateToView(viewName))
            //    });
            //}
            //else
            //{
            RunAsyncTask(_element, () => NavigateToView(viewName));
            //}


        }

        public Task RunAsyncTask(FrameworkElement element, Action action)
        {

            return Task.Factory.StartNew(action, CancellationToken.None, TaskCreationOptions.AttachedToParent, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void LoadView(ViewTypes viewType)
        {
            NavigateToView(viewType.ToString());
        }

        #endregion


        #region Properties
        //public ApplicationManager ApplicationManager
        //{
        //    get { return ApplicationManager.Instance; }
        //}


        #endregion

        public void InitApp()
        {
            var defaultScreen = "";

            //if (ApplicationManager.Instance.AppSettings != null && !String.IsNullOrEmpty(ApplicationManager.Instance.AppSettings.MacAddress))
            //{
            //    if (!String.IsNullOrEmpty(ApplicationManager.Instance.AppSettings.TerminalName) || (ApplicationManager.Instance.AppSettings.TerminalId != 0))
            //    {
            //        defaultScreen = "Login";
            //    }
            //    else
            //    {
            //        defaultScreen = "SelectDevice";
            //    }

            //    //_applicationService.UpdateStoreDetails();
            //}
            //else
            //{
            //var settings = App.GetSettings();

            //if (settings != null && settings.AppSettings != null)
            //{
            //    //  ApplicationManager.Instance.UpdateAppSettings(settings.AppSettings);
            //    defaultScreen = "Login";
            //}
            //else
            //{
            defaultScreen = "CurrentTransaction";
            //}


            //}

            NavigateToView(defaultScreen);

        }

        //private void AuthenticateUser()
        //{
        //    //IsCurrentTransactionEnable = true;
        //    if (ApplicationManager.UserPermissions.Count > 0)
        //    {
        //        foreach (var item in ApplicationManager.UserPermissions)
        //        {
        //            //if ((item.displayName.ToLower().ToString() == PosPermissions.Linediscountamount.ToString()) ? IsLineDiscountAmountEnable = false : IsLineDiscountAmountEnable = true) ;
        //            if (item.displayName.ToLower().ToString() == PosPermissions.CurrentTransactions.ToString().ToLower())
        //            {
        //                IsCurrentTransactionEnable = true;
        //            }
        //            if (item.displayName.ToLower().ToString() == PosPermissions.Inventorylookup.ToString().ToLower())
        //            {
        //                IsInventoryLookUpEnable = true;
        //            }
        //            if (item.displayName.ToLower().ToString() == PosPermissions.ReserveItems.ToString().ToLower())
        //            {
        //                IsReserveItemsEnable = true;
        //            }
        //            if (item.displayName.ToLower().ToString() == PosPermissions.ReleaseItems.ToString().ToLower())
        //            {
        //                IsReleaseItemsEnable = true;
        //            }
        //            if (item.displayName.ToLower().ToString() == PosPermissions.Customer.ToString().ToLower())
        //            {
        //                IsCustomerEnable = true;
        //            }
        //        }
        //    }
        //}

        public void OnBringIntoView()
        {
            InitApp();

        }

        private void InitViewModel()
        {
            //AuthenticateUser();
        }
    }
}
