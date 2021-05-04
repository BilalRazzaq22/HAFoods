using ERP.Common.NotifyProperty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.WpfClient.Model.Supplier
{
    public class SupplierOrderModel : ViewModelBase
    {
        private int _id;
        private int? _supplierId;
        private decimal _totalAmount;
        private decimal _amountPaid;
        private decimal _remainingAmount;

        public int Id
        {
            get { return _id; }
            set { _id = value; RaisePropertyChanged("Id"); }
        }

        public int? SupplierId
        {
            get { return _supplierId; }
            set { _supplierId = value; RaisePropertyChanged("SupplierId"); }
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
