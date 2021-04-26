using ERP.Common;
using ERP.Common.NotifyProperty;
using ERP.WpfClient.Controls.Helpers;
using ERP.WpfClient.View.Popups.Reports;
using ERP.WpfClient.View.Popups.Reports.Customer;
using ERP.WpfClient.View.Popups.Reports.DailySale;
using ERP.WpfClient.View.Popups.Reports.Item;
using ERP.WpfClient.View.Popups.Reports.Ledger;
using ERP.WpfClient.View.Popups.Reports.Supplier;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.WpfClient.ViewModel.Popup.Report
{
    public class AllReportsViewModel : ViewModelBase, INotifyOnBringIntoView
    {
        #region Fields

        #endregion


        #region Properties

        public RelayCommand<string> ReportsCommand { get; set; }
        public RelayCommand<string> ReportActionCommand { get; set; }
        #endregion

        #region Constructor
        public AllReportsViewModel()
        {
            ReportsCommand = new RelayCommand<string>(ExecuteReportsCommand);
            ReportActionCommand = new RelayCommand<string>(ExecuteReportActionCommand);
        }
        #endregion


        #region Methods

        private void ExecuteReportActionCommand(string command)
        {
            switch (command)
            {
                case "Proceed":
                    ApplicationManager.Instance.HideDialog();
                    break;

                case "Cancel":
                    ApplicationManager.Instance.HideDialog();
                    break;

            }
        }




        private void ExecuteReportsCommand(string command)
        {
            switch (command)
            {
                case "CustomerReport":
                    ApplicationManager.Instance.ShowDialog("Customers Report", new CustomerReportPopup(this));
                    break;

                case "SupplierReport":
                    ApplicationManager.Instance.ShowDialog("Supplier Report", new SupplierReportPopup(this));
                    break;

                case "DailySaleReport":
                    ApplicationManager.Instance.ShowDialog("Daily Sale Report", new DailySaleReportPopup(this));
                    break;

                case "ItemReport":
                    ApplicationManager.Instance.ShowDialog("Item List Report", new ItemReportPopup(this));
                    break;

                case "LedgerReport":
                    ApplicationManager.Instance.ShowDialog("Ledger Report", new LedgerReportPopup(this));
                    break;

            }
        }
        public void OnBringIntoView()
        {
            // throw new NotImplementedException();
        }
        #endregion

    }
}
