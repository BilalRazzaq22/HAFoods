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
    public class CashBookTwoViewModel : ViewModelBase, INotifyOnBringIntoView
    {
        #region Fields

        private readonly IGenericRepository<CashBookTwo> _cashBookTwoRepository;
        private readonly IGenericRepository<Entities.DBModel.Customers.Customer> _customerRepository;
        private readonly IGenericRepository<Entities.DBModel.Suppliers.Supplier> _supplierRepository;
        private readonly IGenericRepository<Payment> _paymentRepository;
        private CashBookTwoModel _cashBookTwoModel;
        private ObservableCollection<CashBookTwoModel> _cashBookTwoList;
        private string _cashBookTwoButton;
        private string _cashBookTwoParameter;
        private CustomerModel _debiterCustomer;
        private CustomerModel _crediterCustomer;
        private ObservableCollection<CustomerModel> _debiterCustomerList;
        private ObservableCollection<CustomerModel> _crediterCustomerList;
        private ObservableCollection<PaymentModel> _paymentList;
        private PaymentModel _paymentType;
        private ObservableCollection<SupplierModel> _debiterSupplierList;
        private ObservableCollection<SupplierModel> _crediterSupplierList;
        private SupplierModel _debiterSupplier;
        private SupplierModel _crediterSupplier;
        private ObservableCollection<CashBookTypeModel> _debiterTypeList;
        private ObservableCollection<CashBookTypeModel> _crediterTypeList;
        private CashBookTypeModel _debiterType;
        private CashBookTypeModel _crediterType;
        private string _isDebiterCustomer;
        private string _isDebiterSupplier;
        private string _isCrediterCustomer;
        private string _isCrediterSupplier;

        #endregion

        #region Ctor

        public CashBookTwoViewModel()
        {
            CashBookTwoCommands = new RelayCommand<object>(ExecuteCashBookTwoCommand);
            DeleteCashBookTwoCommand = new RelayCommand<object>(ExecuteDeleteCashBookTwoCommand);
            _cashBookTwoRepository = App.Resolve<IGenericRepository<CashBookTwo>>();
            _customerRepository = App.Resolve<IGenericRepository<Entities.DBModel.Customers.Customer>>();
            _supplierRepository = App.Resolve<IGenericRepository<Entities.DBModel.Suppliers.Supplier>>();
            _paymentRepository = App.Resolve<IGenericRepository<Payment>>();
            CashBookTwoModel = new CashBookTwoModel();
            CashBookTwoList = new ObservableCollection<CashBookTwoModel>();
            DebiterCustomerList = new ObservableCollection<CustomerModel>();
            CrediterCustomerList = new ObservableCollection<CustomerModel>();
            DebiterSupplierList = new ObservableCollection<SupplierModel>();
            CrediterSupplierList = new ObservableCollection<SupplierModel>();
            PaymentList = new ObservableCollection<PaymentModel>();
            DebiterTypeList = new ObservableCollection<CashBookTypeModel>();
            CashBookTwoButton = "Save";
            CashBookTwoParameter = "SaveCashBookTwo";
        }

        #endregion

        #region Properties
        public RelayCommand<object> CashBookTwoCommands { get; set; }
        public RelayCommand<object> DeleteCashBookTwoCommand { get; set; }

        //public CustomerCommand CashBookTwoCommands { get; set; }

        public CashBookTwoModel CashBookTwoModel
        {
            get { return _cashBookTwoModel; }
            set { _cashBookTwoModel = value; RaisePropertyChanged("CashBookTwoModel"); }
        }

        public ObservableCollection<CashBookTwoModel> CashBookTwoList
        {
            get { return _cashBookTwoList; }
            set { _cashBookTwoList = value; RaisePropertyChanged("CashBookTwoList"); }
        }

        public string CashBookTwoParameter
        {
            get { return _cashBookTwoParameter; }
            set { _cashBookTwoParameter = value; RaisePropertyChanged("CashBookTwoParameter"); }
        }

        public string CashBookTwoButton
        {
            get { return _cashBookTwoButton; }
            set { _cashBookTwoButton = value; RaisePropertyChanged("CashBookTwoButton"); }
        }

        public CustomerModel DebiterCustomer
        {
            get { return _debiterCustomer; }
            set { _debiterCustomer = value; RaisePropertyChanged("DebiterCustomer"); }
        }

        public ObservableCollection<CustomerModel> DebiterCustomerList
        {
            get { return _debiterCustomerList; }
            set { _debiterCustomerList = value; RaisePropertyChanged("DebiterCustomerList"); }
        }

        public CustomerModel CrediterCustomer
        {
            get { return _crediterCustomer; }
            set { _crediterCustomer = value; RaisePropertyChanged("CrediterCustomer"); }
        }

        public ObservableCollection<CustomerModel> CrediterCustomerList
        {
            get { return _crediterCustomerList; }
            set { _crediterCustomerList = value; RaisePropertyChanged("CrediterCustomerList"); }
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

        public SupplierModel DebiterSupplier
        {
            get { return _debiterSupplier; }
            set { _debiterSupplier = value; RaisePropertyChanged("DebiterSupplier"); }
        }

        public ObservableCollection<SupplierModel> DebiterSupplierList
        {
            get { return _debiterSupplierList; }
            set { _debiterSupplierList = value; RaisePropertyChanged("DebiterSupplierList"); }
        }

        public SupplierModel CrediterSupplier
        {
            get { return _crediterSupplier; }
            set { _crediterSupplier = value; RaisePropertyChanged("CrediterSupplier"); }
        }

        public ObservableCollection<SupplierModel> CrediterSupplierList
        {
            get { return _crediterSupplierList; }
            set { _crediterSupplierList = value; RaisePropertyChanged("CrediterSupplierList"); }
        }

        public CashBookTypeModel DebiterType
        {
            get { return _debiterType; }
            set
            {
                _debiterType = value;
                if (_debiterType != null)
                    GetDebiterType(_debiterType);
                RaisePropertyChanged("DebiterType");
            }
        }

        public CashBookTypeModel CrediterType
        {
            get { return _crediterType; }
            set
            {
                _crediterType = value;
                if (_crediterType != null)
                    GetCrediterType(_crediterType);
                RaisePropertyChanged("CrediterType");
            }
        }

        public ObservableCollection<CashBookTypeModel> DebiterTypeList
        {
            get { return _debiterTypeList; }
            set { _debiterTypeList = value; RaisePropertyChanged("DebiterTypeList"); }
        }

        public ObservableCollection<CashBookTypeModel> CrediterTypeList
        {
            get { return _crediterTypeList; }
            set { _crediterTypeList = value; RaisePropertyChanged("CrediterTypeList"); }
        }

        public string IsDebiterCustomer
        {
            get { return _isDebiterCustomer; }
            set { _isDebiterCustomer = value; RaisePropertyChanged("IsDebiterCustomer"); }
        }

        public string IsDebiterSupplier
        {
            get { return _isDebiterSupplier; }
            set { _isDebiterSupplier = value; RaisePropertyChanged("IsDebiterSupplier"); }
        }

        public string IsCrediterCustomer
        {
            get { return _isCrediterCustomer; }
            set { _isCrediterCustomer = value; RaisePropertyChanged("IsCrediterCustomer"); }
        }

        public string IsCrediterSupplier
        {
            get { return _isCrediterSupplier; }
            set { _isCrediterSupplier = value; RaisePropertyChanged("IsCrediterSupplier"); }
        }

        #endregion

        #region Methods

        private void ExecuteCashBookTwoCommand(object str)
        {
            if (str as string == "SaveCashBookTwo")
            {
                SaveCashBookTwo();
            }
            else if (str as string == "UpdateCashBookTwo")
            {
                UpdateCashBookTwo();
            }
            else if (str as string == "Clear")
            {
                Reset();
            }
            else if (str != null)
            {
                EditCashBookTwo(str as CashBookTwoModel);
            }
        }

        private void Reset()
        {
            CashBookTwoModel = new CashBookTwoModel();
            DebiterCustomer = new CustomerModel();
            CrediterCustomer = new CustomerModel();
            DebiterSupplier = new SupplierModel();
            CrediterSupplier = new SupplierModel();
            PaymentType = new PaymentModel();
            DebiterType = new CashBookTypeModel();
            CrediterType = new CashBookTypeModel();
            CashBookTwoButton = "Save";
            CashBookTwoParameter = "SaveCashBookTwo";
            GetPaymentType();
            GetDebiterCustomer();
            GetDebiterSupplier();
            GetCrediterCustomer();
            GetCrediterSupplier();
            GetDebiterType();
            GetCrediterType();
            IsDebiterCustomer = Visibility.Visible.ToString();
            IsDebiterSupplier = Visibility.Collapsed.ToString();
            IsCrediterCustomer = Visibility.Visible.ToString();
            IsCrediterSupplier = Visibility.Collapsed.ToString();
            CashBookTwoModel.CashBookTwoDate = DateTime.Now;
        }

        private void ExecuteDeleteCashBookTwoCommand(object obj)
        {
            if (obj != null)
            {
                ApplicationManager.Instance.ShowConfirmDialog("Are you sure you want to Delete the cash book two?", () =>
                {
                    DeleteCustomer(obj as CashBookTwoModel);
                    ApplicationManager.Instance.HideDialog();
                }, () => ApplicationManager.Instance.HideMessageBox(), useYesNo: true);
            }
        }

        public void SaveCashBookTwo()
        {
            if (DebiterType.Type == "Select Type")
            {
                ApplicationManager.Instance.ShowMessageBox("Please Select Debitor Type");
                return;
            }
            else if (CrediterType.Type == "Select Type")
            {
                ApplicationManager.Instance.ShowMessageBox("Please Select Creditor Type");
                return;
            }


            CashBookTwoModel.DebiterType = DebiterType.Type;
            CashBookTwoModel.CrediterType = CrediterType.Type;

            if (DebiterType.Type == "Supplier")
            {
                CashBookTwoModel.DebiterCustomerId = null;
                CashBookTwoModel.DebiterSupplierId = DebiterSupplier.Id;
            }
            else if (DebiterType.Type == "Customer")
            {
                CashBookTwoModel.DebiterCustomerId = DebiterCustomer.Id;
                CashBookTwoModel.DebiterSupplierId = null;
            }

            if (CrediterType.Type == "Supplier")
            {

                CashBookTwoModel.CrediterCustomerId = null;
                CashBookTwoModel.CrediterSupplierId = CrediterSupplier.Id;
            }
            else if (CrediterType.Type == "Customer")
            {

                CashBookTwoModel.CrediterCustomerId = CrediterCustomer.Id;
                CashBookTwoModel.CrediterSupplierId = null;
            }


     
            var model = _cashBookTwoRepository.Add(MapperProfile.iMapper.Map<CashBookTwo>(CashBookTwoModel));
            CashBookTwoModel.Id = model.Id;
            CashBookTwoList.Add(CashBookTwoModel);
            Reset();
        }

        public void EditCashBookTwo(CashBookTwoModel cashBookOneModel)
        {
            CashBookTwoButton = "Update";
            CashBookTwoParameter = "UpdateCashBookTwo";
            CashBookTwoModel.Id = cashBookOneModel.Id;
            CashBookTwoModel.DebiterType = cashBookOneModel.DebiterType;
            CashBookTwoModel.DebiterCustomerId = cashBookOneModel.DebiterCustomerId;
            CashBookTwoModel.DebiterSupplierId = cashBookOneModel.DebiterSupplierId;
            CashBookTwoModel.DebiterAmount = cashBookOneModel.DebiterAmount;
            CashBookTwoModel.DebiterDescription = cashBookOneModel.DebiterDescription;
            CashBookTwoModel.CrediterType = cashBookOneModel.CrediterType;
            CashBookTwoModel.CrediterCustomerId = cashBookOneModel.CrediterCustomerId;
            CashBookTwoModel.CrediterSupplierId = cashBookOneModel.CrediterSupplierId;
            CashBookTwoModel.CrediterDescription = cashBookOneModel.CrediterDescription;
            CashBookTwoModel.CashBookTwoDate = cashBookOneModel.CashBookTwoDate;


            DebiterCustomer = DebiterCustomerList.FirstOrDefault(x => x.Id == cashBookOneModel.DebiterCustomerId);
            DebiterSupplier = DebiterSupplierList.FirstOrDefault(x => x.Id == cashBookOneModel.DebiterSupplierId);
            CrediterCustomer = CrediterCustomerList.FirstOrDefault(x => x.Id == cashBookOneModel.CrediterCustomerId);
            CrediterSupplier = CrediterSupplierList.FirstOrDefault(x => x.Id == cashBookOneModel.CrediterSupplierId);
            DebiterType = DebiterTypeList.FirstOrDefault(x => x.Type == cashBookOneModel.DebiterType);
            CrediterType = CrediterTypeList.FirstOrDefault(x => x.Type == cashBookOneModel.CrediterType);
        }

        public void UpdateCashBookTwo()
        {
            CashBookTwoModel.DebiterType = DebiterType.Type;
            CashBookTwoModel.CrediterType = CrediterType.Type;
            CashBookTwoModel.CrediterCustomerId = CrediterCustomer.Id;
            CashBookTwoModel.CrediterSupplierId = CrediterSupplier.Id;
            CashBookTwoModel.DebiterCustomerId = DebiterCustomer.Id;
            CashBookTwoModel.DebiterSupplierId = DebiterSupplier.Id;
            _cashBookTwoRepository.Update(MapperProfile.iMapper.Map<CashBookTwo>(CashBookTwoModel), CashBookTwoModel.Id);
            Reset();
        }

        public void DeleteCustomer(CashBookTwoModel cashBookOneModel)
        {
            _cashBookTwoRepository.Delete(cashBookOneModel.Id);
            CashBookTwoList.Remove(cashBookOneModel);
        }

        private void Init()
        {
            var bw = new BackgroundWorker();
            List<CashBookTwo> cashBookOnes = null;
            bw.DoWork += (sender, args) =>
            {
                try
                {
                    ApplicationManager.Instance.ShowBusyInidicator("Loading Data... !");
                    cashBookOnes = _cashBookTwoRepository.Get();
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
                    CashBookTwoList = MapperProfile.iMapper.Map<ObservableCollection<CashBookTwoModel>>(cashBookOnes);
                    InitializeDebiterTypeList();
                    InitializeCrediterTypeList();
                }));
                Reset();
                ApplicationManager.Instance.HideBusyInidicator();
            };

            bw.RunWorkerAsync();
        }

        private void InitializeCollection()
        {
            DebiterCustomerList = MapperProfile.iMapper.Map<ObservableCollection<CustomerModel>>(_customerRepository.Get());
            CrediterCustomerList = MapperProfile.iMapper.Map<ObservableCollection<CustomerModel>>(_customerRepository.Get());
            DebiterSupplierList = MapperProfile.iMapper.Map<ObservableCollection<SupplierModel>>(_supplierRepository.Get());
            CrediterSupplierList = MapperProfile.iMapper.Map<ObservableCollection<SupplierModel>>(_supplierRepository.Get());
            PaymentList = MapperProfile.iMapper.Map<ObservableCollection<PaymentModel>>(_paymentRepository.Get());
        }

        private void InitializeDebiterTypeList()
        {
            DebiterTypeList = new ObservableCollection<CashBookTypeModel>();

            DebiterTypeList.Add(new CashBookTypeModel() { Type = "Select Type" });
            DebiterTypeList.Add(new CashBookTypeModel() { Type = "Customer" });
            DebiterTypeList.Add(new CashBookTypeModel() { Type = "Supplier" });
        }

        private void InitializeCrediterTypeList()
        {
            CrediterTypeList = new ObservableCollection<CashBookTypeModel>();

            CrediterTypeList.Add(new CashBookTypeModel() { Type = "Select Type" });
            CrediterTypeList.Add(new CashBookTypeModel() { Type = "Customer" });
            CrediterTypeList.Add(new CashBookTypeModel() { Type = "Supplier" });
        }

        private void GetDebiterCustomer()
        {
            DebiterCustomer = DebiterCustomerList.FirstOrDefault();
        }

        private void GetCrediterCustomer()
        {
            CrediterCustomer = CrediterCustomerList.FirstOrDefault();
        }

        private void GetPaymentType()
        {
            PaymentType = PaymentList.FirstOrDefault();
        }

        private void GetDebiterSupplier()
        {
            DebiterSupplier = DebiterSupplierList.FirstOrDefault();
        }

        private void GetCrediterSupplier()
        {
            CrediterSupplier = CrediterSupplierList.FirstOrDefault();
        }

        private void GetDebiterType()
        {
            DebiterType = DebiterTypeList.FirstOrDefault();
        }

        private void GetCrediterType()
        {
            CrediterType = CrediterTypeList.FirstOrDefault();
        }

        private void GetDebiterType(CashBookTypeModel cashBookType)
        {
            if (cashBookType.Type != null)
            {
                if (cashBookType.Type == "Customer")
                {
                    GetDebiterCustomer();
                    DebiterSupplier = new SupplierModel();
                    IsDebiterCustomer = Visibility.Visible.ToString();
                    IsDebiterSupplier = Visibility.Collapsed.ToString();
                }
                if (cashBookType.Type == "Supplier")
                {
                    GetDebiterSupplier();
                    DebiterCustomer = new CustomerModel();
                    IsDebiterSupplier = Visibility.Visible.ToString();
                    IsDebiterCustomer = Visibility.Collapsed.ToString();
                }
            }
        }

        private void GetCrediterType(CashBookTypeModel cashBookType)
        {
            if (cashBookType.Type != null)
            {
                if (cashBookType.Type == "Customer")
                {
                    GetCrediterCustomer();
                    CrediterSupplier = new SupplierModel();
                    IsCrediterCustomer = Visibility.Visible.ToString();
                    IsCrediterSupplier = Visibility.Collapsed.ToString();
                }
                if (cashBookType.Type == "Supplier")
                {
                    GetCrediterSupplier();
                    CrediterCustomer = new CustomerModel();
                    IsCrediterSupplier = Visibility.Visible.ToString();
                    IsCrediterCustomer = Visibility.Collapsed.ToString();
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
