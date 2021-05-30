using ERP.Common.NotifyProperty;
using ERP.Entities.DbContext;
using ERP.Repository.Customer;
using ERP.Repository.Generic;
using ERP.WpfClient.Controls.Helpers;
using ERP.WpfClient.Model.BalanceSheet;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.WpfClient.ViewModel.Popup.Report
{
    public class BalanceSheetViewModel : ViewModelBase
    {
        #region Fields

        private IGenericRepository<Entities.DBModel.Stocks.Stock> _stockRepository;
        private readonly CustomerOrderRepository _customerOrderRepository;
        private decimal _accountReceivable;
        private decimal _cash;
        private decimal _stocks;
        private decimal _accountPayable;
        private decimal _utilityBills;
        private decimal _staffSalaries;
        private decimal _otherExpense;
        private ObservableCollection<BalanceSheetModel> _balanceSheetModel;

        #endregion

        #region Constructor

        public BalanceSheetViewModel()
        {
            ReportActionCommand = new RelayCommand<string>(ExecuteReportActionCommand);
            _stockRepository = new GenericRepository<Entities.DBModel.Stocks.Stock>(new HAFoodDbContext());
            _customerOrderRepository = new CustomerOrderRepository(new HAFoodDbContext());
            GetAssests();
        }

        #endregion

        #region Properties
        public RelayCommand<string> ReportActionCommand { get; set; }

        public decimal OtherExpense
        {
            get { return _otherExpense; }
            set { _otherExpense = value; RaisePropertyChanged("OtherExpense"); }
        }

        public decimal StaffSalaries
        {
            get { return _staffSalaries; }
            set { _staffSalaries = value; RaisePropertyChanged("StaffSalaries"); }
        }

        public decimal UtilityBills
        {
            get { return _utilityBills; }
            set { _utilityBills = value; RaisePropertyChanged("UtilityBills"); }
        }

        public decimal AccountPayable
        {
            get { return _accountPayable; }
            set { _accountPayable = value; RaisePropertyChanged("AccountPayable"); }
        }

        public decimal Stocks
        {
            get { return _stocks; }
            set { _stocks = value; RaisePropertyChanged("Stocks"); }
        }

        public decimal Cash
        {
            get { return _cash; }
            set { _cash = value; RaisePropertyChanged("Cash"); }
        }

        public decimal AccountReceivable
        {
            get { return _accountReceivable; }
            set { _accountReceivable = value; RaisePropertyChanged("AccountReceivable"); }
        }

        public ObservableCollection<BalanceSheetModel> BalanceSheetModel
        {
            get { return _balanceSheetModel; }
            set { _balanceSheetModel = value; RaisePropertyChanged("BalanceSheetModel"); }
        }

        #endregion

        #region Methods

        private decimal GetAssests()
        {
            Stocks = Convert.ToDecimal(_stockRepository.Get().Sum(x => x.Packing * x.Quantity));
            Cash = _customerOrderRepository.Get().Sum(x => x.AmountPaid);
            AccountReceivable = (_customerOrderRepository.Get().LastOrDefault() != null) ? _customerOrderRepository.Get().LastOrDefault().Balance : 0;

            return Stocks + Cash + AccountReceivable;
        }

        private decimal GetLiabilities()
        {
            return AccountPayable + StaffSalaries + UtilityBills + OtherExpense;
        }

        public void GetBalanceSheet()
        {
            BalanceSheetModel = new ObservableCollection<BalanceSheetModel>();
            BalanceSheetModel.Add(new BalanceSheetModel()
            {
                AccountPayable = AccountPayable,
                AccountReceivable = AccountReceivable,
                Cash = Cash,
                NetWorth = GetAssests() - GetLiabilities(),
                OtherExpense = OtherExpense,
                StaffSalaries = StaffSalaries,
                Stocks = Stocks,
                UtilityBills = UtilityBills
            });

            ApplicationManager.Instance.PrintReport(BalanceSheetModel, @"/Reports/rptBalanceSheet", "dsBalanceSheet", "BalanceSheet");
        }

        private void ExecuteReportActionCommand(string command)
        {
            switch (command)
            {
                case "Proceed":
                    GetBalanceSheet();
                    break;
                case "Cancel":
                    ApplicationManager.Instance.HideDialog();
                    break;

            }
        }

        #endregion
    }
}
