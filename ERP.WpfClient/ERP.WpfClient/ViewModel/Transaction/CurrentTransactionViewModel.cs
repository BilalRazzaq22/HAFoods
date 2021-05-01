using ERP.Common;
using ERP.Common.NotifyProperty;
using ERP.Entities.DbContext;
using ERP.Entities.DBModel;
using ERP.Entities.DBModel.Customers;
using ERP.Entities.DBModel.Payments;
using ERP.Entities.DBModel.Transactions;
using ERP.Repository.Customer;
using ERP.Repository.Generic;
using ERP.Repository.Transaction;
using ERP.WpfClient.Controls.Helpers;
using ERP.WpfClient.Mapper;
using ERP.WpfClient.Model;
using ERP.WpfClient.Model.Customer;
using ERP.WpfClient.Model.Payment;
using ERP.WpfClient.Model.Stock;
using ERP.WpfClient.Model.Transaction;
using ERP.WpfClient.View.Popups.Payments;
using GalaSoft.MvvmLight.Command;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace ERP.WpfClient.ViewModel.Transaction
{
    public class CurrentTransactionViewModel : ViewModelBase, INotifyOnBringIntoView
    {
        #region Fields

        private readonly CurrentTransactionRepository _currentTransactionRepository;
        private readonly IGenericRepository<CurrentTransactionDetail> _currentTransactionDetailRepository;
        private readonly IGenericRepository<Entities.DBModel.Customers.Customer> _customerRepository;
        private IGenericRepository<Entities.DBModel.Stocks.Stock> _stockRepository;
        private readonly IGenericRepository<Payment> _paymentRepository;
        private readonly CustomerOrderRepository _customerOrderRepository;
        private CurrentTransactionModel _currentTransactionModel;
        private ObservableCollection<CurrentTransactionModel> _currentTransactionList;
        private CurrentTransactionDetailModel _currentTransactionDetailModel;
        private ObservableCollection<CurrentTransactionDetailModel> _currentTransactionDetailList;
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
        private int _currentTransactionId;
        private CustomerOrderModel _customerOrderModel;
        private decimal _amountPaid;
        private decimal _remainingAmount;
        private decimal _newItemPrice;

        //CurrentTransactionRepository currentTransaction = new CurrentTransactionRepository();

        #endregion

        #region Ctor

        public CurrentTransactionViewModel()
        {
            CurrentOrderCommand = new RelayCommand<string>(ExecuteCurrentOrderCommand);
            _currentTransactionRepository = new CurrentTransactionRepository(new HAFoodDbContext());
            _currentTransactionDetailRepository = App.Resolve<IGenericRepository<CurrentTransactionDetail>>();
            _customerRepository = App.Resolve<IGenericRepository<Entities.DBModel.Customers.Customer>>();
            _stockRepository = new GenericRepository<Entities.DBModel.Stocks.Stock>(new HAFoodDbContext());
            _paymentRepository = App.Resolve<IGenericRepository<Payment>>();
            _customerOrderRepository = new CustomerOrderRepository(new HAFoodDbContext());
            StockList = new ObservableCollection<StockModel>();
            CustomerList = new ObservableCollection<CustomerModel>();
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

        public CurrentTransactionModel CurrentTransactionModel
        {
            get { return _currentTransactionModel; }
            set { _currentTransactionModel = value; RaisePropertyChanged("CurrentTransactionModel"); }
        }

        public CurrentTransactionDetailModel CurrentTransactionDetailModel
        {
            get { return _currentTransactionDetailModel; }
            set { _currentTransactionDetailModel = value; RaisePropertyChanged("CurrentTransactionDetailModel"); }
        }

        public ObservableCollection<CurrentTransactionDetailModel> CurrentTransactionDetailList
        {
            get { return _currentTransactionDetailList; }
            set { _currentTransactionDetailList = value; RaisePropertyChanged("CurrentTransactionDetailList"); }
        }

        public ObservableCollection<CurrentTransactionModel> CurrentTransactionList
        {
            get { return _currentTransactionList; }
            set { _currentTransactionList = value; RaisePropertyChanged("CurrentTransactionList"); }
        }

        public CustomerModel CustomerModel
        {
            get { return _customerModel; }
            set
            {
                _customerModel = value;
                if (_customerModel != null)
                    GetCustomerAmount(_customerModel.Id);
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

                CurrentTransactionDetailModel = CurrentTransactionDetailList.Where(x => x.ItemName == StockModel.ItemName).FirstOrDefault();

                if (CurrentTransactionDetailModel != null)
                {
                    CurrentTransactionDetailModel.ItemName = StockModel.ItemName;
                    CurrentTransactionDetailModel.Price = StockModel.SalePrice;
                    //if (_isPreviousOrder)
                    //{
                    CurrentTransactionDetailModel.NewQuantity = CurrentTransactionDetailModel.NewQuantity + Quantity;
                    //}
                    CurrentTransactionDetailModel.Quantity = Quantity;
                    CurrentTransactionDetailModel.NewDiscount = CurrentTransactionDetailModel.NewDiscount + Discount;
                    CurrentTransactionDetailModel.Discount = Discount;
                    decimal totalPrice = CurrentTransactionDetailModel.Price * CurrentTransactionDetailModel.NewQuantity;
                    CurrentTransactionDetailModel.TotalPrice = totalPrice - CurrentTransactionDetailModel.NewDiscount;
                }
                else
                {
                    CurrentTransactionDetailList.Add(new CurrentTransactionDetailModel()
                    {
                        StockId = StockModel.Id,
                        ItemName = StockModel.ItemName,
                        Price = StockModel.SalePrice,
                        Quantity = Quantity,
                        NewQuantity = Quantity,
                        Discount = Discount,
                        NewDiscount = Discount,
                        TotalPrice = (StockModel.SalePrice * Quantity) - Discount
                    });
                }

                decimal price = CurrentTransactionDetailList.Sum(x => x.Price * x.NewQuantity);

                CurrentTransactionModel.TotalPrice = price;
                CurrentTransactionModel.TotalDiscount = CurrentTransactionDetailList.Sum(x => x.NewDiscount);
                if (_isPreviousOrder)
                {
                    CurrentTransactionModel.GrandTotal = ((((CurrentTransactionDetailModel != null) ? CurrentTransactionDetailModel.Price : StockModel.SalePrice) * Quantity) + CustomerOrderModel.RemainingAmount) - Discount;
                    NewItemPrice += ((CurrentTransactionDetailModel != null) ? CurrentTransactionDetailModel.Price : StockModel.SalePrice) * Quantity;
                }
                else
                    CurrentTransactionModel.GrandTotal = (CurrentTransactionModel.TotalPrice + CustomerOrderModel.RemainingAmount) - CurrentTransactionModel.TotalDiscount;
            }
            else if (str == "Save Order")
            {
                var bw = new BackgroundWorker();
                bw.DoWork += (sender, args) =>
                {
                    try
                    {
                        ApplicationManager.Instance.ShowBusyInidicator("Saving Order ... !");

                        CurrentTransaction transaction = new CurrentTransaction();

                        if (_isPreviousOrder)
                        {
                            transaction.Id = _currentTransactionId;
                            transaction.UpdatedDate = DateTime.Now;
                        }
                        else
                        {
                            transaction.CreatedDate = DateTime.Now;
                        }
                        transaction.OrderNo = CurrentTransactionModel.OrderNo;
                        transaction.CustomerId = CustomerModel.Id;
                        transaction.PaymentId = PaymentType.Id;
                        transaction.TotalPrice = CurrentTransactionModel.TotalPrice;
                        transaction.TotalDiscount = CurrentTransactionModel.TotalDiscount;
                        transaction.GrandTotal = CurrentTransactionModel.GrandTotal;
                        transaction.AmountPaid = CurrentTransactionModel.AmountPaid;
                        transaction.CreatedDate = DateTime.Now;

                        foreach (var item in CurrentTransactionDetailList)
                        {
                            CurrentTransactionDetail currentTransactionDetail = new CurrentTransactionDetail();
                            if (_isPreviousOrder)
                            {
                                currentTransactionDetail.Id = item.Id;
                                currentTransactionDetail.UpdatedDate = DateTime.Now;
                            }
                            else
                            {
                                currentTransactionDetail.CreatedDate = DateTime.Now;
                            }
                            currentTransactionDetail.StockId = item.StockId;
                            currentTransactionDetail.ItemName = item.ItemName;
                            //if (!_isPreviousOrder)
                            //    currentTransactionDetail.PreviousQuantity = item.PreviousQuantity;
                            currentTransactionDetail.Quantity = item.NewQuantity;
                            currentTransactionDetail.Price = item.Price;
                            currentTransactionDetail.Discount = item.Discount;
                            currentTransactionDetail.TotalPrice = item.TotalPrice;
                            transaction.CurrentTransactionDetails.Add(currentTransactionDetail);
                        }

                        SaveCustomerAmount();

                        CurrentTransaction t = _currentTransactionRepository.SaveCustomerOrder(transaction);

                        CalculateStock(CurrentTransactionDetailList);
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
                CurrentTransactionDetailList = new ObservableCollection<CurrentTransactionDetailModel>();
                var result = _currentTransactionRepository.GetOrder(OrderNumber);
                if (result != null)
                {
                    _isPreviousOrder = true;

                    GetCustomerAmount(Convert.ToInt32(result.CustomerId));

                    _currentTransactionId = result.Id;
                    CurrentTransactionModel.OrderNo = result.OrderNo;
                    CurrentTransactionModel.TotalPrice = result.TotalPrice;
                    CurrentTransactionModel.TotalDiscount = result.TotalDiscount;
                    CurrentTransactionModel.GrandTotal = CustomerOrderModel.RemainingAmount;//result.GrandTotal;

                    foreach (var item in result.CurrentTransactionDetails)
                    {
                        CurrentTransactionDetailList.Add(new CurrentTransactionDetailModel()
                        {
                            Id = item.Id,
                            StockId = item.StockId,
                            ItemName = item.ItemName,
                            Price = item.Price,
                            //PreviousQuantity = item.PreviousQuantity,
                            NewQuantity = item.Quantity,
                            NewDiscount = item.Discount,
                            TotalPrice = (item.Price * item.Quantity) - item.Discount
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
            CurrentTransactionDetailModel = new CurrentTransactionDetailModel();
            CurrentTransactionDetailList = new ObservableCollection<CurrentTransactionDetailModel>();
            CurrentTransactionList = new ObservableCollection<CurrentTransactionModel>();
            CurrentTransactionModel = new CurrentTransactionModel();
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
            var result = _currentTransactionRepository.Get().LastOrDefault();
            if (result != null)
            {
                int orderNo = Convert.ToInt32(result.OrderNo) + 1;
                CurrentTransactionModel.OrderNo = orderNo.ToString();
            }
            else
                CurrentTransactionModel.OrderNo = "1000";
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

        private void CalculateStock(ObservableCollection<CurrentTransactionDetailModel> currentTransactionDetailModel)
        {
            _stockRepository = new GenericRepository<Entities.DBModel.Stocks.Stock>(new HAFoodDbContext());
            foreach (var item in currentTransactionDetailModel)
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

        private void GetCustomerAmount(int customerId)
        {
            CustomerOrderModel = new CustomerOrderModel();
            CustomerOrder customerOrder = _customerOrderRepository.Get().FirstOrDefault(x => x.CustomerId == customerId);
            if (customerOrder != null)
            {
                CustomerOrderModel.CustomerId = customerOrder.CustomerId;
                CustomerOrderModel.AmountPaid = customerOrder.AmountPaid;
                CustomerOrderModel.RemainingAmount = customerOrder.RemainingAmount;
                CustomerOrderModel.TotalAmount = customerOrder.TotalAmount;
            }
        }

        private void SaveCustomerAmount()
        {
            CustomerOrder customerOrder = _customerOrderRepository.Get().FirstOrDefault(x => x.CustomerId == CustomerModel.Id);
            if (customerOrder == null)
            {
                CustomerOrder custOrder = new CustomerOrder
                {
                    CustomerId = CustomerModel.Id,
                    AmountPaid = CurrentTransactionModel.AmountPaid,
                    RemainingAmount = CurrentTransactionModel.GrandTotal - CurrentTransactionModel.AmountPaid,
                    TotalAmount = CurrentTransactionModel.TotalPrice
                };

                GrandTotal = custOrder.TotalAmount;
                AmountPaid = custOrder.AmountPaid;
                RemainingAmount = custOrder.RemainingAmount;

                _customerOrderRepository.SaveCustomerOrderAmount(custOrder);
            }
            else
            {
                customerOrder.CustomerId = CustomerModel.Id;
                customerOrder.AmountPaid = CurrentTransactionModel.AmountPaid + customerOrder.AmountPaid;
                customerOrder.RemainingAmount = CurrentTransactionModel.GrandTotal - CurrentTransactionModel.AmountPaid;
                if (_isPreviousOrder)
                    customerOrder.TotalAmount = NewItemPrice + customerOrder.TotalAmount;
                else
                    customerOrder.TotalAmount = (CurrentTransactionModel.TotalPrice - CurrentTransactionModel.TotalDiscount) + customerOrder.TotalAmount;

                GrandTotal = customerOrder.TotalAmount;
                AmountPaid = customerOrder.AmountPaid;
                RemainingAmount = customerOrder.RemainingAmount;

                _customerOrderRepository.SaveCustomerOrderAmount(customerOrder);
            }
        }

        private void LoadReport(CurrentTransaction t)
        {
            var query = (from c in _currentTransactionRepository.Get().Where(x => x.Id == t.Id)
                         join cd in _currentTransactionDetailRepository.Get() on c.Id equals cd.CurrentTransactionId
                         join p in _paymentRepository.Get() on c.PaymentId equals p.Id
                         join co in _customerOrderRepository.Get() on c.CustomerId equals co.CustomerId
                         select new
                         {
                             ItemName = cd.ItemName,
                             Quantity = cd.Quantity,
                             Price = cd.Price,
                             Discount = cd.Discount,
                             TotalPrice = cd.TotalPrice,
                             OrderNo = c.OrderNo,
                             GrandTotalPrice = c.TotalPrice,
                             GrandTotalDiscount = c.TotalDiscount,
                             GrandTotal = c.GrandTotal,
                             TotalAmount = co.TotalAmount,
                             AmountPaid = co.AmountPaid,
                             RemainingAmount = co.RemainingAmount,
                             PaymentType = p.PaymentType,
                             TotalCustomerOrders = _currentTransactionRepository.Get().Count(x => x.CustomerId == t.CustomerId),
                             CreatedDate = c.CreatedDate
                         }).ToList();
            if (query.Count > 0)
            {
                ApplicationManager.Instance.PrintReport(query, @"/Reports/rptCurrentTransaction", "dsCurrentTransaction", "CustomerBill");
            }
        }

        public void OnBringIntoView()
        {
            Init();
        }

        #endregion
    }
}
