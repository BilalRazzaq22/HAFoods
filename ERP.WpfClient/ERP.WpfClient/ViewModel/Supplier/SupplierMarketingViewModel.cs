using ERP.Common;
using ERP.Common.NotifyProperty;
using ERP.Entities.DbContext;
using ERP.Entities.DBModel.CashBook;
using ERP.Entities.DBModel.Customers;
using ERP.Entities.DBModel.Payments;
using ERP.Entities.DBModel.Suppliers;
using ERP.Repository.Customer;
using ERP.Repository.Generic;
using ERP.Repository.Supplier;
using ERP.WpfClient.Controls.Helpers;
using ERP.WpfClient.Mapper;
using ERP.WpfClient.Model;
using ERP.WpfClient.Model.Customer;
using ERP.WpfClient.Model.Payment;
using ERP.WpfClient.Model.Stock;
using ERP.WpfClient.Model.Supplier;
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

namespace ERP.WpfClient.ViewModel.Supplier
{
    public class SupplierMarketingViewModel : ViewModelBase, INotifyOnBringIntoView
    {
        #region Fields

        private readonly SupplierMarketingBillRepository _supplierMarketingOrderRepository;
        private readonly IGenericRepository<Entities.DBModel.Suppliers.Supplier> _supplierRepository;
        private IGenericRepository<Entities.DBModel.Stocks.Stock> _stockRepository;
        private readonly IGenericRepository<Payment> _paymentRepository;
        private readonly IGenericRepository<SupplierMarketingOrderAmount> _supplierMarketingOrderAmountRepository;
        private readonly IGenericRepository<CashBookOne> _cashBookOneRepository;
        private SupplierMarketingOrderModel _supplierMarketingOrderModel;
        private SupplierMarketingOrderItemModel _supplierMarketingOrderItemModel;
        private ObservableCollection<SupplierMarketingOrderModel> _supplierMarketingOrderList;
        private ObservableCollection<SupplierMarketingOrderItemModel> _supplierMarketingOrderItemList;
        private SupplierModel _supplierModel;
        private ObservableCollection<SupplierModel> _supplierList;
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
        private decimal _amountPaid;
        private decimal _remainingAmount;
        private decimal _newItemPrice;
        private decimal _totalQuantity;
        private decimal _totalAmount;
        private SupplierOrderModel _supplierOrderModel;
        private decimal _supplierCashBookAmount;
        //CurrentTransactionRepository currentTransaction = new CurrentTransactionRepository();

        #endregion

        #region Ctor

        public SupplierMarketingViewModel()
        {
            PurchaseOrderCommand = new RelayCommand<string>(ExecutePurchaseOrderCommand);
            _supplierMarketingOrderRepository = new SupplierMarketingBillRepository(new HAFoodDbContext());
            _supplierRepository = App.Resolve<IGenericRepository<Entities.DBModel.Suppliers.Supplier>>();
            _stockRepository = new GenericRepository<Entities.DBModel.Stocks.Stock>(new HAFoodDbContext());
            _paymentRepository = App.Resolve<IGenericRepository<Payment>>();
            _supplierMarketingOrderAmountRepository = App.Resolve<IGenericRepository<SupplierMarketingOrderAmount>>();
            _cashBookOneRepository = App.Resolve<IGenericRepository<CashBookOne>>();
            StockList = new ObservableCollection<StockModel>();
            SupplierList = new ObservableCollection<SupplierModel>();
        }

        #endregion

        #region Properties
        public RelayCommand<string> PurchaseOrderCommand { get; set; }

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

        public SupplierMarketingOrderModel SupplierMarketingOrderModel
        {
            get { return _supplierMarketingOrderModel; }
            set { _supplierMarketingOrderModel = value; RaisePropertyChanged("SupplierMarketingOrderModel"); }
        }

        public ObservableCollection<SupplierMarketingOrderModel> PurchaseOrderList
        {
            get { return _supplierMarketingOrderList; }
            set { _supplierMarketingOrderList = value; RaisePropertyChanged("PurchaseOrderList"); }
        }

        public SupplierMarketingOrderItemModel SupplierMarketingOrderItemModel
        {
            get { return _supplierMarketingOrderItemModel; }
            set { _supplierMarketingOrderItemModel = value; RaisePropertyChanged("SupplierMarketingOrderItemModel"); }
        }

