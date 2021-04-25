using ERP.Common.NotifyProperty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.WpfClient.Model.Customer
{
    public class CustomerOrderModel : ViewModelBase
    {
        private int _id;
        private int? _customerId;
        private decimal _totalAmount;
        private decimal _amountPaid;
        private decimal _remainingAmount;

        public int Id
        {
            get { return _id; }
            set { _id = value; RaisePropertyChanged("Id"); }
        }

        public int? CustomerId
        {
            get { return _customerId; }
            set { _customerId = value; RaisePropertyChanged("CustomerId"); }
        }

        public decimal TotalAmount
        {
            get { return _totalAmount; }
            set { _totalAmount = value; RaisePropertyChanged("TotalAmount"); }
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
    }
}
