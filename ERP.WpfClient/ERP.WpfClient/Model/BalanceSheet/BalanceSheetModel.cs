using ERP.Common.NotifyProperty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.WpfClient.Model.BalanceSheet
{
    public class BalanceSheetModel : ViewModelBase
    {
        private decimal _accountReceivable;
        private decimal _cash;
        private decimal _stocks;
        private decimal _accountPayable;
        private decimal _utilityBills;
        private decimal _staffSalaries;
        private decimal _otherExpense;
        private decimal _netWorth;

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

        public decimal NetWorth
        {
            get { return _netWorth; }
            set { _netWorth = value; RaisePropertyChanged("NetWorth"); }
        }
    }
}
