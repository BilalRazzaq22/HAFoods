using ERP.Common.NotifyProperty;
using System;

namespace ERP.WpfClient.Model.Transaction
{
    public class CurrentTransactionModel : ViewModelBase
    {
        private int _id;
        private int _customerId;
        private string _orderNo;
        private decimal? _totalPrice;
        private decimal? _totalDiscount;
        private decimal? _grandTotal;
        private DateTime? _createdDate;
        private string _createdBy;
        private DateTime? _updatedDate;
        private string _updatedBy;

        public int Id
        {
            get { return _id; }
            set { _id = value; RaisePropertyChanged("Id"); }
        }

        public int CustomerId
        {
            get { return _customerId; }
            set { _customerId = value; RaisePropertyChanged("CustomerId"); }
        }

        public string OrderNo
        {
            get { return _orderNo; }
            set { _orderNo = value; RaisePropertyChanged("OrderNo"); }
        }

        public decimal? TotalPrice
        {
            get { return _totalPrice; }
            set { _totalPrice = value; RaisePropertyChanged("TotalPrice"); }
        }

        public decimal? TotalDiscount
        {
            get { return _totalDiscount; }
            set { _totalDiscount = value; RaisePropertyChanged("TotalDiscount"); }
        }

        public decimal? GrandTotal
        {
            get { return _grandTotal; }
            set { _grandTotal = value; RaisePropertyChanged("GrandTotal"); }
        }

        public DateTime? CreatedDate
        {
            get { return _createdDate; }
            set { _createdDate = DateTime.Now; RaisePropertyChanged("CreatedDate"); }
        }

        public string CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = value; RaisePropertyChanged("CreatedBy"); }
        }

        public DateTime? UpdatedDate
        {
            get { return _updatedDate; }
            set { _updatedDate = value; RaisePropertyChanged("UpdatedDate"); }
        }

        public string UpdatedBy
        {
            get { return _updatedBy; }
            set { _updatedBy = value; RaisePropertyChanged("UpdatedBy"); }
        }
    }
}
