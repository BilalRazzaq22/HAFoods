using ERP.Common.NotifyProperty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.WpfClient.Model.Supplier
{
    public class SupplierMarketingOrderModel : ViewModelBase
    {
        private int _id;
        private string _orderNo;
        private int? _supplierId;
        private int? _paymentId;
        private DateTime _purchaseOrderDate;
        private int _totalQuantity;
        private decimal _totalPrice;
        private decimal _totalDiscount;
        private decimal _grandTotal;
        private decimal _amountPaid;
        private decimal _remainingAmount;
        private decimal _totalAmount;
        private DateTime? _createdDate;
        private string _createdBy;
        private DateTime? _updatedDate;
        private string _updatedBy;

        public int Id
        {
            get { return _id; }
            set { _id = value; RaisePropertyChanged("Id"); }
        }

        public string OrderNo
        {
            get { return _orderNo; }
            set { _orderNo = value; RaisePropertyChanged("OrderNo"); }
        }

        public int? SupplierId
        {
            get { return _supplierId; }
            set { _supplierId = value; RaisePropertyChanged("SupplierId"); }
        }

        public int? PaymentId
        {
            get { return _paymentId; }
            set { _paymentId = value; RaisePropertyChanged("PaymentId"); }
        }

        public DateTime PurchaseOrderDate
        {
            get { return _purchaseOrderDate; }
            set { _purchaseOrderDate = value; RaisePropertyChanged("PurchaseOrderDate"); }
        }

        public int TotalQuantity
        {
            get { return _totalQuantity; }
            set { _totalQuantity = value; RaisePropertyChanged("TotalQuantity"); }
        }

        public decimal TotalPrice
        {
            get { return _totalPrice; }
            set { _totalPrice = value; RaisePropertyChanged("TotalPrice"); }
        }

        public decimal TotalDiscount
        {
            get { return _totalDiscount; }
            set { _totalDiscount = value; RaisePropertyChanged("TotalDiscount"); }
        }

        public decimal GrandTotal
        {
            get { return _grandTotal; }
            set { _grandTotal = value; RaisePropertyChanged("GrandTotal"); }
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


        public decimal TotalAmount
        {
            get { return _totalAmount; }
            set { _totalAmount = value; RaisePropertyChanged("TotalAmount"); }
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
