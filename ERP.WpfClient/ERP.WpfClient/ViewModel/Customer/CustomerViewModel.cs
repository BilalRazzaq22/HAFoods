using ERP.Common;
using ERP.Common.NotifyProperty;
using ERP.Entities.DbContext;
using ERP.Repository.Generic;
using ERP.WpfClient.Controls.Helpers;
using ERP.WpfClient.Mapper;
using ERP.WpfClient.Model;
using GalaSoft.MvvmLight.Command;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace ERP.WpfClient.ViewModel.Customer
{
    public class CustomerViewModel : ViewModelBase, INotifyOnBringIntoView
    {
        #region Fields

        private readonly IGenericRepository<Entities.DBModel.Customers.Customer> _customerRepository;
        private CustomerModel _customerModel;
        private ObservableCollection<CustomerModel> _customerList;
        private string _customerButton;
        private string _customerParameter;

        #endregion

        #region Ctor

        public CustomerViewModel()
        {
            CustomerCommands = new RelayCommand<object>(ExecuteCustomerCommand);
            DeleteCustomerCommand = new RelayCommand<object>(ExecuteDeleteCustomerCommand);
            //this.CustomerCommands = new CustomerCommand(this);
            _customerRepository = new GenericRepository<Entities.DBModel.Customers.Customer>(new HAFoodDbContext());
            CustomerModel = new CustomerModel();
            CustomerList = new ObservableCollection<CustomerModel>();
            CustomerButton = "Save";
            CustomerParameter = "SaveCustomer";
        }

        #endregion

        #region Properties
        public RelayCommand<object> CustomerCommands { get; set; }
        public RelayCommand<object> DeleteCustomerCommand { get; set; }

        //public CustomerCommand CustomerCommands { get; set; }

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

        public string CustomerParameter
        {
            get { return _customerParameter; }
            set { _customerParameter = value; RaisePropertyChanged("CustomerParameter"); }
        }

        public string CustomerButton
        {
            get { return _customerButton; }
            set { _customerButton = value; RaisePropertyChanged("CustomerButton"); }
        }

        #endregion

        #region Methods

        private void ExecuteCustomerCommand(object str)
        {
            if (str as string == "SaveCustomer")
            {
                SaveCustomer();
            }
            else if (str as string == "UpdateCustomer")
            {
                UpdateCustomer();
            }
            else if (str as string == "Clear")
            {
                Reset();
            }
            else if (str != null)
            {
                EditCustomer(str as CustomerModel);
            }
        }

        private void Reset()
        {
            CustomerModel = new CustomerModel();
            CustomerButton = "Save";
            CustomerParameter = "SaveCustomer";
        }

        private void ExecuteDeleteCustomerCommand(object obj)
        {
            if (obj != null)
            {
                ApplicationManager.Instance.ShowConfirmDialog("Are you sure you want to Delete the customer?", () =>
                {
                    DeleteCustomer(obj as CustomerModel);
                    ApplicationManager.Instance.HideDialog();
                }, () => ApplicationManager.Instance.HideMessageBox(), useYesNo: true);
            }
        }

        public void SaveCustomer()
        {
            if (!String.IsNullOrEmpty(CustomerModel.FirstName))
            {
                var model = _customerRepository.Add(MapperProfile.iMapper.Map<Entities.DBModel.Customers.Customer>(CustomerModel));
                CustomerModel.Id = model.Id;
                CustomerList.Add(CustomerModel);
                Reset();
            }
            else
            {
                ApplicationManager.Instance.ShowMessageBox("Please add First Name");
                return;
            }
        }

        public void EditCustomer(CustomerModel customerModel)
        {
            CustomerButton = "Update";
            CustomerParameter = "UpdateCustomer";
            CustomerModel.Id = customerModel.Id;
            CustomerModel.FirstName = customerModel.FirstName;
            CustomerModel.LastName = customerModel.LastName;
            CustomerModel.FullName = customerModel.FirstName + " " + customerModel.LastName;
            CustomerModel.ContactNo = customerModel.ContactNo;
            CustomerModel.Address = customerModel.Address;
        }

        public void UpdateCustomer()
        {
            if (!String.IsNullOrEmpty(CustomerModel.FirstName))
            {
                _customerRepository.Update(MapperProfile.iMapper.Map<Entities.DBModel.Customers.Customer>(CustomerModel), CustomerModel.Id);
                Reset();
            }
            else
            {
                ApplicationManager.Instance.ShowMessageBox("Please add First Name");
                return;
            }
        }

        public void DeleteCustomer(CustomerModel customerModel)
        {
            _customerRepository.Delete(customerModel.Id);
            CustomerList.Remove(customerModel);
        }

        private void Init()
        {
            var bw = new BackgroundWorker();
            List<Entities.DBModel.Customers.Customer> customers = null;
            bw.DoWork += (sender, args) =>
            {
                try
                {
                    ApplicationManager.Instance.ShowBusyInidicator("Loading Data... !");
                    customers = _customerRepository.Get();
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
                    CustomerList = MapperProfile.iMapper.Map<ObservableCollection<CustomerModel>>(customers);
                }));
                ApplicationManager.Instance.HideBusyInidicator();
            };

            bw.RunWorkerAsync();
        }

        private void LoadCustomerReport()
        {
            var query = (from cust in _customerRepository.Get().ToList()
                         select new Entities.DBModel.Customers.Customer
                         {
                             FirstName = cust.FirstName,
                             LastName = cust.LastName,
                             ContactNo = cust.ContactNo,
                             Address = cust.Address
                         }).ToList();

            if (query.Count > 0)
            {
                string path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

                string fullpath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location).Remove(path.Length - 10) + @"\Reports\Customer\rptCustomer.rdlc";

                string deviceInfo = "";
                string[] streamIds;
                Warning[] warnings;

                string mimeType = string.Empty;
                string encoding = string.Empty;
                string extension = string.Empty;

                ReportViewer reportViewer = new ReportViewer();
                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = fullpath;
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsCustomer", query));
                var bytes = reportViewer.LocalReport.Render("PDF", deviceInfo, out mimeType, out encoding, out extension, out streamIds, out warnings);
                string fileName = @"D:\data.pdf";
                File.WriteAllBytes(fileName, bytes);
                Process.Start(fileName);
                //LocalReport report = new LocalReport();
                //ApplicationManager.Instance.PrintToPrinter(report);


                //ReportViewer reportViewer = new ReportViewer();
                //string path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                //string fullpath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location).Remove(path.Length - 10) + @"\Reports\Customer\rptCustomer.rdlc";
                //reportViewer.LocalReport.ReportPath = fullpath;
                //reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsCustomer", query));
                //reportViewer.RefreshReport();
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