        public ObservableCollection<SupplierMarketingOrderItemModel> PurchaseOrderItemList
        {
            get { return _supplierMarketingOrderItemList; }
            set { _supplierMarketingOrderItemList = value; RaisePropertyChanged("PurchaseOrderItemList"); }
        }

        public SupplierModel SupplierModel
        {
            get { return _supplierModel; }
            set
            {
                _supplierModel = value;
                if (_supplierModel != null)
                    GetSupplierMarketingAmount(_supplierModel.Id);
                RaisePropertyChanged("SupplierModel");
            }
        }

        public ObservableCollection<SupplierModel> SupplierList
        {
            get { return _supplierList; }
            set { _supplierList = value; RaisePropertyChanged("SupplierList"); }
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

        public decimal TotalQuantity
        {
            get { return _totalQuantity; }
            set { _totalQuantity = value; RaisePropertyChanged("TotalQuantity"); }
        }

        public decimal TotalAmount
        {
            get { return _totalAmount; }
            set { _totalAmount = value; RaisePropertyChanged("TotalAmount"); }
        }


        public SupplierOrderModel SupplierOrderModel
        {
            get { return _supplierOrderModel; }
            set { _supplierOrderModel = value; RaisePropertyChanged("SupplierOrderModel"); }
        }

        public decimal SupplierCashBookAmount
        {
            get { return _supplierCashBookAmount; }
            set { _supplierCashBookAmount = value; RaisePropertyChanged("SupplierCashBookAmount"); }
        }

        #endregion

        #region Methods

        private void ExecutePurchaseOrderCommand(string str)
        {

            if (str == "Add Item")
            {
                if (Quantity == 0)
                {
                    ApplicationManager.Instance.ShowMessageBox("Please add Quantity");
                    return;
                }

                SupplierMarketingOrderItemModel = PurchaseOrderItemList.Where(x => x.ItemName == StockModel.ItemName).FirstOrDefault();

                if (SupplierMarketingOrderItemModel != null)
                {
                    SupplierMarketingOrderItemModel.ItemName = StockModel.ItemName;
                    SupplierMarketingOrderItemModel.BuyPrice = StockModel.BuyPrice;
                    //if (_isPreviousOrder)
                    //{
                    SupplierMarketingOrderItemModel.NewQuantity = SupplierMarketingOrderItemModel.NewQuantity + Quantity;
                    //}
                    SupplierMarketingOrderItemModel.PurchaseQuantity = Quantity;
                    SupplierMarketingOrderItemModel.NewDiscount = SupplierMarketingOrderItemModel.NewDiscount + Discount;
                    SupplierMarketingOrderItemModel.Discount = Discount;
                    decimal totalPrice = (SupplierMarketingOrderItemModel.BuyPrice * StockModel.Packing) * SupplierMarketingOrderItemModel.NewQuantity;
                    SupplierMarketingOrderItemModel.TotalPrice = totalPrice - SupplierMarketingOrderItemModel.NewDiscount;
                }
                else
                {
                    PurchaseOrderItemList.Add(new SupplierMarketingOrderItemModel()
                    {
                        StockId = StockModel.Id,
                        ItemName = StockModel.ItemName,
                        Packing = StockModel.Packing,
                        BuyPrice = StockModel.BuyPrice,
                        PurchaseQuantity = Quantity,
                        NewQuantity = Quantity,
                        Discount = Discount,
                        NewDiscount = Discount,
                        TotalPrice = ((StockModel.BuyPrice * StockModel.Packing) * Quantity) - Discount
                    });
                }

                decimal price = PurchaseOrderItemList.Sum(x => x.TotalPrice);

                SupplierMarketingOrderModel.TotalPrice = price;
                SupplierMarketingOrderModel.TotalDiscount = PurchaseOrderItemList.Sum(x => x.NewDiscount);
                if (_isPreviousOrder)
                {
                    //SupplierMarketingOrderModel.GrandTotal = ((((SupplierMarketingOrderItemModel != null) ? SupplierMarketingOrderItemModel.BuyPrice : StockModel.BuyPrice) * Quantity) + SupplierOrderModel.RemainingAmount) - Discount;
                    var newItemTotalPrice = (StockModel.BuyPrice * StockModel.Packing) * Quantity;
                    SupplierMarketingOrderModel.GrandTotal += newItemTotalPrice;
                    NewItemPrice += ((SupplierMarketingOrderItemModel != null) ? SupplierMarketingOrderItemModel.BuyPrice * StockModel.Packing : StockModel.BuyPrice * StockModel.Packing) * Quantity;
                }
                else
                    SupplierMarketingOrderModel.GrandTotal = (SupplierMarketingOrderModel.TotalPrice + SupplierOrderModel.RemainingAmount) - SupplierMarketingOrderModel.TotalDiscount - SupplierCashBookAmount;
            }
            else if (str == "Save Order")
            {
                var bw = new BackgroundWorker();
                bw.DoWork += (sender, args) =>
                {
                    try
                    {
                        ApplicationManager.Instance.ShowBusyInidicator("Saving Order ... !");

                        SupplierMarketingOrder supplierMarketingOrder = new SupplierMarketingOrder();

                        if (_isPreviousOrder)
                        {
                            supplierMarketingOrder.Id = _currentTransactionId;
                            supplierMarketingOrder.UpdatedDate = DateTime.Now;
                        }
                        else
                        {
                            supplierMarketingOrder.CreatedDate = DateTime.Now;
                        }
                        supplierMarketingOrder.OrderNo = SupplierMarketingOrderModel.OrderNo;
                        supplierMarketingOrder.SupplierId = SupplierModel.Id;
                        supplierMarketingOrder.PaymentId = PaymentType.Id;
                        supplierMarketingOrder.TotalPrice = SupplierMarketingOrderModel.TotalPrice;
                        supplierMarketingOrder.TotalDiscount = SupplierMarketingOrderModel.TotalDiscount;
                        supplierMarketingOrder.GrandTotal = SupplierMarketingOrderModel.GrandTotal;
                        supplierMarketingOrder.AmountPaid = SupplierMarketingOrderModel.AmountPaid;
                        supplierMarketingOrder.CreatedDate = DateTime.Now;
                        supplierMarketingOrder.PurchaseOrderDate = SupplierMarketingOrderModel.PurchaseOrderDate;

                        foreach (var item in PurchaseOrderItemList)
                        {
                            SupplierMarketingOrderItem supplierMarketingOrderItem = new SupplierMarketingOrderItem();
                            if (_isPreviousOrder)
                            {
                                supplierMarketingOrderItem.Id = item.Id;
                                supplierMarketingOrderItem.UpdatedDate = DateTime.Now;
                            }
                            else
                            {
                                supplierMarketingOrderItem.CreatedDate = DateTime.Now;
                            }
                            supplierMarketingOrderItem.StockId = item.StockId;
                            supplierMarketingOrderItem.ItemName = item.ItemName;
                            supplierMarketingOrderItem.Packing = item.Packing;
                            //if (!_isPreviousOrder)
                            //    supplierMarketingOrderItem.PreviousQuantity = item.PreviousQuantity;
                            supplierMarketingOrderItem.PurchaseQuantity = item.NewQuantity;
                            supplierMarketingOrderItem.BuyPrice = item.BuyPrice;
                            supplierMarketingOrderItem.Discount = item.Discount;
                            supplierMarketingOrderItem.TotalPrice = item.TotalPrice;
                            supplierMarketingOrder.SupplierMarketingOrderItems.Add(supplierMarketingOrderItem);
                        }

                        SaveSupplierMarketingAmount();

                        SupplierMarketingOrder t = _supplierMarketingOrderRepository.SaveSupplierMarketingOrder(supplierMarketingOrder);

                        CalculateStock(PurchaseOrderItemList);
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

                    ApplicationManager.Instance.ShowDialog("Order Saved!", new PurchasePaymentPopup(this));
                    Reset();
                };

                bw.RunWorkerAsync();

            }

            else if (str == "Search Order")
            {
                PurchaseOrderItemList = new ObservableCollection<SupplierMarketingOrderItemModel>();
                var result = _supplierMarketingOrderRepository.GetOrder(OrderNumber);
                if (result != null)
                {
                    _isPreviousOrder = true;

                    GetSupplierMarketingAmount(Convert.ToInt32(result.SupplierId));

                    _currentTransactionId = result.Id;
                    SupplierMarketingOrderModel.OrderNo = result.OrderNo;
                    SupplierMarketingOrderModel.TotalPrice = result.TotalPrice;
                    SupplierMarketingOrderModel.TotalDiscount = result.TotalDiscount;
                    SupplierMarketingOrderModel.GrandTotal = SupplierOrderModel.RemainingAmount;//result.GrandTotal;

                    foreach (var item in result.SupplierMarketingOrderItems)
                    {
                        PurchaseOrderItemList.Add(new SupplierMarketingOrderItemModel()
                        {
                            Id = item.Id,
                            StockId = item.StockId,
                            ItemName = item.ItemName,
                            Packing = item.Packing,
                            BuyPrice = item.BuyPrice,
                            //PreviousQuantity = item.PreviousQuantity,
                            NewQuantity = item.PurchaseQuantity,
                            NewDiscount = item.Discount,
                            TotalPrice = ((item.BuyPrice * item.Packing) * item.PurchaseQuantity) - item.Discount
                        });
                    }
                    SupplierModel = SupplierList.FirstOrDefault(x => x.Id == result.SupplierId);
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

        private void GetSupplierMarketingAmount(int supplierId)
        {
            SupplierOrderModel = new SupplierOrderModel();
            List<SupplierMarketingOrderAmount> supplierMarketingOrderAmount = _supplierMarketingOrderAmountRepository.Get().Where(x => x.SupplierId == supplierId).ToList();
            if (supplierMarketingOrderAmount != null && supplierMarketingOrderAmount.Count > 0)
            {
                SupplierOrderModel.SupplierId = supplierId;
                SupplierOrderModel.AmountPaid = supplierMarketingOrderAmount.Sum(x => x.AmountPaid);
                SupplierOrderModel.RemainingAmount = supplierMarketingOrderAmount.Sum(x => x.RemainingAmount);
                SupplierOrderModel.TotalAmount = supplierMarketingOrderAmount.Sum(x => x.TotalAmount);
                SupplierCashBookAmount = _cashBookOneRepository.Get().Where(x => x.SupplierId == supplierId).Sum(x => x.Amount);
            }
        }

        private void SaveSupplierMarketingAmount()
        {
            SupplierMarketingOrderAmount supplierMarketingOrderAmount = _supplierMarketingOrderAmountRepository.Get().FirstOrDefault(x => x.OrderNo == SupplierMarketingOrderModel.OrderNo);

            if (supplierMarketingOrderAmount == null)
            {
                SupplierMarketingOrderAmount suppOrder = new SupplierMarketingOrderAmount
                {
                    SupplierId = SupplierModel.Id,
                    OrderNo = SupplierMarketingOrderModel.OrderNo,
                    AmountPaid = SupplierMarketingOrderModel.AmountPaid,
                    RemainingAmount = SupplierMarketingOrderModel.TotalPrice - SupplierMarketingOrderModel.AmountPaid,
                    TotalAmount = SupplierMarketingOrderModel.TotalPrice,
                    Balance = SupplierMarketingOrderModel.GrandTotal - SupplierMarketingOrderModel.AmountPaid,
                    CreatedDate = DateTime.Now
                };

                GrandTotal = suppOrder.TotalAmount;
                AmountPaid = suppOrder.AmountPaid;
                RemainingAmount = suppOrder.RemainingAmount;

                _supplierMarketingOrderAmountRepository.Add(suppOrder);
            }
            else
            {
                supplierMarketingOrderAmount.SupplierId = SupplierModel.Id;
                supplierMarketingOrderAmount.OrderNo = SupplierMarketingOrderModel.OrderNo;
                supplierMarketingOrderAmount.AmountPaid = SupplierMarketingOrderModel.AmountPaid + supplierMarketingOrderAmount.AmountPaid;
                supplierMarketingOrderAmount.RemainingAmount = SupplierMarketingOrderModel.TotalPrice - SupplierMarketingOrderModel.AmountPaid;
                supplierMarketingOrderAmount.TotalAmount = NewItemPrice + supplierMarketingOrderAmount.TotalAmount;
                supplierMarketingOrderAmount.Balance = SupplierMarketingOrderModel.GrandTotal - SupplierMarketingOrderModel.AmountPaid;
                supplierMarketingOrderAmount.UpdatedDate = DateTime.Now;

                GrandTotal = supplierMarketingOrderAmount.TotalAmount;
                AmountPaid = supplierMarketingOrderAmount.AmountPaid;
                RemainingAmount = supplierMarketingOrderAmount.Balance;

                _supplierMarketingOrderAmountRepository.Update(supplierMarketingOrderAmount, supplierMarketingOrderAmount.Id);
            }
            //}
            //else
            //{
            //    supplierMarketingOrderAmount.SupplierId = SupplierModel.Id;
            //    supplierMarketingOrderAmount.AmountPaid = SupplierMarketingOrderModel.AmountPaid + supplierMarketingOrderAmount.AmountPaid;
            //    supplierMarketingOrderAmount.RemainingAmount = SupplierMarketingOrderModel.GrandTotal - SupplierMarketingOrderModel.AmountPaid;
            //    if (_isPreviousOrder)
            //        supplierMarketingOrderAmount.TotalAmount = NewItemPrice + supplierMarketingOrderAmount.TotalAmount;
            //    else
            //        supplierMarketingOrderAmount.TotalAmount = (SupplierMarketingOrderModel.TotalPrice - SupplierMarketingOrderModel.TotalDiscount) + supplierMarketingOrderAmount.TotalAmount;

            //    GrandTotal = supplierMarketingOrderAmount.TotalAmount;
            //    AmountPaid = supplierMarketingOrderAmount.AmountPaid;
            //    RemainingAmount = supplierMarketingOrderAmount.RemainingAmount;

            //    _supplierMarketingOrderAmountRepository.Update(supplierMarketingOrderAmount, supplierMarketingOrderAmount.Id);
            //}
        }

        private void Reset()
        {
            SupplierMarketingOrderItemModel = new SupplierMarketingOrderItemModel();
            PurchaseOrderItemList = new ObservableCollection<SupplierMarketingOrderItemModel>();
            SupplierMarketingOrderModel = new SupplierMarketingOrderModel();
            SupplierModel = new SupplierModel();
            GetPaymentType();
            GetSupplier();
            GetItems();
            _isPreviousOrder = false;
            OrderNumber = "Search Order";
            Quantity = 0;
            Discount = 0;
            NewItemPrice = 0;
            //SupplierCashBookAmount = 0;
            GetOrderNumber();
            SupplierMarketingOrderModel.PurchaseOrderDate = DateTime.Now;
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
            var result = _supplierMarketingOrderRepository.Get().LastOrDefault();
            if (result != null)
            {
                int orderNo = Convert.ToInt32(result.OrderNo) + 1;
                SupplierMarketingOrderModel.OrderNo = orderNo.ToString();
            }
            else
                SupplierMarketingOrderModel.OrderNo = "1000";
        }

        private void GetSupplier()
        {
            SupplierModel = SupplierList.FirstOrDefault();
        }

        private void GetItems()
        {
            StockModel = StockList.FirstOrDefault();
        }

        private void LoadCollections()
        {
            SupplierList = MapperProfile.iMapper.Map<ObservableCollection<SupplierModel>>(_supplierRepository.Get());
            StockList = MapperProfile.iMapper.Map<ObservableCollection<StockModel>>(_stockRepository.Get());
            PaymentList = MapperProfile.iMapper.Map<ObservableCollection<PaymentModel>>(_paymentRepository.Get());
        }

        private void CalculateStock(ObservableCollection<SupplierMarketingOrderItemModel> currentTransactionDetailModel)
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
                    stock.Quantity = stock.Quantity + item.PurchaseQuantity;
                    _stockRepository.Update(stock, stock.Id);
                }
            }
        }

        private void LoadReport(SupplierMarketingOrder supplierMarketingOrder)
        {
            DataTable dt = new DataTable();
            string constr = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\HAFood\HAFoodDB.mdf; Integrated Security = True;";
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("[dbo].[SP_GetSupplierMarketingBill]", con))
                {
                    cmd.Parameters.AddWithValue("@SupplierMarketingBillId", supplierMarketingOrder.Id);
                    cmd.Parameters.AddWithValue("@SupplierId", supplierMarketingOrder.SupplierId);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                }
            }
            if (dt.Rows.Count > 0)
            {
                ApplicationManager.Instance.PrintReport(dt, @"/Reports/rptSupplierMarketingBill", "dsPurchaseOrder", "SupplierMarketingOrder");
            }
        }

        public void OnBringIntoView()
        {
            Init();
        }

        #endregion
    }
}
