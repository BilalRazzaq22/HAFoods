using ERP.Common;
using ERP.Common.NotifyProperty;
using ERP.Entities.DbContext;
using ERP.Entities.DBModel.CashBook;
using ERP.Entities.DBModel.Payments;
using ERP.Repository.Generic;
using ERP.WpfClient.Controls.Helpers;
using ERP.WpfClient.Mapper;
using ERP.WpfClient.Model;
using ERP.WpfClient.Model.CashBooks;
using ERP.WpfClient.Model.Payment;
using ERP.WpfClient.Model.Supplier;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ERP.WpfClient.ViewModel.CashBooks
{
    public class CashBookOneViewModel : ViewModelBase, INotifyOnBringIntoView
    {
        #region Fields

        private readonly IGenericRepository<CashBookOne> _cashBookOneRepository;
        private readonly IGenericRepository<Entities.DBModel.Customers.Customer> _customerRepository;
        private readonly IGenericRepository<Entities.DBModel.Suppliers.Supplier> _supplierRepository;
        private readonly IGenericRepository<Payment> _paymentRepository;
        private CashBookOneModel _cashBookOneModel;
        private ObservableCollection<CashBookOneModel> _cashBookOneList;
        private string _cashBookOneButton;
        private string _cashBookOneParameter;
        private CustomerModel _customerModel;
        private ObservableCollection<CustomerModel> _customerList;
        private ObservableCollection<PaymentModel> _paymentList;
        private PaymentModel _paymentType;
        private ObservableCollection<SupplierModel> _supplierList;
        private SupplierModel _supplierModel;
        private ObservableCollection<CashBookTypeModel> _cashBookTypeList;
        private CashBookTypeModel _cashBookType;
        private string _isCustomer;
        private string _isSupplier;

        #endregion

        #region Ctor

        public CashBookOneViewModel()
        {
            CashBookOneCommands = new RelayCommand<object>(ExecuteCashBookOneCommand);
            DeleteCashBookOneCommand = new RelayCommand<object>(ExecuteDeleteCashBookOneCommand);
            _cashBookOneRepository = App.Resolve<IGenericRepository<CashBookOne>>();
            _customerRepository = App.Resolve<IGenericRepository<Entities.DBModel.Customers.Customer>>();
            _supplierRepository = App.Resolve<IGenericRepository<Entities.DBModel.Suppliers.Supplier>>();
            _paymentRepository = App.Resolve<IGenericRepository<Payment>>();
            CashBookOneModel = new CashBookOneModel();
            CashBookOneList = new ObservableCollection<CashBookOneModel>();
            CustomerList = new ObservableCollection<CustomerModel>();
            SupplierList = new ObservableCollection<SupplierModel>();
            PaymentList = new ObservableCollection<PaymentModel>();
            CashBookTypeList = new ObservableCollection<CashBookTypeModel>();
            CashBookOneButton = "Save";
            CashBookOneParameter = "SaveCashBookOne";
        }

        #endregion

        #region Properties
        public RelayCommand<object> CashBookOneCommands { get; set; }
        public RelayCommand<object> DeleteCashBookOneCommand { get; set; }

        //public CustomerCommand CashBookOneCommands { get; set; }

        public CashBookOneModel CashBookOneModel
        {
            get { return _cashBookOneModel; }
            set { _cashBookOneModel = value; RaisePropertyChanged("CashBookOneModel"); }
        }

        public ObservableCollection<CashBookOneModel> CashBookOneList
        {
            get { return _cashBookOneList; }
            set { _cashBookOneList = value; RaisePropertyChanged("CashBookOneList"); }
        }

        public string CashBookOneParameter
        {
            get { return _cashBookOneParameter; }
            set { _cashBookOneParameter = value; RaisePropertyChanged("CashBookOneParameter"); }
        }

        public string CashBookOneButton
        {
            get { return _cashBookOneButton; }
            set { _cashBookOneButton = value; RaisePropertyChanged("CashBookOneButton"); }
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

        private void ExecuteCashBookOneCommand(object str)
        {
            if (str as string == "SaveCashBookOne")
            {
                SaveCashBookOne();
            }
            else if (str as string == "UpdateCashBookOne")
            {
                UpdateCashBookOne();
            }
            else if (str as string == "Clear")
            {
                Reset();
            }
            else if (str != null)
            {
                EditCashBookOne(str as CashBookOneModel);
            }
        }

        private void Reset()
        {
            CashBookOneModel = new CashBookOneModel();
            CustomerModel = new CustomerModel();
            SupplierModel = new SupplierModel();
            PaymentType = new PaymentModel();
            CashBookType = new CashBookTypeModel();
            CashBookOneButton = "Save";
            CashBookOneParameter = "SaveCashBookOne";
            GetPaymentType();
            GetCustomer();
            GetSupplier();
            GetCashBookType();
            IsCustomer = Visibility.Collapsed.ToString();
            IsSupplier = Visibility.Collapsed.ToString();
            CashBookOneModel.CashBookOneDate = DateTime.Now;
        }

        private void ExecuteDeleteCashBookOneCommand(object obj)
        {
            if (obj != null)
            {
                ApplicationManager.Instance.ShowConfirmDialog("Are you sure you want to Delete the cash book one?", () =>
                {
                    DeleteCustomer(obj as CashBookOneModel);
                    ApplicationManager.Instance.HideDialog();
                }, () => ApplicationManager.Instance.HideMessageBox(), useYesNo: true);
            }
        }

        public void SaveCashBookOne()
        {
            if (CashBookType.Type == "Select Type")
            {
                ApplicationManager.Instance.ShowMessageBox("Please Select any Type");
                return;
            }
            else
            {
                if (CashBookType.Type == "Customer")
                {
                    CashBookOneModel.CustomerId = CustomerModel.Id;
                    CashBookOneModel.SupplierId = null;
                }
                else if (CashBookType.Type == "Supplier")
                {
                    CashBookOneModel.CustomerId = null;
                    CashBookOneModel.SupplierId = SupplierModel.Id;
                }

                CashBookOneModel.Type = CashBookType.Type;
                CashBookOneModel.PaymentId = PaymentType.Id;
                var model = _cashBookOneRepository.Add(MapperProfile.iMapper.Map<CashBookOne>(CashBookOneModel));
                CashBookOneModel.Id = model.Id;
                CashBookOneList.Add(CashBookOneModel);
                Reset();
            }
        }

        public void EditCashBookOne(CashBookOneModel cashBookOneModel)
        {
            CashBookOneButton = "Update";
            CashBookOneParameter = "UpdateCashBookOne";
            CashBookOneModel.Id = cashBookOneModel.Id;
            CashBookOneModel.Type = cashBookOneModel.Type;
            CashBookOneModel.CustomerId = cashBookOneModel.CustomerId;
            CashBookOneModel.SupplierId = cashBookOneModel.SupplierId;
            CashBookOneModel.Amount = cashBookOneModel.Amount;
            CashBookOneModel.PaymentId = cashBookOneModel.PaymentId;
            CashBookOneModel.Description = cashBookOneModel.Description;

            CustomerModel = CustomerList.FirstOrDefault(x => x.Id == cashBookOneModel.CustomerId);
            SupplierModel = SupplierList.FirstOrDefault(x => x.Id == cashBookOneModel.SupplierId);
            PaymentType = PaymentList.FirstOrDefault(x => x.Id == cashBookOneModel.PaymentId);
            CashBookType = CashBookTypeList.FirstOrDefault(x => x.Type == cashBookOneModel.Type);
        }

        public void UpdateCashBookOne()
        {
            CashBookOneModel.Type = CashBookType.Type;
            CashBookOneModel.CustomerId = CustomerModel.Id;
            CashBookOneModel.SupplierId = SupplierModel.Id;
            CashBookOneModel.PaymentId = PaymentType.Id;
            _cashBookOneRepository.Update(MapperProfile.iMapper.Map<CashBookOne>(CashBookOneModel), CashBookOneModel.Id);
            Reset();
        }

        public void DeleteCustomer(CashBookOneModel cashBookOneModel)
        {
            _cashBookOneRepository.Delete(cashBookOneModel.Id);
            CashBookOneList.Remove(cashBookOneModel);
        }

        private void Init()
        {
            var bw = new BackgroundWorker();
            List<CashBookOne> cashBookOnes = null;
            bw.DoWork += (sender, args) =>
            {
                try
                {
                    ApplicationManager.Instance.ShowBusyInidicator("Loading Data... !");
                    cashBookOnes = _cashBookOneRepository.Get();
                    InitializeCollection();
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
                    CashBookOneList = MapperProfile.iMapper.Map<ObservableCollection<CashBookOneModel>>(cashBookOnes);
                    InitializeCashBookTypeList();
                }));
                Reset();
                ApplicationManager.Instance.HideBusyInidicator();
            };

            bw.RunWorkerAsync();
        }

        private void InitializeCollection()
        {
            CustomerList = MapperProfile.iMapper.Map<ObservableCollection<CustomerModel>>(_customerRepository.Get());
            SupplierList = MapperProfile.iMapper.Map<ObservableCollection<SupplierModel>>(_supplierRepository.Get());
            PaymentList = MapperProfile.iMapper.Map<ObservableCollection<PaymentModel>>(_paymentRepository.Get());
        }

        private void InitializeCashBookTypeList()
        {
            CashBookTypeList = new ObservableCollection<CashBookTypeModel>();

            CashBookTypeList.Add(new CashBookTypeModel() { Type = "Select Type" });
            CashBookTypeList.Add(new CashBookTypeModel() { Type = "Customer" });
            CashBookTypeList.Add(new CashBookTypeModel() { Type = "Supplier" });
        }

        private void GetCustomer()
        {
            CustomerModel = CustomerList.FirstOrDefault();
        }

        private void GetPaymentType()
        {
            PaymentType = PaymentList.FirstOrDefault();
        }

        private void GetSupplier()
        {
            SupplierModel = SupplierList.FirstOrDefault();
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

        public void OnBringIntoView()
        {
            Init();
        }

        #endregion
    }
}
