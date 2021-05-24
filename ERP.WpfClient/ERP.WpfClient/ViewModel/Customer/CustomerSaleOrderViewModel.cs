using ERP.Common;
using ERP.Common.NotifyProperty;
using ERP.WpfClient.Controls.Helpers;
using ERP.WpfClient.Model.Customer;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.WpfClient.ViewModel.Customer
{
    public class CustomerSaleOrderViewModel : ViewModelBase
    {
        #region Fields

        private CustomerSaleOrderModel _customerSaleOrderModel;
        private ObservableCollection<CustomerSaleOrderModel> _customerSaleOrderList;
        private DateTime _selectedDate;

        #endregion

        #region Constructor

        public CustomerSaleOrderViewModel()
        {
            CustomerSaleOrderModel = new CustomerSaleOrderModel();
            SaleOrderCommand = new RelayCommand<string>(ExecuteSaleOrderCommand);
            Init();
        }

        #endregion

        #region Properties

        public RelayCommand<string> SaleOrderCommand { get; set; }

        public ObservableCollection<CustomerSaleOrderModel> CustomerSaleOrderList
        {
            get { return _customerSaleOrderList; }
            set { _customerSaleOrderList = value; RaisePropertyChanged("CustomerSaleOrderList"); }
        }

        public CustomerSaleOrderModel CustomerSaleOrderModel
        {
            get { return _customerSaleOrderModel; }
            set { _customerSaleOrderModel = value; RaisePropertyChanged("CustomerSaleOrderModel"); }
        }

        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                _selectedDate = value;
                GetCustomerSaleOrder(_selectedDate.ToString("yyyy-MM-dd"));
                RaisePropertyChanged("SelectedDate");
            }
        }

        private string _orderNos;

        public string OrderNos
        {
            get { return _orderNos; }
            set
            {
                _orderNos = value;
                if (_orderNos != null)
                {
                    GetSaleOrderDetails(_orderNos);
                }
                RaisePropertyChanged("OrderNos");
            }
        }

        private string _customerName;

        public string CustomerName
        {
            get { return _customerName; }
            set { _customerName = value; RaisePropertyChanged("CustomerName"); }
        }


        #endregion

        #region Methods

        public void ExecuteSaleOrderCommand(string str)
        {
            switch (str)
            {
                case "Print Sale Report":
                    PrintSaleReport();
                    break;
                default:
                    break;
            }
        }

        private void PrintSaleReport()
        {
            if (CustomerSaleOrderList.Count > 0)
            {
                ApplicationManager.Instance.PrintReport(CustomerSaleOrderList, @"/Reports/rptCustomerSaleOrder", "dsCustomerSaleOrder", "CustomerSaleOrder");
            }
            else
            {
                ApplicationManager.Instance.ShowMessageBox("No Record Found.");
            }
        }

        public void GetSaleOrderDetails(string orderNo)
        {
            //OrderNos = "";
            CustomerSaleOrderList = new ObservableCollection<CustomerSaleOrderModel>(CustomerSaleOrderList.Where(c => c.OrderNo.Contains(orderNo)).ToList());
            CustomerName = CustomerSaleOrderList.FirstOrDefault().CustomerName;
        }

        private void GetCustomerSaleOrder(string date)
        {
            DataTable dt = new DataTable();
            string constr = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\HAFood\HAFoodDB.mdf; Integrated Security = True;";
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("[dbo].[SP_GetCustomerSaleOrder]", con))
                {
                    cmd.Parameters.AddWithValue("@Date", date);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                }
            }

            CustomerSaleOrderList = new ObservableCollection<CustomerSaleOrderModel>();
            foreach (DataRow item in dt.Rows)
            {
                CustomerSaleOrderList.Add(new CustomerSaleOrderModel()
                {
                    OrderNo = item["OrderNo"].ToString(),
                    CustomerName = item["CustomerName"].ToString(),
                    ItemName = item["ItemName"].ToString(),
                    Quantity = Convert.ToInt32(item["Quantity"].ToString()),
                    Price = Convert.ToDecimal(item["Price"].ToString()),
                    Packing = Convert.ToInt32(item["Packing"].ToString()),
                    PerCarton = Convert.ToDecimal(item["PerCarton"].ToString()),
                    Discount = Convert.ToDecimal(item["Discount"].ToString()),
                    TotalPrice = Convert.ToDecimal(item["TotalPrice"].ToString()),
                    GrandTotalPrice = Convert.ToDecimal(item["GrandTotalPrice"].ToString()),
                    TotalDiscount = Convert.ToDecimal(item["TotalDiscount"].ToString()),
                    GrandTotal = Convert.ToDecimal(item["GrandTotal"].ToString()),
                    TotalAmount = Convert.ToDecimal(item["TotalAmount"].ToString()),
                    AmountPaid = Convert.ToDecimal(item["AmountPaid"].ToString()),
                    RemainingAmount = Convert.ToDecimal(item["RemainingAmount"].ToString()),
                    PreviousBalance = Convert.ToDecimal(item["PreviousBalance"].ToString()),
                    TransactionDate = Convert.ToDateTime(item["TransactionDate"].ToString())
                });
            }
        }

        private void Init()
        {
            SelectedDate = DateTime.Now;
        }

        #endregion
    }
}
