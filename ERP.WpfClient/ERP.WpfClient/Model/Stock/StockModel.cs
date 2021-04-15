using ERP.Common.NotifyProperty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.WpfClient.Model.Stock
{
    public class StockModel : ViewModelBase
    {
        private int _id;
        private string _itemName;
        private string _urduName;
        private decimal _buyPrice;
        private decimal _salePrice;
        private int _quantity;
        private int _newQuantity;
        private string _category;
        private int _packing;
        private string _remarks;

        public int Id
        {
            get { return _id; }
            set { _id = value; RaisePropertyChanged("Id"); }
        }

        public string ItemName
        {
            get { return _itemName; }
            set { _itemName = value; RaisePropertyChanged("ItemName"); }
        }

        public string UrduName
        {
            get { return _urduName; }
            set { _urduName = value; RaisePropertyChanged("UrduName"); }
        }

        public decimal BuyPrice
        {
            get { return _buyPrice; }
            set { _buyPrice = value; RaisePropertyChanged("BuyPrice"); }
        }

        public decimal SalePrice
        {
            get { return _salePrice; }
            set { _salePrice = value; RaisePropertyChanged("SalePrice"); }
        }

        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; RaisePropertyChanged("Quantity"); }
        }

        public int NewQuantity
        {
            get { return _newQuantity; }
            set { _newQuantity = value; RaisePropertyChanged("NewQuantity"); }
        }

        public string Category
        {
            get { return _category; }
            set { _category = value; RaisePropertyChanged("Category"); }
        }

        public int Packing
        {
            get { return _packing; }
            set { _packing = value; RaisePropertyChanged("Packing"); }
        }


        public string Remarks
        {
            get { return _remarks; }
            set { _remarks = value; RaisePropertyChanged("Remarks"); }
        }
    }
}
