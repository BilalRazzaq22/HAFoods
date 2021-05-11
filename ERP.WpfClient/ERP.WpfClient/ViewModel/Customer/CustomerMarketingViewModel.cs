using ERP.Common;
using ERP.Common.NotifyProperty;
using ERP.Entities.DbContext;
using ERP.Entities.DBModel.CashBook;
using ERP.Entities.DBModel.Customers;
using ERP.Entities.DBModel.Payments;
using ERP.Repository.Customer;
using ERP.Repository.Generic;
using ERP.WpfClient.Controls.Helpers;
using ERP.WpfClient.Mapper;
using ERP.WpfClient.Model;
using ERP.WpfClient.Model.Customer;
using ERP.WpfClient.Model.Payment;
using ERP.WpfClient.Model.Stock;
using ERP.WpfClient.View.Popups.Payments;
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

namespace ERP.WpfClient.ViewModel.Customer
{
    public class CustomerMarketingViewModel : ViewModelBase
    {
        #region Fields

        private readonly CustomerMarketingBillRepository _customerMarketingBillRepository;
        private readonly IGenericRepository<CustomerMarketingOrderItem> _customerMarketingOrderItemRepository;
        private readonly IGenericRepository<Entities.DBModel.Customers.Customer> _customerRepository;
        private IGenericRepository<Entities.DBModel.Stocks.Stock> _stockRepository;
        private readonly IGenericRepository<Payment> _paymentRepository;
        private readonly IGenericRepository<CashBookOne> _cashBookOneRepository;
        private readonly IGenericRepository<CustomerMarketingOrderAmount> _customerMarketingOrderAmountRepository;
        private CustomerMarketingOrderModel _customerMarketingOrderModel;
        private ObservableCollection<CustomerMarketingOrderModel> _customerMarketingOrderList;
        private CustomerMarketingOrderItemModel _customerMarketingOrderItemModel;
        private ObservableCollection<CustomerMarketingOrderItemModel> _customerMarketingOrderItemList;
        private CustomerModel _customerModel;
        private ObservableCollection<CustomerModel> _customerList;
        private StockModel _stockModel;
        private ObservableCollection<StockModel> _stockList;
        private ObservableCollection<PaymentModel> _paymentList;
        private PaymentModel _paymentType;
        private string _orderNumber;
        private int _quantity;
        private decimal _discount;
        private decimal _totalPrice;
        private decimal _totalDiscount;
        private decimal _grandTotal;
        private bool _isPreviousOrder = false;
        private bool _isExistingCustomer = false;
        private int _customerMarketingOrderId;
        private CustomerOrderModel _customerOrderModel;
        private decimal _amountPaid;
        private decimal _remainingAmount;
        private decimal _newItemPrice;
        private decimal _customerCashBookAmount;

        //CustomerMarketingBillRepository currentTransaction = new CustomerMarketingBillRepository();

        #endregion

        #region Ctor

        public CustomerMarketingViewModel()
        {
            CurrentOrderCommand = new RelayCommand<string>(ExecuteCurrentOrderCommand);
            _customerMarketingBillRepository = new CustomerMarketingBillRepository(new HAFoodDbContext());
            _customerMarketingOrderItemRepository = App.Resolve<IGenericRepository<CustomerMarketingOrderItem>>();
            _customerRepository = App.Resolve<IGenericRepository<Entities.DBModel.Customers.Customer>>();
            _stockRepository = new GenericRepository<Entities.DBModel.Stocks.Stock>(new HAFoodDbContext());
            _paymentRepository = App.Resolve<IGenericRepository<Payment>>();
            _customerMarketingOrderAmountRepository = App.Resolve<IGenericRepository<CustomerMarketingOrderAmount>>();
            _cashBookOneRepository = App.Resolve<IGenericRepository<CashBookOne>>();
            StockList = new ObservableCollection<StockModel>();
            CustomerList = new ObservableCollection<CustomerModel>();
            Init();
        }

