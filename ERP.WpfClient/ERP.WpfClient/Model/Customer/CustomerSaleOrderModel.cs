using ERP.Common.NotifyProperty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.WpfClient.Model.Customer
{
    public class CustomerSaleOrderModel : ViewModelBase
    {
        private string _orderNo;
        private string _customerName;
        private string _itemName;
        private int _quantity;
        private decimal _price;
        private int _packing;
        private decimal _perCarton;
        private decimal _discount;
        private decimal _totalPrice;
        private decimal _grandTotalPrice;
        private decimal _totalDiscount;
        private decimal _grandTotal;
        private decimal _totalAmount;
        private decimal _amountPaid;
        private decimal _remainingAmount;
        private decimal _previousBalance;
        private DateTime _transactionDate;

        public string OrderNo
        {
            get { return _orderNo; }
            set { _orderNo = value; RaisePropertyChanged("OrderNo"); }
        }

        public string CustomerName
        {
            get { return _customerName; }
            set { _customerName = value; RaisePropertyChanged("CustomerName"); }
        }

        public string ItemName
        {
            get { return _itemName; }
            set { _itemName = value; RaisePropertyChanged("ItemName"); }
        }

        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; RaisePropertyChanged("Quantity"); }
        }

        public decimal Price
        {
            get { return _price; }
            set { _price = value; RaisePropertyChanged("Price"); }
        }

        public int Packing
        {
            get { return _packing; }
            set { _packing = value; RaisePropertyChanged("Packing"); }
        }

        public decimal PerCarton
        {
            get { return _perCarton; }
            set { _perCarton = value; RaisePropertyChanged("PerCarton"); }
        }

        public decimal Discount
        {
            get { return _discount; }
            set { _discount = value; RaisePropertyChanged("Discount"); }
        }

        public decimal TotalPrice
        {
            get { return _totalPrice; }
            set { _totalPrice = value; RaisePropertyChanged("TotalPrice"); }
        }

        public decimal GrandTotalPrice
        {
            get { return _grandTotalPrice; }
            set { _grandTotalPrice = value; RaisePropertyChanged("GrandTotalPrice"); }
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

        public decimal PreviousBalance
        {
            get { return _previousBalance; }
            set { _previousBalance = value; RaisePropertyChanged("PreviousBalance"); }
        }

        public DateTime TransactionDate
        {
            get { return _transactionDate; }
            set { _transactionDate = value; RaisePropertyChanged("TransactionDate"); }
        }
    }
}
