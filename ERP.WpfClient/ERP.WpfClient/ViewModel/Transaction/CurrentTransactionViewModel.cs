using ERP.Common;
using ERP.Common.NotifyProperty;
using ERP.Repository.Generic;
using ERP.WpfClient.Controls.Helpers;
using ERP.WpfClient.Mapper;
using ERP.WpfClient.Model;
using ERP.WpfClient.Model.Payment;
using ERP.WpfClient.Model.Stock;
using ERP.WpfClient.Model.Transaction;
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

        private readonly IGenericRepository<Entities.DBModel.CurrentTransaction> _currentTransactionRepository;
        private readonly IGenericRepository<Entities.DBModel.Customer> _customerRepository;
        private readonly IGenericRepository<Entities.DBModel.Stock> _stockRepository;
        private CurrentTransactionModel _currentTransactionModel;
        private ObservableCollection<CurrentTransactionModel> _currentTransactionList;
        private string _currentTransactionButton;
        private string _currentTransactionParameter;
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
        private decimal _grandTotal;

        #endregion

        #region Ctor

        public CurrentTransactionViewModel()
        {
            CurrentTransactionCommands = new RelayCommand<object>(ExecuteCurrentTransactionCommand);
            DeleteCurrentTransactionCommand = new RelayCommand<object>(ExecuteDeleteCurrentTransactionCommand);
            CurrentOrderCommand = new RelayCommand<string>(ExecuteCurrentOrderCommand);
            //this.CurrentTransactionCommands = new CustomerCommand(this);
            _currentTransactionRepository = App.Resolve<IGenericRepository<Entities.DBModel.CurrentTransaction>>();
            _customerRepository = App.Resolve<IGenericRepository<Entities.DBModel.Customer>>();
            _stockRepository = App.Resolve<IGenericRepository<Entities.DBModel.Stock>>();
            CurrentTransactionModel = new CurrentTransactionModel();
            CurrentTransactionDetailModel = new CurrentTransactionDetailModel();
            CurrentTransactionDetailList = new ObservableCollection<CurrentTransactionDetailModel>();
            CurrentTransactionList = new ObservableCollection<CurrentTransactionModel>();
            CurrentTransactionButton = "Save";
            CurrentTransactionParameter = "SaveCurrentTransaction";
            CustomerModel = new CustomerModel();
            CustomerList = new ObservableCollection<CustomerModel>();
            StockModel = new StockModel();
            StockList = new ObservableCollection<StockModel>();
            PaymentList = new List<PaymentModel>();
        }

        #endregion

        #region Properties
        public RelayCommand<object> CurrentTransactionCommands { get; set; }
        public RelayCommand<object> DeleteCurrentTransactionCommand { get; set; }
        public RelayCommand<string> CurrentOrderCommand { get; set; }
        //public CustomerCommand CurrentTransactionCommands { get; set; }

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


        public string CurrentTransactionParameter
        {
            get { return _currentTransactionParameter; }
            set { _currentTransactionParameter = value; RaisePropertyChanged("CurrentTransactionParameter"); }
        }

        public string CurrentTransactionButton
        {
            get { return _currentTransactionButton; }
            set { _currentTransactionButton = value; RaisePropertyChanged("CurrentTransactionButton"); }
        }

        #endregion

        #region Methods

        private void ExecuteCurrentOrderCommand(string str)
        {
            CurrentTransactionDetailModel = CurrentTransactionDetailList.Where(x => x.ItemName == StockModel.ItemName).FirstOrDefault();

            if (CurrentTransactionDetailModel != null)
            {
                CurrentTransactionDetailModel.ItemName = StockModel.ItemName;
                CurrentTransactionDetailModel.Price = StockModel.SalePrice;
                CurrentTransactionDetailModel.Quantity = Quantity;
                CurrentTransactionDetailModel.Discount = Discount;
                decimal totalPrice = CurrentTransactionDetailModel.Price * Quantity;
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

            TotalPrice = CurrentTransactionDetailList.Sum(x => x.TotalPrice);
            TotalDiscount = CurrentTransactionDetailList.Sum(x => x.Discount);
            GrandTotal = TotalPrice - TotalDiscount;
            
        }

        private void ExecuteCurrentTransactionCommand(object str)
        {
            if (str as string == "SaveCurrentTransaction")
            {
                SaveCurrentTransaction();
            }
            else if (str as string == "UpdateCurrentTransaction")
            {
                UpdateCurrentTransaction();
            }
            else if (str as string == "Clear")
            {
                Reset();
            }
            else if (str != null)
            {
                EditStock(str as CurrentTransactionModel);
            }
        }

        private void Reset()
        {
            CurrentTransactionModel = new CurrentTransactionModel();
            CurrentTransactionButton = "Save";
            CurrentTransactionParameter = "SaveCurrentTransaction";
        }

        private void ExecuteDeleteCurrentTransactionCommand(object obj)
        {
            if (obj != null)
            {
                ApplicationManager.Instance.ShowConfirmDialog("Are you sure you want to Delete the delete?", () =>
                {
                    DeleteStock(obj as CurrentTransactionModel);
                    ApplicationManager.Instance.HideDialog();
                }, () => ApplicationManager.Instance.HideMessageBox(), useYesNo: true);
            }
        }

        public void SaveCurrentTransaction()
        {
            var model = _currentTransactionRepository.Add(MapperProfile.iMapper.Map<Entities.DBModel.CurrentTransaction>(CurrentTransactionModel));
            CurrentTransactionModel.Id = model.Id;
            CurrentTransactionList.Add(CurrentTransactionModel);
            Reset();
        }

        public void EditStock(CurrentTransactionModel currentTransactionModel)
        {
            CurrentTransactionButton = "Update";
            CurrentTransactionParameter = "UpdateCurrentTransaction";
            CurrentTransactionModel.Id = currentTransactionModel.Id;
            //CurrentTransactionModel.ItemName = currentTransactionModel.ItemName;
            //CurrentTransactionModel.UrduName = currentTransactionModel.UrduName;
            //CurrentTransactionModel.BuyPrice = currentTransactionModel.BuyPrice;
            //CurrentTransactionModel.SalePrice = currentTransactionModel.SalePrice;
            //CurrentTransactionModel.Quantity = currentTransactionModel.Quantity;
            //CurrentTransactionModel.Category = currentTransactionModel.Category;
            //CurrentTransactionModel.Packing = currentTransactionModel.Packing;
            //CurrentTransactionModel.Remarks = currentTransactionModel.Remarks;

        }

        public void UpdateCurrentTransaction()
        {
            _currentTransactionRepository.Update(MapperProfile.iMapper.Map<Entities.DBModel.CurrentTransaction>(CurrentTransactionModel), CurrentTransactionModel.Id);
            Reset();
        }

        public void DeleteStock(CurrentTransactionModel currentTransactionModel)
        {
            _currentTransactionRepository.Delete(currentTransactionModel.Id);
            CurrentTransactionList.Remove(currentTransactionModel);
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
                OrderNumber = "#" + result.OrderNo;
            else
                OrderNumber = "#0001";
        }

        public void OnBringIntoView()
        {
            Init();
        }

        #endregion
    }
}
