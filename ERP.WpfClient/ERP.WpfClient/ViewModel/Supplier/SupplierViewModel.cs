using ERP.Common;
using ERP.Common.NotifyProperty;
using ERP.Repository.Generic;
using ERP.WpfClient.Controls.Helpers;
using ERP.WpfClient.Mapper;
using ERP.WpfClient.Model.Supplier;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ERP.WpfClient.ViewModel.Supplier
{
    public class SupplierViewModel : ViewModelBase, INotifyOnBringIntoView
    {
        #region Fields

        private readonly IGenericRepository<Entities.DBModel.Suppliers.Supplier> _supplierRepository;
        private SupplierModel _supplierModel;
        private ObservableCollection<SupplierModel> _supplierList;
        private string _supplierButton;
        private string _supplierParameter;

        #endregion

        #region Ctor

        public SupplierViewModel()
        {
            SupplierCommands = new RelayCommand<object>(ExecuteSupplierCommand);
            DeleteSupplierCommand = new RelayCommand<object>(ExecuteDeleteSupplierCommand);
            //this.SupplierCommands = new CustomerCommand(this);
            _supplierRepository = App.Resolve<IGenericRepository<Entities.DBModel.Suppliers.Supplier>>();
            SupplierModel = new SupplierModel();
            SupplierList = new ObservableCollection<SupplierModel>();
            SupplierButton = "Save";
            SupplierParameter = "SaveSupplier";
        }

        #endregion

        #region Properties
        public RelayCommand<object> SupplierCommands { get; set; }
        public RelayCommand<object> DeleteSupplierCommand { get; set; }

        //public CustomerCommand SupplierCommands { get; set; }

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

        public string SupplierParameter
        {
            get { return _supplierParameter; }
            set { _supplierParameter = value; RaisePropertyChanged("SupplierParameter"); }
        }

        public string SupplierButton
        {
            get { return _supplierButton; }
            set { _supplierButton = value; RaisePropertyChanged("SupplierButton"); }
        }

        #endregion

        #region Methods

        private void ExecuteSupplierCommand(object str)
        {
            if (str as string == "SaveSupplier")
            {
                SaveSupplier();
            }
            else if (str as string == "UpdateSupplier")
            {
                UpdateSupplier();
            }
            else if (str as string == "Clear")
            {
                Reset();
            }
            else if (str != null)
            {
                EditSupplier(str as SupplierModel);
            }
        }

        private void Reset()
        {
            SupplierModel = new SupplierModel();
            SupplierButton = "Save";
            SupplierParameter = "SaveSupplier";
        }

        private void ExecuteDeleteSupplierCommand(object obj)
        {
            if (obj != null)
            {
                ApplicationManager.Instance.ShowConfirmDialog("Are you sure you want to Delete the supplier?", () =>
                {
                    DeleteSupplier(obj as SupplierModel);
                    ApplicationManager.Instance.HideDialog();
                }, () => ApplicationManager.Instance.HideMessageBox(), useYesNo: true);
            }
        }

        public void SaveSupplier()
        {
            if (!String.IsNullOrEmpty(SupplierModel.FirstName))
            {
                var model = _supplierRepository.Add(MapperProfile.iMapper.Map<Entities.DBModel.Suppliers.Supplier>(SupplierModel));
                SupplierModel.Id = model.Id;
                SupplierList.Add(SupplierModel);
                Reset();
            }
            else
            {
                ApplicationManager.Instance.ShowMessageBox("Please add First Name");
                return;
            }
        }

        public void EditSupplier(SupplierModel supplierModel)
        {
            SupplierButton = "Update";
            SupplierParameter = "UpdateSupplier";
            SupplierModel.Id = supplierModel.Id;
            SupplierModel.FirstName = supplierModel.FirstName;
            SupplierModel.LastName = supplierModel.LastName;
            SupplierModel.FullName = supplierModel.FirstName + " " + supplierModel.LastName;
            SupplierModel.ContactNo = supplierModel.ContactNo;
            SupplierModel.Address = supplierModel.Address;
        }

        public void UpdateSupplier()
        {
            if (!String.IsNullOrEmpty(SupplierModel.FirstName))
            {
                _supplierRepository.Update(MapperProfile.iMapper.Map<Entities.DBModel.Suppliers.Supplier>(SupplierModel), SupplierModel.Id);
                Reset();
            }
            else
            {
                ApplicationManager.Instance.ShowMessageBox("Please add First Name");
                return;
            }
        }

        public void DeleteSupplier(SupplierModel supplierModel)
        {
            _supplierRepository.Delete(supplierModel.Id);
            SupplierList.Remove(supplierModel);
        }

        private void Init()
        {
            var bw = new BackgroundWorker();
            List<Entities.DBModel.Suppliers.Supplier> suppliers = null;
            bw.DoWork += (sender, args) =>
            {
                try
                {
                    ApplicationManager.Instance.ShowBusyInidicator("Loading Data... !");
                    suppliers = _supplierRepository.Get();
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
                    SupplierList = MapperProfile.iMapper.Map<ObservableCollection<SupplierModel>>(suppliers);
                }));
                ApplicationManager.Instance.HideBusyInidicator();
            };

            bw.RunWorkerAsync();
        }

        private void LoadCustomerReport()
        {
            var query = (from supp in _supplierRepository.Get().ToList()
                         select new Entities.DBModel.Suppliers.Supplier
                         {
                             FirstName = supp.FirstName,
                             LastName = supp.LastName,
                             ContactNo = supp.ContactNo,
                             Address = supp.Address
                         }).ToList();

            if (query.Count > 0)
            {
                //LocalReport report = new LocalReport();
                //string path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

                //string fullpath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location).Remove(path.Length - 10) + @"\Reports\Customer\rptCustomer.rdlc";
                //report.ReportPath = fullpath;
                //report.DataSources.Add(new ReportDataSource("dsCustomer", query));
                //ApplicationManager.Instance.PrintToPrinter(report);
            }
        }

        public void OnBringIntoView()
        {
            Init();
            //LoadCustomerReport();
        }

        #endregion
    }
}
