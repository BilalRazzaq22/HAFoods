using ERP.Common.NotifyProperty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.WpfClient.Model.Customer
{
    public class CustomerMarketingOrderItemModel : ViewModelBase
    {
        private int _id;
        private int _customerMarketingOrderId;
        private int? _stockId;
        private string _itemName;
        private int _packing;
        private int _newQuantity;
        private int _quantity;
        private decimal _price;
        private decimal _discount;
        private decimal _newDiscount;
        private decimal _totalPrice;
        private DateTime? _createdDate;
        private string _createdBy;
        private DateTime? _updatedDate;
        private string _updatedBy;

        public int Id
        {
            get { return _id; }
            set { _id = value; RaisePropertyChanged("Id"); }
        }

        public int CustomerMarketingOrderId
        {
            get { return _customerMarketingOrderId; }
            set { _customerMarketingOrderId = value; RaisePropertyChanged("CustomerMarketingOrderId"); }
        }

        public int? StockId
        {
            get { return _stockId; }
            set { _stockId = value; RaisePropertyChanged("StockId"); }
        }

        public string ItemName
        {
            get { return _itemName; }
            set { _itemName = value; RaisePropertyChanged("ItemName"); }
        }

        public int Packing
        {
            get { return _packing; }
            set { _packing = value; RaisePropertyChanged("Packing"); }
        }

        public int NewQuantity
        {
            get { return _newQuantity; }
            set { _newQuantity = value; RaisePropertyChanged("NewQuantity"); }
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

        public decimal Discount
        {
            get { return _discount; }
            set { _discount = value; RaisePropertyChanged("Discount"); }
        }

        public decimal NewDiscount
        {
            get { return _newDiscount; }
            set { _newDiscount = value; RaisePropertyChanged("NewDiscount"); }
        }

        public decimal TotalPrice
        {
            get { return _totalPrice; }
            set { _totalPrice = value; RaisePropertyChanged("TotalPrice"); }
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
