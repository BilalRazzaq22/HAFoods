using ERP.Common.NotifyProperty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.WpfClient.Model.CashBooks
{
    public class CashBookOneModel : ViewModelBase
    {
        private int _id;
        private string _type;
        private int? _customerId;
        private int? _supplierId;
        private decimal? _amount;
        private int? _paymentId;
        private string _description;
        private DateTime? _createdDate;
        private string _createdBy;
        private DateTime? _updatedDate;
        private string _updatedBy;

        public int Id
        {
            get { return _id; }
            set { _id = value; RaisePropertyChanged("Id"); }
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; RaisePropertyChanged("Type"); }
        }

        public int? CustomerId
        {
            get { return _customerId; }
            set { _customerId = value; RaisePropertyChanged("CustomerId"); }
        }

        public int? SupplierId
        {
            get { return _supplierId; }
            set { _supplierId = value; RaisePropertyChanged("SupplierId"); }
        }

        public decimal? Amount
        {
            get { return _amount; }
            set { _amount = value; RaisePropertyChanged("Amount"); }
        }

        public int? PaymentId
        {
            get { return _paymentId; }
            set { _paymentId = value; RaisePropertyChanged("PaymentId"); }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; RaisePropertyChanged("Description"); }
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
