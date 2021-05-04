using ERP.Common;
using ERP.Common.NotifyProperty;
using ERP.Entities.DbContext;
using ERP.Entities.DBModel;
using ERP.Entities.DBModel.Customers;
using ERP.Entities.DBModel.Payments;
using ERP.Entities.DBModel.PurchaseOrders;
using ERP.Entities.DBModel.Suppliers;
using ERP.Entities.DBModel.Transactions;
using ERP.Repository.Customer;
using ERP.Repository.Generic;
using ERP.Repository.PurchaseOrders;
using ERP.Repository.Transaction;
using ERP.WpfClient.Controls.Helpers;
using ERP.WpfClient.Mapper;
using ERP.WpfClient.Model;
using ERP.WpfClient.Model.Customer;
using ERP.WpfClient.Model.Payment;
using ERP.WpfClient.Model.PurchaseOrder;
using ERP.WpfClient.Model.Stock;
using ERP.WpfClient.Model.Supplier;
using ERP.WpfClient.Model.Transaction;
using ERP.WpfClient.View.Popups.Payments;
using GalaSoft.MvvmLight.Command;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
namespace ERP.WpfClient.ViewModel.PurchaseOrders
{
    public class PurchaseOrderViewModel : ViewModelBase, INotifyOnBringIntoView
    {
        #region Fields

        private readonly PurchaseOrderRepository _purchaseOrderRepository;
        private readonly IGenericRepository<Entities.DBModel.Suppliers.Supplier> _supplierRepository;
        private IGenericRepository<Entities.DBModel.Stocks.Stock> _stockRepository;
        private readonly IGenericRepository<Payment> _paymentRepository;
        private readonly IGenericRepository<SupplierOrder> _supplierOrderRepository;
        private PurchaseOrderModel _purchaseOrderModel;
        private PurchaseOrderItemModel _purchaseOrderItemModel;
        private ObservableCollection<PurchaseOrderModel> _purchaseOrderList;
        private ObservableCollection<PurchaseOrderItemModel> _purchaseOrderItemList;
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

        //CurrentTransactionRepository currentTransaction = new CurrentTransactionRepository();

        #endregion

        #region Ctor