        #endregion

        #region Properties
        public RelayCommand<string> CurrentOrderCommand { get; set; }

        public decimal GrandTotal
        {
            get { return _grandTotal; }
            set { _grandTotal = value; RaisePropertyChanged("GrandTotal"); }
        }

        public decimal TotalDiscount
        {
            get { return _totalDiscount; }
            set { _totalDiscount = value; RaisePropertyChanged("TotalDiscount"); }
        }

        public decimal TotalPrice
        {
            get { return _totalPrice; }
            set { _totalPrice = value; RaisePropertyChanged("TotalPrice"); }
        }

        public string OrderNumber
        {
            get { return _orderNumber; }
            set { _orderNumber = value; RaisePropertyChanged("OrderNumber"); }
        }

        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; RaisePropertyChanged("Quantity"); }
        }

        public decimal Discount
        {
            get { return _discount; }
            set { _discount = value; RaisePropertyChanged("Discount"); }
        }

        public CustomerMarketingOrderModel CustomerMarketingOrderModel
        {
            get { return _customerMarketingOrderModel; }
            set { _customerMarketingOrderModel = value; RaisePropertyChanged("CustomerMarketingOrderModel"); }
        }

        public CustomerMarketingOrderItemModel CustomerMarketingOrderItemModel
        {
            get { return _customerMarketingOrderItemModel; }
            set { _customerMarketingOrderItemModel = value; RaisePropertyChanged("CustomerMarketingOrderItemModel"); }
        }

        public ObservableCollection<CustomerMarketingOrderItemModel> CustomerMarketingOrderItemList
        {
            get { return _customerMarketingOrderItemList; }
            set { _customerMarketingOrderItemList = value; RaisePropertyChanged("CustomerMarketingOrderItemList"); }
        }

        public ObservableCollection<CustomerMarketingOrderModel> CustomerMarketingOrderList
        {
            get { return _customerMarketingOrderList; }
            set { _customerMarketingOrderList = value; RaisePropertyChanged("CustomerMarketingOrderList"); }
        }

        public CustomerModel CustomerModel
        {
            get { return _customerModel; }
            set
            {
                _customerModel = value;
                if (_customerModel != null)
                    GetCustomerMarketingAmount(_customerModel.Id);
                RaisePropertyChanged("CustomerModel");
            }
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

        public PaymentModel PaymentType
        {
            get { return _paymentType; }
            set { _paymentType = value; RaisePropertyChanged("PaymentType"); }
        }

        public ObservableCollection<PaymentModel> PaymentList
        {
            get { return _paymentList; }
            set { _paymentList = value; RaisePropertyChanged("PaymentList"); }
        }

        public CustomerOrderModel CustomerOrderModel
        {
            get { return _customerOrderModel; }
            set { _customerOrderModel = value; RaisePropertyChanged("CustomerOrderModel"); }
        }

        public decimal AmountPaid
        {
            get { return _amountPaid; }
            set { _amountPaid = value; RaisePropertyChanged("AmountPaid"); }
        }

        public decimal RemainingAmount
        {
            get { return _remainingAmount; }
            set { _remainingAmount = value; RaisePropertyChanged("RemainingAmount"); }
        }

        public decimal NewItemPrice
        {
            get { return _newItemPrice; }
            set { _newItemPrice = value; RaisePropertyChanged("NewItemPrice"); }
        }

        public decimal CustomerCashBookAmount
        {
            get { return _customerCashBookAmount; }
            set { _customerCashBookAmount = value; RaisePropertyChanged("CustomerCashBookAmount"); }
        }
        #endregion

        #region Methods

        private void ExecuteCurrentOrderCommand(string str)
        {

            if (str == "Add Item")
            {
                if (Quantity == 0)
                {
                    ApplicationManager.Instance.ShowMessageBox("Please add Quantity");
                    return;
                }

                CustomerMarketingOrderItemModel = CustomerMarketingOrderItemList.Where(x => x.ItemName == StockModel.ItemName).FirstOrDefault();

                if (CustomerMarketingOrderItemModel != null)
                {
                    CustomerMarketingOrderItemModel.ItemName = StockModel.ItemName;
                    CustomerMarketingOrderItemModel.Price = StockModel.SalePrice;
                    //if (_isPreviousOrder)
                    //{
                    CustomerMarketingOrderItemModel.NewQuantity = CustomerMarketingOrderItemModel.NewQuantity + Quantity;
                    //}
                    CustomerMarketingOrderItemModel.Quantity = Quantity;
                    CustomerMarketingOrderItemModel.NewDiscount = CustomerMarketingOrderItemModel.NewDiscount + Discount;
                    CustomerMarketingOrderItemModel.Discount = Discount;
                    decimal totalPrice = CustomerMarketingOrderItemModel.Price * StockModel.Packing * CustomerMarketingOrderItemModel.NewQuantity;
                    CustomerMarketingOrderItemModel.TotalPrice = totalPrice - CustomerMarketingOrderItemModel.NewDiscount;
                }
                else
                {
                    CustomerMarketingOrderItemList.Add(new CustomerMarketingOrderItemModel()
                    {
                        StockId = StockModel.Id,
                        ItemName = StockModel.ItemName,
                        Packing = StockModel.Packing,
                        Price = StockModel.SalePrice,
                        Quantity = Quantity,
                        NewQuantity = Quantity,
                        Discount = Discount,
                        NewDiscount = Discount,
                        TotalPrice = ((StockModel.SalePrice * StockModel.Packing) * Quantity) - Discount
                    });
                }

                decimal price = CustomerMarketingOrderItemList.Sum(x => x.TotalPrice);

                CustomerMarketingOrderModel.TotalPrice = price;
                CustomerMarketingOrderModel.TotalDiscount = CustomerMarketingOrderItemList.Sum(x => x.NewDiscount);
                if (_isPreviousOrder)
                {
                    //if (CustomerMarketingOrderItemModel != null)
                    //{
                    //    CustomerMarketingOrderModel.GrandTotal = ((((CustomerMarketingOrderItemModel != null) ? (CustomerMarketingOrderItemModel.Price * StockModel.Packing) : (StockModel.SalePrice * StockModel.Packing)) * Quantity) + CustomerOrderModel.RemainingAmount) - Discount;
                    //}
                    //else
                    //{
                    var newItemTotalPrice = (StockModel.SalePrice * StockModel.Packing) * Quantity;
                    CustomerMarketingOrderModel.GrandTotal += newItemTotalPrice;
                    //}
                    NewItemPrice += ((CustomerMarketingOrderItemModel != null) ? CustomerMarketingOrderItemModel.Price * StockModel.Packing : StockModel.SalePrice * StockModel.Packing) * Quantity;
                }
                else
                    CustomerMarketingOrderModel.GrandTotal = (CustomerMarketingOrderModel.TotalPrice + CustomerOrderModel.RemainingAmount) - CustomerMarketingOrderModel.TotalDiscount - CustomerCashBookAmount;
            }
            else if (str == "Save Order")
            {
                var bw = new BackgroundWorker();
                bw.DoWork += (sender, args) =>
                {
                    try
                    {
                        ApplicationManager.Instance.ShowBusyInidicator("Saving Order ... !");

                        CustomerMarketingOrder customerMarketingOrder = new CustomerMarketingOrder();

                        if (_isPreviousOrder)
                        {
                            customerMarketingOrder.Id = _customerMarketingOrderId;
                            customerMarketingOrder.UpdatedDate = DateTime.Now;
                        }
                        else
                        {
                            customerMarketingOrder.CreatedDate = DateTime.Now;
                        }
                        customerMarketingOrder.OrderNo = CustomerMarketingOrderModel.OrderNo;
                        customerMarketingOrder.CustomerId = CustomerModel.Id;
                        customerMarketingOrder.PaymentId = PaymentType.Id;
                        customerMarketingOrder.TotalPrice = CustomerMarketingOrderModel.TotalPrice;
                        customerMarketingOrder.TotalDiscount = CustomerMarketingOrderModel.TotalDiscount;
                        customerMarketingOrder.GrandTotal = CustomerMarketingOrderModel.GrandTotal;
                        customerMarketingOrder.AmountPaid = CustomerMarketingOrderModel.AmountPaid;
                        customerMarketingOrder.CreatedDate = DateTime.Now;

                        foreach (var item in CustomerMarketingOrderItemList)
                        {
                            CustomerMarketingOrderItem customerMarketingOrderItem = new CustomerMarketingOrderItem();
                            if (_isPreviousOrder)
                            {
                                customerMarketingOrderItem.Id = item.Id;
                                customerMarketingOrderItem.UpdatedDate = DateTime.Now;
                            }
                            else
                            {
                                customerMarketingOrderItem.CreatedDate = DateTime.Now;
                            }
                            customerMarketingOrderItem.StockId = item.StockId;
                            customerMarketingOrderItem.ItemName = item.ItemName;
                            customerMarketingOrderItem.Packing = item.Packing;
                            //if (!_isPreviousOrder)
                            //    customerMarketingOrderItem.PreviousQuantity = item.PreviousQuantity;
                            customerMarketingOrderItem.Quantity = item.NewQuantity;
                            customerMarketingOrderItem.Price = item.Price;
                            customerMarketingOrderItem.Discount = item.Discount;
                            customerMarketingOrderItem.TotalPrice = item.TotalPrice;
                            customerMarketingOrder.CustomerMarketingOrderItems.Add(customerMarketingOrderItem);
                        }

                        SaveCustomerMarketingAmount();

                        CustomerMarketingOrder t = _customerMarketingBillRepository.SaveCustomerOrder(customerMarketingOrder);

                        CalculateStock(CustomerMarketingOrderItemList);
                        LoadReport(t);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Post Error\nMessage: " + ex.Message, "HA Foods");
                    }
                };

                bw.RunWorkerCompleted += async (sender, args) =>
                {
                    ApplicationManager.Instance.HideBusyInidicator();

                    ApplicationManager.Instance.ShowDialog("Order Saved!", new PaymentPopup(this));
                    Reset();
                };

                bw.RunWorkerAsync();

            }

            else if (str == "Search Order")
            {
                CustomerMarketingOrderItemList = new ObservableCollection<CustomerMarketingOrderItemModel>();
                var result = _customerMarketingBillRepository.GetOrder(OrderNumber);
                if (result != null)
                {
                    _isPreviousOrder = true;

                    GetCustomerMarketingAmount(Convert.ToInt32(result.CustomerId));

                    _customerMarketingOrderId = result.Id;
                    CustomerMarketingOrderModel.OrderNo = result.OrderNo;
                    CustomerMarketingOrderModel.TotalPrice = result.TotalPrice;
                    CustomerMarketingOrderModel.TotalDiscount = result.TotalDiscount;
                    CustomerMarketingOrderModel.GrandTotal = CustomerOrderModel.RemainingAmount;//result.GrandTotal;

                    foreach (var item in result.CustomerMarketingOrderItems)
                    {
                        CustomerMarketingOrderItemList.Add(new CustomerMarketingOrderItemModel()
                        {
                            Id = item.Id,
                            StockId = item.StockId,
                            ItemName = item.ItemName,
                            Packing = item.Packing,
                            Price = item.Price,
                            //PreviousQuantity = item.PreviousQuantity,
                            NewQuantity = item.Quantity,
                            NewDiscount = item.Discount,
                            TotalPrice = ((item.Price * item.Packing) * item.Quantity) - item.Discount
                        });
                    }
                    CustomerModel = CustomerList.FirstOrDefault(x => x.Id == result.CustomerId);
                    PaymentType = PaymentList.FirstOrDefault(x => x.Id == result.PaymentId);
                }

            }

            else if (str == "New Order")
            {
                Reset();
                ApplicationManager.Instance.HideDialog();
            }
            else if (str == "Clear")
            {
                Init();
            }
        }

        private void Reset()
        {
            CustomerMarketingOrderItemModel = new CustomerMarketingOrderItemModel();
            CustomerMarketingOrderItemList = new ObservableCollection<CustomerMarketingOrderItemModel>();
            CustomerMarketingOrderList = new ObservableCollection<CustomerMarketingOrderModel>();
            CustomerMarketingOrderModel = new CustomerMarketingOrderModel();
            CustomerOrderModel = new CustomerOrderModel();
            GetPaymentType();
            GetCustomer();
            GetItems();
            _isPreviousOrder = false;
            OrderNumber = "Search Order";
            Quantity = 0;
            Discount = 0;
            NewItemPrice = 0;
            GetOrderNumber();
        }

        private void Init()
        {
            var bw = new BackgroundWorker();
            bw.DoWork += (sender, args) =>
            {
                try
                {
                    ApplicationManager.Instance.ShowBusyInidicator("Loading Data... !");
                    _stockRepository = new GenericRepository<Entities.DBModel.Stocks.Stock>(new HAFoodDbContext());
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

        private void GetPaymentType()
        {

            PaymentType = PaymentList.FirstOrDefault();
        }

        private void GetOrderNumber()
        {
            var result = _customerMarketingBillRepository.Get().LastOrDefault();
            if (result != null)
            {
                int orderNo = Convert.ToInt32(result.OrderNo) + 1;
                CustomerMarketingOrderModel.OrderNo = orderNo.ToString();
            }
            else
                CustomerMarketingOrderModel.OrderNo = "1000";
        }

        private void GetCustomer()
        {
            CustomerModel = CustomerList.FirstOrDefault();
        }

        private void GetItems()
        {
            StockModel = StockList.FirstOrDefault();
        }

        private void LoadCollections()
        {
            CustomerList = MapperProfile.iMapper.Map<ObservableCollection<CustomerModel>>(_customerRepository.Get());
            StockList = MapperProfile.iMapper.Map<ObservableCollection<StockModel>>(_stockRepository.Get());
            PaymentList = MapperProfile.iMapper.Map<ObservableCollection<PaymentModel>>(_paymentRepository.Get());
        }

        private void CalculateStock(ObservableCollection<CustomerMarketingOrderItemModel> customerMarketingOrderItemModel)
        {
            _stockRepository = new GenericRepository<Entities.DBModel.Stocks.Stock>(new HAFoodDbContext());
            foreach (var item in customerMarketingOrderItemModel)
            {
                Entities.DBModel.Stocks.Stock stock = _stockRepository.GetById(item.StockId);
                if (stock != null)
                {
                    //if (_isPreviousOrder)
                    //    stock.NewQuantity = (item.PreviousQuantity + stock.NewQuantity) - item.NewQuantity;
                    //else
                    stock.Quantity = stock.Quantity - item.Quantity;
                    _stockRepository.Update(stock, stock.Id);
                }
            }
        }

        private void GetCustomerMarketingAmount(int customerId)
        {
            CustomerOrderModel = new CustomerOrderModel();
            List<CustomerMarketingOrderAmount> customerMarketingOrderAmount = _customerMarketingOrderAmountRepository.Get().Where(x => x.CustomerId == customerId).ToList();
            if (customerMarketingOrderAmount != null && customerMarketingOrderAmount.Count > 0)
            {
                CustomerOrderModel.CustomerId = customerId;
                CustomerOrderModel.AmountPaid = customerMarketingOrderAmount.Sum(x => x.AmountPaid);
                CustomerOrderModel.RemainingAmount = customerMarketingOrderAmount.Sum(x => x.RemainingAmount);
                CustomerOrderModel.TotalAmount = customerMarketingOrderAmount.Sum(x => x.TotalAmount);
                CustomerCashBookAmount = _cashBookOneRepository.Get().Where(x => x.CustomerId == customerId).Sum(x => x.Amount);
            }
        }

        private void SaveCustomerMarketingAmount()
        {
            CustomerMarketingOrderAmount customerMarketingOrderAmount = _customerMarketingOrderAmountRepository.Get().FirstOrDefault(x => x.OrderNo == CustomerMarketingOrderModel.OrderNo);
            if (customerMarketingOrderAmount == null)
            {
                CustomerMarketingOrderAmount custMarketingOrderAmount = new CustomerMarketingOrderAmount
                {
                    CustomerId = CustomerModel.Id,
                    OrderNo = CustomerMarketingOrderModel.OrderNo,
                    AmountPaid = CustomerMarketingOrderModel.AmountPaid,
                    RemainingAmount = CustomerMarketingOrderModel.TotalPrice - CustomerMarketingOrderModel.AmountPaid,
                    TotalAmount = CustomerMarketingOrderModel.TotalPrice,
                    Balance = CustomerMarketingOrderModel.GrandTotal - CustomerMarketingOrderModel.AmountPaid,
                    CreatedDate = DateTime.Now
                };

                GrandTotal = custMarketingOrderAmount.TotalAmount;
                AmountPaid = custMarketingOrderAmount.AmountPaid;
                RemainingAmount = custMarketingOrderAmount.RemainingAmount;

                _customerMarketingOrderAmountRepository.Add(custMarketingOrderAmount);
            }
            else
            {
                customerMarketingOrderAmount.CustomerId = CustomerModel.Id;
                customerMarketingOrderAmount.OrderNo = CustomerMarketingOrderModel.OrderNo;
                customerMarketingOrderAmount.AmountPaid = CustomerMarketingOrderModel.AmountPaid + customerMarketingOrderAmount.AmountPaid;
                customerMarketingOrderAmount.RemainingAmount = CustomerMarketingOrderModel.TotalPrice - CustomerMarketingOrderModel.AmountPaid;
                customerMarketingOrderAmount.TotalAmount = NewItemPrice + customerMarketingOrderAmount.TotalAmount;
                customerMarketingOrderAmount.Balance = CustomerMarketingOrderModel.GrandTotal - CustomerMarketingOrderModel.AmountPaid;
                customerMarketingOrderAmount.UpdatedDate = DateTime.Now;

                GrandTotal = customerMarketingOrderAmount.TotalAmount;
                AmountPaid = customerMarketingOrderAmount.AmountPaid;
                RemainingAmount = customerMarketingOrderAmount.Balance;

                _customerMarketingOrderAmountRepository.Update(customerMarketingOrderAmount, customerMarketingOrderAmount.Id);
            }
        }

        private void LoadReport(CustomerMarketingOrder customerMarketingOrder)
        {
            DataTable dt = new DataTable();
            string constr = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\HAFood\HAFoodDB.mdf; Integrated Security = True;";
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("[dbo].[SP_GetCustomerMarketingBill]", con))
                {
                    cmd.Parameters.AddWithValue("@CustomerMarketingBillId", customerMarketingOrder.Id);
                    cmd.Parameters.AddWithValue("@CustomerId", customerMarketingOrder.CustomerId);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                }
            }
            if (dt.Rows.Count > 0)
            {
                ApplicationManager.Instance.PrintReport(dt, @"/Reports/rptCustomerMarketingBill", "dsCurrentTransaction", "CustomerMarketingBill");
            }
        }

        #endregion
    }
}
