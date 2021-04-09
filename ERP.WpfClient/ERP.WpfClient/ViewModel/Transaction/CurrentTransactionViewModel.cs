using ERP.Common;
using ERP.Common.NotifyProperty;
using ERP.Entities.DBModel;
using ERP.Repository.Generic;
using ERP.Repository.Transaction;
using ERP.WpfClient.Controls.Helpers;
using ERP.WpfClient.Mapper;
using ERP.WpfClient.Model;
using ERP.WpfClient.Model.Payment;
using ERP.WpfClient.Model.Stock;
using ERP.WpfClient.Model.Transaction;
using ERP.WpfClient.View.Popups.Payments;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ERP.WpfClient.ViewModel.Transaction
{
    public class CurrentTransactionViewModel : ViewModelBase, INotifyOnBringIntoView
    {
        #region Fields

        private readonly IGenericRepository<CurrentTransaction> _currentTransactionRepository;
        private readonly IGenericRepository<CurrentTransactionDetail> _currentTransactionDetailRepository;
        private readonly IGenericRepository<Entities.DBModel.Customer> _customerRepository;
        private readonly IGenericRepository<Entities.DBModel.Stock> _stockRepository;
        private CurrentTransactionModel _currentTransactionModel;
        private ObservableCollection<CurrentTransactionModel> _currentTransactionList;
        private CurrentTransactionDetailModel _currentTransactionDetailModel;
        private ObservableCollection<CurrentTransactionDetailModel> _currentTransactionDetailList;
        private CustomerModel _customerModel;
        private ObservableCollection<CustomerModel> _customerList;
        private StockModel _stockModel;
        private ObservableCollection<StockModel> _stockList;
        private List<PaymentModel> _paymentList;
        private PaymentModel _paymentType;
        private string _orderNumber;
        private int _quantity;
        private decimal _discount;
        private decimal _totalPrice;
        private decimal _totalDiscount;
        private decimal? _grandTotal;
        private bool _isPreviousOrder = false;
        private int _currentTransactionId;
        #endregion

        #region Ctor

        public CurrentTransactionViewModel()
        {
            CurrentOrderCommand = new RelayCommand<string>(ExecuteCurrentOrderCommand);
            //this.CurrentTransactionCommands = new CustomerCommand(this);
            _currentTransactionRepository = App.Resolve<IGenericRepository<CurrentTransaction>>();
            _currentTransactionDetailRepository = App.Resolve<IGenericRepository<CurrentTransactionDetail>>();
            _customerRepository = App.Resolve<IGenericRepository<Entities.DBModel.Customer>>();
            _stockRepository = App.Resolve<IGenericRepository<Entities.DBModel.Stock>>();
            StockList = new ObservableCollection<StockModel>();
            CustomerList = new ObservableCollection<CustomerModel>();
            PaymentList = new List<PaymentModel>();
        }

        #endregion

        #region Properties
        public RelayCommand<string> CurrentOrderCommand { get; set; }
        //public CustomerCommand CurrentTransactionCommands { get; set; }

        public decimal? GrandTotal
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

        public PaymentModel PaymentType
        {
            get { return _paymentType; }
            set { _paymentType = value; RaisePropertyChanged("PaymentType"); }
        }

        public List<PaymentModel> PaymentList
        {
            get { return _paymentList; }
            set { _paymentList = value; RaisePropertyChanged("PaymentList"); }
        }

        #endregion

        #region Methods

        private void ExecuteCurrentOrderCommand(string str)
        {
            CurrentTransactionDetailRepository currentTransaction = new CurrentTransactionDetailRepository();

            if (str == "Add Item")
            {
                CurrentTransactionDetailModel = CurrentTransactionDetailList.Where(x => x.ItemName == StockModel.ItemName).FirstOrDefault();

                if (CurrentTransactionDetailModel != null)
                {
                    CurrentTransactionDetailModel.ItemName = StockModel.ItemName;
                    CurrentTransactionDetailModel.Price = StockModel.SalePrice;
                    CurrentTransactionDetailModel.Quantity = Quantity;
                    CurrentTransactionDetailModel.Discount = Discount;
                    decimal? totalPrice = CurrentTransactionDetailModel.Price * Quantity;
                    CurrentTransactionDetailModel.TotalPrice = totalPrice - Discount;
                }
                else
                {
                    CurrentTransactionDetailList.Add(new CurrentTransactionDetailModel()
                    {
                        ItemName = StockModel.ItemName,
                        Price = StockModel.SalePrice,
                        Quantity = Quantity,
                        Discount = Discount,
                        TotalPrice = (StockModel.SalePrice * Quantity) - Discount
                    });
                }

                decimal? price = CurrentTransactionDetailList.Sum(x => x.Price * x.Quantity);

                CurrentTransactionModel.TotalPrice = price;
                CurrentTransactionModel.TotalDiscount = CurrentTransactionDetailList.Sum(x => x.Discount);
                CurrentTransactionModel.GrandTotal = CurrentTransactionModel.TotalPrice - CurrentTransactionModel.TotalDiscount;
                GrandTotal = CurrentTransactionModel.GrandTotal;
            }
            else if (str == "Save Order")
            {
                CurrentTransaction transaction = new CurrentTransaction();

                if (_isPreviousOrder)
                    transaction.Id = _currentTransactionId;
                transaction.OrderNo = CurrentTransactionModel.OrderNo;
                transaction.CustomerId = CustomerModel.Id;
                transaction.TotalPrice = CurrentTransactionModel.TotalPrice;
                transaction.TotalDiscount = CurrentTransactionModel.TotalDiscount;
                transaction.GrandTotal = CurrentTransactionModel.GrandTotal;
                transaction.CreatedDate = DateTime.Now;

                foreach (var item in CurrentTransactionDetailList)
                {
                    CurrentTransactionDetail currentTransactionDetail = new CurrentTransactionDetail();
                    if (_isPreviousOrder)
                        currentTransactionDetail.Id = item.Id;
                    currentTransactionDetail.ItemName = item.ItemName;
                    currentTransactionDetail.Quantity = item.Quantity;
                    currentTransactionDetail.Price = item.Price;
                    currentTransactionDetail.Discount = item.Discount;
                    currentTransactionDetail.TotalPrice = item.TotalPrice;
                    transaction.CurrentTransactionDetails.Add(currentTransactionDetail);
                }

                Payment payment = new Payment();
                if (_isPreviousOrder)
                    payment.Id = PaymentType.Id;
                payment.PaymentType = PaymentType.PaymentType;
                transaction.Payments.Add(payment);

                currentTransaction.SaveDetail(transaction);

                ApplicationManager.Instance.ShowDialog("Order Saved!", new PaymentPopup(this));

                Reset();
            }

            else if (str == "Search Order")
            {
                CurrentTransactionDetailList = new ObservableCollection<CurrentTransactionDetailModel>();
                var result = currentTransaction.GetOrder(OrderNumber);
                if (result != null)
                {
                    _isPreviousOrder = true;
                    _currentTransactionId = result.Id;

                    CurrentTransactionModel.OrderNo = result.OrderNo;
                    CurrentTransactionModel.TotalPrice = result.TotalPrice;
                    CurrentTransactionModel.TotalDiscount = result.TotalPrice;
                    CurrentTransactionModel.GrandTotal = result.TotalPrice;

                    foreach (var item in result.CurrentTransactionDetails)
                    {
                        CurrentTransactionDetailList.Add(new CurrentTransactionDetailModel()
                        {
                            Id = item.Id,
                            ItemName = item.ItemName,
                            Price = item.Price,
                            Quantity = item.Quantity,
                            Discount = item.Discount,
                            TotalPrice = (item.Price * item.Quantity) - item.Discount
                        });
                    }
                }

                CustomerList = MapperProfile.iMapper.Map<ObservableCollection<CustomerModel>>(_customerRepository.Get());
                CustomerModel = CustomerList.FirstOrDefault();

                PaymentList = MapperProfile.iMapper.Map<List<PaymentModel>>(result.Payments.Where(x => x.CurrentTransactionId == result.Id).ToList());
                PaymentType = PaymentList.FirstOrDefault();
            }

            else if (str == "New Order")
            {
                Reset();
                ApplicationManager.Instance.HideDialog();
            }
        }

        private void Reset()
        {
            CurrentTransactionDetailModel = new CurrentTransactionDetailModel();
            CurrentTransactionDetailList = new ObservableCollection<CurrentTransactionDetailModel>();
            CurrentTransactionList = new ObservableCollection<CurrentTransactionModel>();
            //StockList = new ObservableCollection<StockModel>();
            //CustomerList = new ObservableCollection<CustomerModel>();
            //PaymentList = new List<PaymentModel>();
            CurrentTransactionModel = new CurrentTransactionModel();
            CustomerModel = new CustomerModel();
            StockModel = new StockModel();
            _isPreviousOrder = false;
            OrderNumber = "Search Order";
            Quantity = 0;
            Discount = 0;
            GetOrderNumber();
        }

        private void Init()
        {
            var bw = new BackgroundWorker();
            List<Entities.DBModel.CurrentTransaction> currentTransaction = null;
            bw.DoWork += (sender, args) =>
            {
                try
                {
                    ApplicationManager.Instance.ShowBusyInidicator("Loading Data... !");
                    currentTransaction = _currentTransactionRepository.Get();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Post Error\nMessage: " + ex.Message, "HA Foods");
                }
            };

            bw.RunWorkerCompleted += async (sender, args) =>
            {
                await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    CurrentTransactionList = MapperProfile.iMapper.Map<ObservableCollection<CurrentTransactionModel>>(currentTransaction);
                }));
                ApplicationManager.Instance.HideBusyInidicator();
            };

            bw.RunWorkerAsync();

            CustomerList = MapperProfile.iMapper.Map<ObservableCollection<CustomerModel>>(_customerRepository.Get());
            StockList = MapperProfile.iMapper.Map<ObservableCollection<StockModel>>(_stockRepository.Get());
            GetPaymentType();
            GetOrderNumber();
        }

        private void GetPaymentType()
        {
            PaymentList.Add(new PaymentModel { PaymentType = "Cash" });
            PaymentList.Add(new PaymentModel { PaymentType = "Credit" });
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

        public void OnBringIntoView()
        {
            Reset();
            Init();
        }

        #endregion
    }
}
