using ERP.Common.NotifyProperty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.WpfClient.Model.CashBooks
{
    public class CashBookTwoModel: ViewModelBase
    {
        private int _id;
        private string _debiterType;
        private int? _debiterCustomerId;
        private int? _debiterSupplierId;
        private string _debiterDescription;
        private decimal _debiterAmount;
        private string _crediterType;
        private int? _crediterCustomerId;
        private int? _crediterSupplierId;
        private string _crediterDescription;
        private DateTime _cashBookTwoDate;
        private DateTime? _createdDate;
        private string _createdBy;
        private DateTime? _updatedDate;
        private string _updatedBy;

        public int Id
        {
            get { return _id; }
            set { _id = value; RaisePropertyChanged("Id"); }
        }

        public string DebiterType
        {
            get { return _debiterType; }
            set { _debiterType = value; RaisePropertyChanged("DebiterType"); }
        }

        public int? DebiterCustomerId
        {
            get { return _debiterCustomerId; }
            set { _debiterCustomerId = value; RaisePropertyChanged("DebiterCustomerId"); }
        }

        public int? DebiterSupplierId
        {
            get { return _debiterSupplierId; }
            set { _debiterSupplierId = value; RaisePropertyChanged("DebiterSupplierId"); }
        }

        public string DebiterDescription
        {
            get { return _debiterDescription; }
            set { _debiterDescription = value; RaisePropertyChanged("DebiterDescription"); }
        }

        public decimal DebiterAmount
        {
            get { return _debiterAmount; }
            set { _debiterAmount = value; RaisePropertyChanged("DebiterAmount"); }
        }

        public string CrediterType
        {
            get { return _crediterType; }
            set { _crediterType = value; RaisePropertyChanged("CrediterType"); }
        }

        public int? CrediterCustomerId
        {
            get { return _crediterCustomerId; }
            set { _crediterCustomerId = value; RaisePropertyChanged("CrediterCustomerId"); }
        }

        public int? CrediterSupplierId
        {
            get { return _crediterSupplierId; }
            set { _crediterSupplierId = value; RaisePropertyChanged("CrediterSupplierId"); }
        }

        public string CrediterDescription
        {
            get { return _crediterDescription; }
            set { _crediterDescription = value; RaisePropertyChanged("CrediterDescription"); }
        }

        public DateTime CashBookTwoDate
        {
            get { return _cashBookTwoDate; }
            set { _cashBookTwoDate = value; RaisePropertyChanged("CashBookTwoDate"); }
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
