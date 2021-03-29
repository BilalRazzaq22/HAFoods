﻿using ERP.Common;
using ERP.Common.NotifyProperty;
using ERP.Repository.Customer;
using ERP.WpfClient.Commands;
using ERP.WpfClient.Mapper;
using ERP.WpfClient.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace ERP.WpfClient.ViewModel.Customer
{
    public class CustomerViewModel : ViewModelBase, INotifyOnBringIntoView
    {
        #region Fields

        private readonly ICustomerRepository _customerRepository;
        private CustomerModel _customerModel;
        private ObservableCollection<CustomerModel> _customerList;
        private string _customerButton;
        private string _customerParameter;

        #endregion

        #region Ctor

        public CustomerViewModel()
        {
            this.CustomerCommands = new CustomerCommand(this);
            _customerRepository = App.Resolve<ICustomerRepository>();
            CustomerModel = new CustomerModel();
            CustomerList = new ObservableCollection<CustomerModel>();
            CustomerButton = "Save";
            CustomerParameter = "SaveCustomer";
        }

        #endregion

        #region Properties
        public CustomerCommand CustomerCommands { get; set; }

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

        public void SaveCustomer()
        {
            _customerRepository.SaveCustomer(MapperProfile.iMapper.Map<Entities.DBModel.Customer>(CustomerModel));
            CustomerList.Add(CustomerModel);
            CustomerModel = new CustomerModel();
        }

        public void EditCustomer(CustomerModel customerModel)
        {
            CustomerButton = "Update";
            CustomerParameter = "UpdateCustomer";
            CustomerModel.Id = customerModel.Id;
            CustomerModel.FirstName = customerModel.FirstName;
            CustomerModel.LastName = customerModel.LastName;
            CustomerModel.ContactNo = customerModel.ContactNo;
            CustomerModel.Address = customerModel.Address;
        }

        public void UpdateCustomer()
        {
            _customerRepository.UpdateCustomer(MapperProfile.iMapper.Map<Entities.DBModel.Customer>(CustomerModel));
        }

        private void Init()
        {
            var bw = new BackgroundWorker();
            List<Entities.DBModel.Customer> customers = null;
            bw.DoWork += (sender, args) =>
            {
                try
                {
                    customers = _customerRepository.Get();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Post Error\nMessage: " + ex.Message, "Friday Retail");
                }
            };

            bw.RunWorkerCompleted += async (sender, args) =>
            {
                await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    CustomerList = MapperProfile.iMapper.Map<ObservableCollection<CustomerModel>>(customers);
                }));
            };

            bw.RunWorkerAsync();
        }

        public void OnBringIntoView()
        {
            Init();
        }

        #endregion
    }
}