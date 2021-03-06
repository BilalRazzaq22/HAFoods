using ERP.Common;
using ERP.Common.NotifyProperty;
using ERP.Entities.DbContext;
using ERP.WpfClient.Controls.Helpers;
using ERP.WpfClient.View.Home;
using ERP.WpfClient.View.Popups.Database;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private string _filePath;
        //public MainViewCommand MainViewCommand { get; set; }
        #endregion

        public MainViewModel()
        {
            DatabaseActionCommand = new RelayCommand<string>(ExecuteDatabaseActionCommand);
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
        public RelayCommand<string> DatabaseActionCommand { get; set; }

        public RelayCommand<string> MainViewCommand { get; set; }

        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; RaisePropertyChanged("FilePath"); }
        }

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
                    LoadViewAsync(ViewTypes.PurchaseOrder.ToString());
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

                case "Database":
                    LoadViewAsync(ViewTypes.Database.ToString());
                    break;
                case "DatabaseRestore":
                    ApplicationManager.Instance.ShowDialog("Restore Database", new RestoreDatabasePopup(this));
                    break;
                case "DatabaseBackup":
                    BackupDatabase();
                    break;
            }
        }

        public void ExecuteDatabaseActionCommand(string str)
        {
            switch (str)
            {
                case "Browse":
                    DatabaseFilePath();
                    break;
                case "Proceed":
                    RestoreDatabase();
                    break;
                case "Cancel":
                    ApplicationManager.Instance.HideDialog();
                    break;
                default:
                    break;
            }
        }

        private void DatabaseFilePath()
        {
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();

            // Launch OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = openFileDlg.ShowDialog();
            // Get the selected file name and display in a TextBox.
            // Load content of file in a TextBlock
            if (result == true)
            {
                FilePath = openFileDlg.FileName;
                //TextBlock1.Text = System.IO.File.ReadAllText(openFileDlg.FileName);
            }
        }

        public void RestoreDatabase()
        {
            var bw = new BackgroundWorker();
            bw.DoWork += (sender, args) =>
            {
                try
                {
                    ApplicationManager.Instance.ShowBusyInidicator("Restoring Database ...!");
                    using (var db = new HAFoodDbContext())
                    {
                        string restoreQuery = @"USE [Master]; 
                                                ALTER DATABASE ""{0}"" SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                                                RESTORE DATABASE ""{0}"" FROM DISK='{1}' WITH REPLACE;
                                                ALTER DATABASE ""{0}"" SET MULTI_USER;";
                        restoreQuery = string.Format(restoreQuery, "HAFoodDB", FilePath);
                        var list = db.Database.SqlQuery<object>(restoreQuery).ToList();
                        var resut = list.FirstOrDefault();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "HA Foods");
                }
            };

            bw.RunWorkerCompleted += async (sender, args) =>
            {
                ApplicationManager.Instance.HideBusyInidicator();
                ApplicationManager.Instance.ShowMessageBox("Backup Restore Successfully.");
            };
            bw.RunWorkerAsync();
        }

        public void BackupDatabase()
        {
            var bw = new BackgroundWorker();
            bw.DoWork += (sender, args) =>
            {
                try
                {
                    ApplicationManager.Instance.ShowBusyInidicator("Backup Database ...!");
                    var destination = @"C:\HAFood Database Backup";
                    using (var db = new HAFoodDbContext())
                    {
                        string backupQuery = @"BACKUP DATABASE ""{0}"" TO DISK = N'{1}' WITH FORMAT, MEDIANAME = 'SQLServerBackups', NAME = 'Full Backup of SQLTestDB'";
                        backupQuery = string.Format(backupQuery, "HAFoodDB", destination + @"\HAFOODDB_" + DateTime.Now.ToString("dd-MM-yyyy") + "_" + DateTime.Now.ToString("hh-mm-ss") + ".bak");
                        db.Database.SqlQuery<object>(backupQuery).ToList().FirstOrDefault();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "HA Foods");
                }
            };

            bw.RunWorkerCompleted += async (sender, args) =>
            {
                ApplicationManager.Instance.HideBusyInidicator();
                ApplicationManager.Instance.ShowMessageBox("Backup Database Successfully.");
            };
            bw.RunWorkerAsync();
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
                case ViewTypes.PurchaseOrder:
                    _viewManagerService.Select("home").Transition<View.PurchaseOrders.PurchaseOrder>(viewType);
                    break;
                case ViewTypes.User:
                    _viewManagerService.Select("home").Transition<View.Users.User>(viewType);
                    break;
                case ViewTypes.Database:
                    _viewManagerService.Select("home").Transition<View.Database.Database>(viewType);
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
            defaultScreen = "Home";
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
