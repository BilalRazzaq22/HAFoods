using ERP.Common.NotifyProperty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.WpfClient.Model.Payment
{
    public class PaymentModel : ViewModelBase
    {
        private int _id;
        private string _paymentType;
        private int _currentTransactionId;

        public int Id
        {
            get { return _id; }
            set { _id = value; RaisePropertyChanged("Id"); }
        }

        public int CurrentTransactionId
        {
            get { return _currentTransactionId; }
            set { _currentTransactionId = value; RaisePropertyChanged("CurrentTransactionId"); }
        }

        public string PaymentType
        {
            get { return _paymentType; }
            set { _paymentType = value; RaisePropertyChanged("PaymentType"); }
        }
    }
}
