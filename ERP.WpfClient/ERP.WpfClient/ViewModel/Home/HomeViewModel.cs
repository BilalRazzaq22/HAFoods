using ERP.Common;
using ERP.Common.NotifyProperty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace ERP.WpfClient.ViewModel.Home
{
    public class HomeViewModel : ViewModelBase, INotifyOnBringIntoView
    {
        #region Fields
        private string _totalCustomers;
        private string _totalSuppliers;
        #endregion

        #region Properties
        public string TotalCustomers
        {
            get { return _totalCustomers; }
            set { _totalCustomers = value; RaisePropertyChanged("TotalCustomers"); }
        }

        public string TotalSuppliers
        {
            get { return _totalSuppliers; }
            set { _totalSuppliers = value; RaisePropertyChanged("TotalSuppliers"); }
        }
        #endregion

        #region Constructor
        public HomeViewModel()
        {
            TotalCustomers = "0";
            TotalSuppliers = "0";
        }
        #endregion

        #region Methods

        public void GetTotalCustomers()
        {

            for (int i = 0; i < 500; i++)
            {
                Thread.Sleep(50);
                Dispatcher.CurrentDispatcher.Invoke(new Action(() => {
                    TotalCustomers = i.ToString();
                }));
            }
        }

        public void GetTotalSuppliers()
        {

            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(100);
                Dispatcher.CurrentDispatcher.Invoke(new Action(() => {
                    TotalSuppliers = i.ToString();
                }));
            }
        }


        public void OnBringIntoView()
        {
            Thread customerThread = new Thread(new ThreadStart(GetTotalCustomers));
            Thread supplierThread = new Thread(new ThreadStart(GetTotalSuppliers));
            customerThread.Start();
            supplierThread.Start();
            //Task.Factory.StartNew(() =>
            //{
            //    GetTotalCustomers();
            //});
            //throw new NotImplementedException();
        }

        #endregion
    }
}