        public PurchaseOrderViewModel()
        {
            PurchaseOrderCommand = new RelayCommand<string>(ExecutePurchaseOrderCommand);
            _purchaseOrderRepository = new PurchaseOrderRepository(new HAFoodDbContext());
            _supplierRepository = App.Resolve<IGenericRepository<Entities.DBModel.Suppliers.Supplier>>();
            _stockRepository = new GenericRepository<Entities.DBModel.Stocks.Stock>(new HAFoodDbContext());
            _paymentRepository = App.Resolve<IGenericRepository<Payment>>();
            _supplierOrderRepository = App.Resolve<IGenericRepository<SupplierOrder>>();
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

        public PurchaseOrderModel PurchaseOrderModel
        {
            get { return _purchaseOrderModel; }
            set { _purchaseOrderModel = value; RaisePropertyChanged("PurchaseOrderModel"); }
        }

        public ObservableCollection<PurchaseOrderModel> PurchaseOrderList
        {
            get { return _purchaseOrderList; }
            set { _purchaseOrderList = value; RaisePropertyChanged("PurchaseOrderList"); }
        }

        public PurchaseOrderItemModel PurchaseOrderItemModel
        {
            get { return _purchaseOrderItemModel; }
            set { _purchaseOrderItemModel = value; RaisePropertyChanged("PurchaseOrderItemModel"); }
        }

        public ObservableCollection<PurchaseOrderItemModel> PurchaseOrderItemList
        {
            get { return _purchaseOrderItemList; }
            set { _purchaseOrderItemList = value; RaisePropertyChanged("PurchaseOrderItemList"); }
        }

        public SupplierModel SupplierModel
        {
            get { return _supplierModel; }
            set
            {
                _supplierModel = value;
                if (_supplierModel != null)
                    GetSupplierAmount(_supplierModel.Id);
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

                PurchaseOrderItemModel = PurchaseOrderItemList.Where(x => x.ItemName == StockModel.ItemName).FirstOrDefault();

                if (PurchaseOrderItemModel != null)
                {
                    PurchaseOrderItemModel.ItemName = StockModel.ItemName;
                    PurchaseOrderItemModel.BuyPrice = StockModel.BuyPrice * StockModel.Packing;
                    //if (_isPreviousOrder)
                    //{
                    PurchaseOrderItemModel.NewQuantity = PurchaseOrderItemModel.NewQuantity + Quantity;
                    //}
                    PurchaseOrderItemModel.PurchaseQuantity = Quantity;
                    PurchaseOrderItemModel.NewDiscount = PurchaseOrderItemModel.NewDiscount + Discount;
                    PurchaseOrderItemModel.Discount = Discount;
                    decimal totalPrice = PurchaseOrderItemModel.NewQuantity * PurchaseOrderItemModel.BuyPrice;
                    PurchaseOrderItemModel.TotalPrice = totalPrice - PurchaseOrderItemModel.NewDiscount;
                }
                else
                {
                    PurchaseOrderItemList.Add(new PurchaseOrderItemModel()
                    {
                        StockId = StockModel.Id,
                        ItemName = StockModel.ItemName,
                        BuyPrice = StockModel.BuyPrice,
                        PurchaseQuantity = Quantity,
                        NewQuantity = Quantity,
                        Discount = Discount,
                        NewDiscount = Discount,
                        TotalPrice = ((StockModel.BuyPrice * StockModel.Packing) * Quantity) - Discount
                    });
                }

                decimal price = PurchaseOrderItemList.Sum(x => x.BuyPrice * x.NewQuantity);

                PurchaseOrderModel.TotalPrice = price;
                PurchaseOrderModel.TotalDiscount = PurchaseOrderItemList.Sum(x => x.NewDiscount);
                if (_isPreviousOrder)
                {
                    PurchaseOrderModel.GrandTotal = ((((PurchaseOrderItemModel != null) ? PurchaseOrderItemModel.BuyPrice : StockModel.BuyPrice) * Quantity) + SupplierOrderModel.RemainingAmount) - Discount;
                    NewItemPrice += ((PurchaseOrderItemModel != null) ? PurchaseOrderItemModel.BuyPrice : StockModel.BuyPrice) * Quantity;
                }
                else
                    PurchaseOrderModel.GrandTotal = (PurchaseOrderModel.TotalPrice + SupplierOrderModel.RemainingAmount) - PurchaseOrderModel.TotalDiscount;
            }
            else if (str == "Save Order")
            {
                var bw = new BackgroundWorker();
                bw.DoWork += (sender, args) =>
                {
                    try
                    {
                        ApplicationManager.Instance.ShowBusyInidicator("Saving Order ... !");

                        PurchaseOrder pOrder = new PurchaseOrder();

                        if (_isPreviousOrder)
                        {
                            pOrder.Id = _currentTransactionId;
                            pOrder.UpdatedDate = DateTime.Now;
                        }
                        else
                        {
                            pOrder.CreatedDate = DateTime.Now;
                        }
                        pOrder.OrderNo = PurchaseOrderModel.OrderNo;
                        pOrder.SupplierId = SupplierModel.Id;
                        pOrder.PaymentId = PaymentType.Id;
                        pOrder.TotalPrice = PurchaseOrderModel.TotalPrice;
                        pOrder.TotalDiscount = PurchaseOrderModel.TotalDiscount;
                        pOrder.GrandTotal = PurchaseOrderModel.GrandTotal;
                        pOrder.AmountPaid = PurchaseOrderModel.AmountPaid;
                        pOrder.CreatedDate = DateTime.Now;
                        pOrder.PurchaseOrderDate = PurchaseOrderModel.PurchaseOrderDate;

                        foreach (var item in PurchaseOrderItemList)
                        {
                            PurchaseOrderItem pOrderItem = new PurchaseOrderItem();
                            if (_isPreviousOrder)
                            {
                                pOrderItem.Id = item.Id;
                                pOrderItem.UpdatedDate = DateTime.Now;
                            }
                            else
                            {
                                pOrderItem.CreatedDate = DateTime.Now;
                            }
                            pOrderItem.StockId = item.StockId;
                            pOrderItem.ItemName = item.ItemName;
                            //if (!_isPreviousOrder)
                            //    pOrderItem.PreviousQuantity = item.PreviousQuantity;
                            pOrderItem.PurchaseQuantity = item.NewQuantity;
                            pOrderItem.BuyPrice = item.BuyPrice;
                            pOrderItem.Discount = item.Discount;
                            pOrderItem.TotalPrice = item.TotalPrice;
                            pOrder.PurchaseOrderItems.Add(pOrderItem);
                        }

                        SaveSupplierAmount();

                        PurchaseOrder t = _purchaseOrderRepository.SavePurchaseOrder(pOrder);

                        CalculateStock(PurchaseOrderItemList);
                        LoadReport(t.Id);
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
                PurchaseOrderItemList = new ObservableCollection<PurchaseOrderItemModel>();
                var result = _purchaseOrderRepository.GetOrder(OrderNumber);
                if (result != null)
                {
                    _isPreviousOrder = true;

                    GetSupplierAmount(Convert.ToInt32(result.SupplierId));

                    _currentTransactionId = result.Id;
                    PurchaseOrderModel.OrderNo = result.OrderNo;
                    PurchaseOrderModel.TotalPrice = result.TotalPrice;
                    PurchaseOrderModel.TotalDiscount = result.TotalDiscount;
                    PurchaseOrderModel.GrandTotal = SupplierOrderModel.RemainingAmount;//result.GrandTotal;

                    foreach (var item in result.PurchaseOrderItems)
                    {
                        PurchaseOrderItemList.Add(new PurchaseOrderItemModel()
                        {
                            Id = item.Id,
                            StockId = item.StockId,
                            ItemName = item.ItemName,
                            BuyPrice = item.BuyPrice,
                            //PreviousQuantity = item.PreviousQuantity,
                            NewQuantity = item.PurchaseQuantity,
                            NewDiscount = item.Discount,
                            TotalPrice = (item.BuyPrice * item.PurchaseQuantity) - item.Discount
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

        private void GetSupplierAmount(int supplierId)
        {
            SupplierOrderModel = new SupplierOrderModel();
            List<SupplierOrder> supplierOrder = _supplierOrderRepository.Get().Where(x => x.SupplierId == supplierId).ToList();
            if (supplierOrder != null && supplierOrder.Count > 0)
            {
                SupplierOrderModel.SupplierId = supplierId;
                SupplierOrderModel.AmountPaid = supplierOrder.Sum(x => x.AmountPaid);
                SupplierOrderModel.RemainingAmount = supplierOrder.Sum(x => x.RemainingAmount);
                SupplierOrderModel.TotalAmount = supplierOrder.Sum(x => x.TotalAmount);
            }
        }

        private void SaveSupplierAmount()
        {
            //SupplierOrder supplierOrder = _supplierOrderRepository.Get().FirstOrDefault(x => x.SupplierId == SupplierModel.Id);
            //if (supplierOrder == null)
            //{
            SupplierOrder suppOrder = new SupplierOrder
            {
                SupplierId = SupplierModel.Id,
                AmountPaid = PurchaseOrderModel.AmountPaid,
                RemainingAmount = PurchaseOrderModel.GrandTotal - PurchaseOrderModel.AmountPaid,
                TotalAmount = PurchaseOrderModel.TotalPrice
            };

            GrandTotal = suppOrder.TotalAmount;
            AmountPaid = suppOrder.AmountPaid;
            RemainingAmount = suppOrder.RemainingAmount;

            _supplierOrderRepository.Add(suppOrder);
            //}
            //else
            //{
            //    supplierOrder.SupplierId = SupplierModel.Id;
            //    supplierOrder.AmountPaid = PurchaseOrderModel.AmountPaid + supplierOrder.AmountPaid;
            //    supplierOrder.RemainingAmount = PurchaseOrderModel.GrandTotal - PurchaseOrderModel.AmountPaid;
            //    if (_isPreviousOrder)
            //        supplierOrder.TotalAmount = NewItemPrice + supplierOrder.TotalAmount;
            //    else
            //        supplierOrder.TotalAmount = (PurchaseOrderModel.TotalPrice - PurchaseOrderModel.TotalDiscount) + supplierOrder.TotalAmount;

            //    GrandTotal = supplierOrder.TotalAmount;
            //    AmountPaid = supplierOrder.AmountPaid;
            //    RemainingAmount = supplierOrder.RemainingAmount;

            //    _supplierOrderRepository.Update(supplierOrder, supplierOrder.Id);
            //}
        }

        private void Reset()
        {
            PurchaseOrderItemModel = new PurchaseOrderItemModel();
            PurchaseOrderItemList = new ObservableCollection<PurchaseOrderItemModel>();
            PurchaseOrderModel = new PurchaseOrderModel();
            GetPaymentType();
            GetSupplier();
            GetItems();
            _isPreviousOrder = false;
            OrderNumber = "Search Order";
            Quantity = 0;
            Discount = 0;
            NewItemPrice = 0;
            GetOrderNumber();
            PurchaseOrderModel.PurchaseOrderDate = DateTime.Now;
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
            var result = _purchaseOrderRepository.Get().LastOrDefault();
            if (result != null)
            {
                int orderNo = Convert.ToInt32(result.OrderNo) + 1;
                PurchaseOrderModel.OrderNo = orderNo.ToString();
            }
            else
                PurchaseOrderModel.OrderNo = "1000";
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

        private void CalculateStock(ObservableCollection<PurchaseOrderItemModel> currentTransactionDetailModel)
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

        private void LoadReport(int purchaseOrderId)
        {
            DataTable dt = new DataTable();
            string constr = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\HAFood\HAFoodDB.mdf; Integrated Security = True;";
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("[dbo].[SP_GetCurrentPurchaseOrder]", con))
                {
                    cmd.Parameters.AddWithValue("@PurchaseOrderId", purchaseOrderId);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                }
            }
            if (dt.Rows.Count > 0)
            {
                ApplicationManager.Instance.PrintReport(dt, @"/Reports/rptPurchaseOrder", "dsPurchaseOrder", "PurchaseOrder");
            }
        }

        public void OnBringIntoView()
        {
            Init();
        }

        #endregion
    }
}
