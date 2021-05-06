using ERP.Common.NotifyProperty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.WpfClient.Model.Transaction
{
    public class RecentTransactionModel : ViewModelBase
    {
        private string _orderNo;

        public string OrderNo
        {
            get { return _orderNo; }
            set { _orderNo = value; }
        }

        private string _customerName;

        public string CustomerName
        {
            get { return _customerName; }
            set { _customerName = value; }
        }

        private string _grandTotal;

        public string GrandTotal
        {
            get { return _grandTotal; }
            set { _grandTotal = value; }
        }

        private string _paymentType;

        public string PaymentType
        {
            get { return _paymentType; }
            set { _paymentType = value; }
        }

    }
}
