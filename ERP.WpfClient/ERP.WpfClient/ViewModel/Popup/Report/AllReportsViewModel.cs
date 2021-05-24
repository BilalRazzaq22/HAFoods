using ERP.Common;
using ERP.Common.NotifyProperty;
using ERP.Entities.DbContext;
using ERP.Entities.DBModel.Payments;
using ERP.Entities.DBModel.Transactions;
using ERP.Repository.Customer;
using ERP.Repository.Generic;
using ERP.Repository.Transaction;
using ERP.WpfClient.Controls.Helpers;
using ERP.WpfClient.Mapper;
using ERP.WpfClient.Model;
using ERP.WpfClient.Model.CashBooks;
using ERP.WpfClient.Model.Stock;
using ERP.WpfClient.Model.Supplier;
using ERP.WpfClient.View.Popups.Reports;
using ERP.WpfClient.View.Popups.Reports.Customer;
using ERP.WpfClient.View.Popups.Reports.DailySale;
using ERP.WpfClient.View.Popups.Reports.Item;
using ERP.WpfClient.View.Popups.Reports.Ledger;
using ERP.WpfClient.View.Popups.Reports.Supplier;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ERP.WpfClient.ViewModel.Popup.Report
{
    public class AllReportsViewModel : ViewModelBase, INotifyOnBringIntoView
    {
        #region Fields

        private readonly CurrentTransactionRepository _currentTransactionRepository;
        private readonly IGenericRepository<CurrentTransactionDetail> _currentTransactionDetailRepository;
        private readonly IGenericRepository<Entities.DBModel.Customers.Customer> _customerRepository;
        private IGenericRepository<Entities.DBModel.Stocks.Stock> _stockRepository;
        private readonly IGenericRepository<Payment> _paymentRepository;
        private readonly CustomerOrderRepository _customerOrderRepository;
        private readonly IGenericRepository<Entities.DBModel.Suppliers.Supplier> _supplierRepository;
        private bool _isCustomerSelected;
        private bool _isSupplierSelected;
        private bool _isItemSelected;
        private DateTime _fromDate;
        private DateTime _toDate;
        private CustomerModel _customerModel;
        private ObservableCollection<CustomerModel> _customerList;
        private StockModel _stockModel;
        private ObservableCollection<StockModel> _stockList;
        private string _reportName;
        private bool _isCreditSelected;
        private bool _isAlertQuantitySelected;
        private bool _isAllCategorySelected;
        private bool _isCategorySelected;
        private bool _isAllCustomerSelected;
        private bool _isAllSupplierSelected;
        private ObservableCollection<CashBookTypeModel> _cashBookTypeList;
        private CashBookTypeModel _cashBookType;
        private ObservableCollection<SupplierModel> _supplierList;
        private SupplierModel _supplierModel;
        private string _isCustomer;
        private string _isSupplier;

        #endregion

        #region Constructor
        public AllReportsViewModel()
        {
            ReportsCommand = new RelayCommand<string>(ExecuteReportsCommand);
            ReportActionCommand = new RelayCommand<string>(ExecuteReportActionCommand);
            _currentTransactionRepository = new CurrentTransactionRepository(new HAFoodDbContext());
            _currentTransactionDetailRepository = App.Resolve<IGenericRepository<CurrentTransactionDetail>>();
            _customerRepository = App.Resolve<IGenericRepository<Entities.DBModel.Customers.Customer>>();
            _stockRepository = new GenericRepository<Entities.DBModel.Stocks.Stock>(new HAFoodDbContext());
            _paymentRepository = App.Resolve<IGenericRepository<Payment>>();
            _customerOrderRepository = new CustomerOrderRepository(new HAFoodDbContext());
            _supplierRepository = App.Resolve<IGenericRepository<Entities.DBModel.Suppliers.Supplier>>();
            CustomerList = new ObservableCollection<CustomerModel>();
            CustomerModel = new CustomerModel();
            StockList = new ObservableCollection<StockModel>();
            StockModel = new StockModel();
        }
        #endregion

        #region Properties

        public RelayCommand<string> ReportsCommand { get; set; }
        public RelayCommand<string> ReportActionCommand { get; set; }

        public bool IsCustomerSelected
        {
            get { return _isCustomerSelected; }
            set { _isCustomerSelected = value; RaisePropertyChanged("IsCustomerSelected"); }
        }

        public bool IsAllCustomerSelected
        {
            get { return _isAllCustomerSelected; }
            set { _isAllCustomerSelected = value; RaisePropertyChanged("IsAllCustomerSelected"); }
        }

        public bool IsSupplierSelected
        {
            get { return _isSupplierSelected; }
            set { _isSupplierSelected = value; RaisePropertyChanged("IsSupplierSelected"); }
        }

        public bool IsAllSupplierSelected
        {
            get { return _isAllSupplierSelected; }
            set { _isAllSupplierSelected = value; RaisePropertyChanged("IsAllSupplierSelected"); }
        }

        public bool IsItemSelected
        {
            get { return _isItemSelected; }
            set { _isItemSelected = value; RaisePropertyChanged("IsItemSelected"); }
        }

        public DateTime FromDate
        {
            get { return _fromDate; }
            set { _fromDate = value; RaisePropertyChanged("FromDate"); }
        }

        public DateTime ToDate
        {
            get { return _toDate; }
            set { _toDate = value; RaisePropertyChanged("ToDate"); }
        }

        public CustomerModel CustomerModel
        {
            get { return _customerModel; }
            set { _customerModel = value; RaisePropertyChanged("CustomerModel"); }
        }

        public ObservableCollection<CustomerModel> CustomerList
        {
            get { return _customerList; }
            set { _customerList = value; RaisePropertyChanged("CustomerList"); }
        }

        public StockModel StockModel
        {
            get { return _stockModel; }
            set { _stockModel = value; RaisePropertyChanged("StockModel"); }
        }

        public ObservableCollection<StockModel> StockList
        {
            get { return _stockList; }
            set { _stockList = value; RaisePropertyChanged("StockList"); }
        }

        public string ReportName
        {
            get { return _reportName; }
            set { _reportName = value; RaisePropertyChanged("ReportName"); }
        }

        public bool IsCreditSelected
        {
            get { return _isCreditSelected; }
            set { _isCreditSelected = value; RaisePropertyChanged("IsCreditSelected"); }
        }

        public bool IsAlertQuantitySelected
        {
            get { return _isAlertQuantitySelected; }
            set { _isAlertQuantitySelected = value; RaisePropertyChanged("IsAlertQuantitySelected"); }
        }

        public bool IsAllCategorySelected
        {
            get { return _isAllCategorySelected; }
            set { _isAllCategorySelected = value; RaisePropertyChanged("IsAllCategorySelected"); }
        }

        public bool IsCategorySelected
        {
            get { return _isCategorySelected; }
            set { _isCategorySelected = value; RaisePropertyChanged("IsCategorySelected"); }
        }

        public CashBookTypeModel CashBookType
        {
            get { return _cashBookType; }
            set
            {
                _cashBookType = value;
                if (_cashBookType != null)
                    GetType(_cashBookType);
                RaisePropertyChanged("CashBookType");
            }
        }

        public SupplierModel SupplierModel
        {
            get { return _supplierModel; }
            set { _supplierModel = value; RaisePropertyChanged("SupplierModel"); }
        }

        public ObservableCollection<SupplierModel> SupplierList
        {
            get { return _supplierList; }
            set { _supplierList = value; RaisePropertyChanged("SupplierList"); }
        }

        public ObservableCollection<CashBookTypeModel> CashBookTypeList
        {
            get { return _cashBookTypeList; }
            set { _cashBookTypeList = value; RaisePropertyChanged("CashBookTypeList"); }
        }

        public string IsCustomer
        {
            get { return _isCustomer; }
            set { _isCustomer = value; RaisePropertyChanged("IsCustomer"); }
        }

        public string IsSupplier
        {
            get { return _isSupplier; }
            set { _isSupplier = value; RaisePropertyChanged("IsSupplier"); }
        }

        #endregion

        #region Methods

        private void ExecuteReportActionCommand(string command)
        {
            switch (command)
            {
                case "Proceed":
                    if (ReportName == "DailySaleReport")
                        GenerateDailySaleReport();
                    if (ReportName == "LedgerReport")
                        GenerateLedgerReport();
                    if (ReportName == "ItemReport")
                        GenerateItemListReport();
                    if (ReportName == "CustomerReport")
                        GenerateCustomerReport();
                    if (ReportName == "SupplierReport")
                        GenerateSupplierReport();
                    break;
                case "Cancel":
                    ApplicationManager.Instance.HideDialog();
                    break;

            }
        }

        private void ExecuteReportsCommand(string command)
        {
            ReportName = "";
            switch (command)
            {
                case "CustomerReport":
                    ApplicationManager.Instance.ShowDialog("Customers Report", new CustomerReportPopup(this));
                    ReportName = "CustomerReport";
                    break;

                case "SupplierReport":
                    ApplicationManager.Instance.ShowDialog("Supplier Report", new SupplierReportPopup(this));
                    ReportName = "SupplierReport";
                    break;

                case "DailySaleReport":
                    ApplicationManager.Instance.ShowDialog("Daily Sale Report", new DailySaleReportPopup(this));
                    ReportName = "DailySaleReport";
                    break;

                case "ItemReport":
                    ApplicationManager.Instance.ShowDialog("Item List Report", new ItemReportPopup(this));
                    ReportName = "ItemReport";
                    break;

                case "LedgerReport":
                    ApplicationManager.Instance.ShowDialog("Ledger Report", new LedgerReportPopup(this));
                    ReportName = "LedgerReport";
                    break;
            }
        }

        private void Reset()
        {
            FromDate = DateTime.Now;
            ToDate = DateTime.Now;
            GetCustomer();
            GetItems();
            GetCashBookType();
            GetSupplier();
        }

        private void GetCustomer()
        {
            CustomerModel = CustomerList.FirstOrDefault();
        }

        private void GetItems()
        {
            StockModel = StockList.FirstOrDefault();
        }

        private void GetSupplier()
        {
            SupplierModel = SupplierList.FirstOrDefault();
        }

        private void LoadCollections()
        {
            CustomerList = MapperProfile.iMapper.Map<ObservableCollection<CustomerModel>>(_customerRepository.Get());
            StockList = MapperProfile.iMapper.Map<ObservableCollection<StockModel>>(_stockRepository.Get());
            SupplierList = MapperProfile.iMapper.Map<ObservableCollection<SupplierModel>>(_supplierRepository.Get());
            InitializeCashBookTypeList();
        }

        private void InitializeCashBookTypeList()
        {
            CashBookTypeList = new ObservableCollection<CashBookTypeModel>();

            CashBookTypeList.Add(new CashBookTypeModel() { Type = "Select Type" });
            CashBookTypeList.Add(new CashBookTypeModel() { Type = "Customer" });
            CashBookTypeList.Add(new CashBookTypeModel() { Type = "Supplier" });
        }

        private void GetCashBookType()
        {
            CashBookType = CashBookTypeList.FirstOrDefault();
        }

        private void GetType(CashBookTypeModel cashBookType)
        {
            if (cashBookType.Type != null)
            {
                if (cashBookType.Type == "Select Type")
                {
                    //GetCustomer();
                    SupplierModel = new SupplierModel();
                    CustomerModel = new CustomerModel();
                    IsCustomer = Visibility.Collapsed.ToString();
                    IsSupplier = Visibility.Collapsed.ToString();
                }
                if (cashBookType.Type == "Customer")
                {
                    GetCustomer();
                    SupplierModel = new SupplierModel();
                    IsCustomer = Visibility.Visible.ToString();
                    IsSupplier = Visibility.Collapsed.ToString();
                }
                if (cashBookType.Type == "Supplier")
                {
                    GetSupplier();
                    CustomerModel = new CustomerModel();
                    IsSupplier = Visibility.Visible.ToString();
                    IsCustomer = Visibility.Collapsed.ToString();
                }
            }
        }


        private void Init()
        {
            var bw = new BackgroundWorker();
            bw.DoWork += (sender, args) =>
            {
                try
                {
                    ApplicationManager.Instance.ShowBusyInidicator("Loading Data... !");
                    LoadCollections();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Post Error\nMessage: " + ex.Message, "HA Foods");
                }
            };

            bw.RunWorkerCompleted += async (sender, args) =>
            {
                Reset();
                ApplicationManager.Instance.HideBusyInidicator();
            };

            bw.RunWorkerAsync();

        }

        private void GenerateDailySaleReport()
        {
            DataTable dt = new DataTable();
            string constr = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\HAFood\HAFoodDB.mdf; Integrated Security = True;";
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("[dbo].[SP_GetDailySalesReport]", con))
                {
                    cmd.Parameters.AddWithValue("@FromDate", FromDate.Date.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@ToDate", ToDate.Date.ToString("yyyy-MM-dd"));
                    if (IsCustomerSelected)
                        cmd.Parameters.AddWithValue("@CustomerId", CustomerModel.Id);
                    if (IsItemSelected)
                        cmd.Parameters.AddWithValue("@StockId", StockModel.Id);
                    if (IsCreditSelected)
                        cmd.Parameters.AddWithValue("@PaymentType", "Credit");
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                }
            }
            if (dt.Rows.Count > 0)
            {
                ApplicationManager.Instance.PrintReport(dt, @"/Reports/rptDailySale", "dsDailySale", "DailySale");
            }
            else
            {
                ApplicationManager.Instance.ShowMessageBox("No Record Found.");
            }
        }

        private void GenerateLedgerReport()
        {
            if (CashBookType.Type == "Select Type")
            {
                ApplicationManager.Instance.ShowMessageBox("Please select one option.");
                return;
            }

            DataTable dt = new DataTable();
            string constr = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\HAFood\HAFoodDB.mdf; Integrated Security = True;";
            if (CashBookType.Type == "Customer")
            {
                using (SqlConnection con = new SqlConnection(constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("[dbo].[SP_GetCurrentTransactionLedgerReport]", con))
                    {
                        cmd.Parameters.AddWithValue("@FromDate", FromDate.Date.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@ToDate", ToDate.Date.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@CustomerId", CustomerModel.Id);
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        da.Fill(dt);
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    ApplicationManager.Instance.PrintReport(dt, @"/Reports/rptCurrentTransactionLedgerReport", "dsCurrentTransactionLedgerReport", "CustomerLedgerReport");
                }
                else
                {
                    ApplicationManager.Instance.ShowMessageBox("No Record Found.");
                }
            }

            if (CashBookType.Type == "Supplier")
            {
                using (SqlConnection con = new SqlConnection(constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("[dbo].[SP_GetPurchaseOrderLedgerReport]", con))
                    {
                        cmd.Parameters.AddWithValue("@FromDate", FromDate.Date.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@ToDate", ToDate.Date.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@SupplierId", SupplierModel.Id);
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        da.Fill(dt);
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    ApplicationManager.Instance.PrintReport(dt, @"/Reports/rptPurchaseOrderLedgerReport", "dsPurchaseOrderLedgerReport", "SupplierLedgerReport");
                }
                else
                {
                    ApplicationManager.Instance.ShowMessageBox("No Record Found.");
                }
            }
        }

        private void GenerateItemListReport()
        {
            if (!IsAlertQuantitySelected && !IsAllCategorySelected && !IsCategorySelected)
            {
                ApplicationManager.Instance.ShowMessageBox("Please select one option.");
                return;
            }
            DataTable dt = new DataTable();
            string constr = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\HAFood\HAFoodDB.mdf; Integrated Security = True;";
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("[dbo].[SP_GetItemList]", con))
                {
                    if (IsAlertQuantitySelected)
                        cmd.Parameters.AddWithValue("@CustomerId", 50);
                    if (IsAllCategorySelected)
                        cmd.Parameters.AddWithValue("@Category", null);
                    if (IsCategorySelected)
                        cmd.Parameters.AddWithValue("@Category", StockModel.Category);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                }
            }
            if (dt.Rows.Count > 0)
            {
                ApplicationManager.Instance.PrintReport(dt, @"/Reports/rptItemList", "dsItemList", "ItemList");
            }
            else
            {
                ApplicationManager.Instance.ShowMessageBox("No Record Found.");
            }
        }

        private void GenerateCustomerReport()
        {
            if (!IsCustomerSelected && !IsAllCustomerSelected)
            {
                ApplicationManager.Instance.ShowMessageBox("Please select one option.");
                return;
            }
            DataTable dt = new DataTable();
            string constr = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\HAFood\HAFoodDB.mdf; Integrated Security = True;";
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("[dbo].[SP_GetCustomerReport]", con))
                {
                    if (IsCustomerSelected)
                        cmd.Parameters.AddWithValue("@CustomerId", CustomerModel.Id);
                    if (IsAllCustomerSelected)
                        cmd.Parameters.AddWithValue("@CustomerId", null);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                }
            }
            if (dt.Rows.Count > 0)
            {
                ApplicationManager.Instance.PrintReport(dt, @"/Reports/rptCustomer", "dsCustomer", "Customer");
            }
            else
            {
                ApplicationManager.Instance.ShowMessageBox("No Record Found.");
            }
        }

        private void GenerateSupplierReport()
        {
            if (!IsSupplierSelected && !IsAllSupplierSelected)
            {
                ApplicationManager.Instance.ShowMessageBox("Please select one option.");
                return;
            }
            DataTable dt = new DataTable();
            string constr = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\HAFood\HAFoodDB.mdf; Integrated Security = True;";
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("[dbo].[SP_GetSupplierReport]", con))
                {
                    if (IsSupplierSelected)
                        cmd.Parameters.AddWithValue("@SupplierId", SupplierModel.Id);
                    if (IsAllSupplierSelected)
                        cmd.Parameters.AddWithValue("@SupplierId", null);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                }
            }
            if (dt.Rows.Count > 0)
            {
                ApplicationManager.Instance.PrintReport(dt, @"/Reports/rptSupplier", "dsSupplier", "Supplier");
            }
            else
            {
                ApplicationManager.Instance.ShowMessageBox("No Record Found.");
            }
        }

        public void OnBringIntoView()
        {
            Init();
        }
        #endregion

    }
}
