using ERP.Common;
using ERP.Common.NotifyProperty;
using ERP.Entities.DBModel.Transactions;
using ERP.Repository.Generic;
using ERP.WpfClient.Model.Transaction;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace ERP.WpfClient.ViewModel.Home
{
    public class HomeViewModel : ViewModelBase, INotifyOnBringIntoView
    {
        #region Fields

        private readonly IGenericRepository<CurrentTransaction> _currentTransactionRepository;
        private readonly IGenericRepository<Entities.DBModel.Customers.Customer> _customerRepository;
        private readonly IGenericRepository<Entities.DBModel.Suppliers.Supplier> _supplierRepository;
        private readonly IGenericRepository<Entities.DBModel.Stocks.Stock> _stockRepository;
        private string _totalCustomers;
        private string _totalSuppliers;
        private string _totalOrders;
        private string _totalStocks;

        #endregion

        #region Properties
        public string TotalCustomers
        {
            get { return _totalCustomers; }
            set { _totalCustomers = value; RaisePropertyChanged("TotalCustomers"); }
        }

        public string TotalSuppliers
        {
            get { return _totalSuppliers; }
            set { _totalSuppliers = value; RaisePropertyChanged("TotalSuppliers"); }
        }

        public string TotalOrders
        {
            get { return _totalOrders; }
            set { _totalOrders = value; RaisePropertyChanged("TotalOrders"); }
        }

        public string TotalStocks
        {
            get { return _totalStocks; }
            set { _totalStocks = value; RaisePropertyChanged("TotalStocks"); }
        }

        private ObservableCollection<RecentTransactionModel> _recentTransactionList;

        public ObservableCollection<RecentTransactionModel> RecentTransactionList
        {
            get { return _recentTransactionList; }
            set { _recentTransactionList = value; RaisePropertyChanged("RecentTransactionList"); }
        }


        #endregion

        #region Constructor
        public HomeViewModel()
        {
            TotalCustomers = "0";
            TotalSuppliers = "0";
            _currentTransactionRepository = App.Resolve<IGenericRepository<CurrentTransaction>>();
            _customerRepository = App.Resolve<IGenericRepository<Entities.DBModel.Customers.Customer>>();
            _supplierRepository = App.Resolve<IGenericRepository<Entities.DBModel.Suppliers.Supplier>>();
            _stockRepository = App.Resolve<IGenericRepository<Entities.DBModel.Stocks.Stock>>();
        }
        #endregion

        #region Methods

        public void GetTotalCustomers()
        {

            for (int i = 0; i <= _customerRepository.Get().Count; i++)
            {
                Thread.Sleep(50);
                Dispatcher.CurrentDispatcher.Invoke(new Action(() => {
                    TotalCustomers = i.ToString();
                }));
            }
        }

        public void GetTotalSuppliers()
        {

            for (int i = 0; i <= _supplierRepository.Get().Count; i++)
            {
                Thread.Sleep(100);
                Dispatcher.CurrentDispatcher.Invoke(new Action(() => {
                    TotalSuppliers = i.ToString();
                }));
            }
        }

        public void GetTotalOrders()
        {

            for (int i = 0; i <= _currentTransactionRepository.Get().Count; i++)
            {
                Thread.Sleep(100);
                Dispatcher.CurrentDispatcher.Invoke(new Action(() => {
                    TotalOrders = i.ToString();
                }));
            }
        }
        
        public void GetTotalStocks()
        {

            for (int i = 0; i <= _stockRepository.Get().Count; i++)
            {
                Thread.Sleep(100);
                Dispatcher.CurrentDispatcher.Invoke(new Action(() => {
                    TotalStocks = i.ToString();
                }));
            }
        }

        private void GetRecentOrders()
        {
            DataTable dt = new DataTable();
            string constr = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\HAFood\HAFoodDB.mdf; Integrated Security = True;";
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("[dbo].[SP_GetTopOrders]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                }
            }

            RecentTransactionList = new ObservableCollection<RecentTransactionModel>();
            foreach (DataRow item in dt.Rows)
            {
                RecentTransactionList.Add(new RecentTransactionModel()
                {
                    CustomerName = item["CustomerName"].ToString(),
                    GrandTotal = item["GrandTotal"].ToString(),
                    OrderNo = item["OrderNo"].ToString(),
                    PaymentType = item["PaymentType"].ToString()
                });
            }
        }

        public void OnBringIntoView()
        {
            Thread customerThread = new Thread(new ThreadStart(GetTotalCustomers));
            Thread supplierThread = new Thread(new ThreadStart(GetTotalSuppliers));
            Thread orderThread = new Thread(new ThreadStart(GetTotalOrders));
            Thread stockThread = new Thread(new ThreadStart(GetTotalStocks));
            customerThread.Start();
            supplierThread.Start();
            orderThread.Start();
            stockThread.Start();
            GetRecentOrders();
            //Task.Factory.StartNew(() =>
            //{
            //    GetTotalCustomers();
            //});
            //throw new NotImplementedException();
        }

        #endregion
    }
}
