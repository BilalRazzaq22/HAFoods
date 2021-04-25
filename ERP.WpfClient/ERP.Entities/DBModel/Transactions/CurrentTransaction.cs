﻿using ERP.Entities.DBModel.Customers;
using ERP.Entities.DBModel.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Entities.DBModel.Transactions
{
    public class CurrentTransaction
    {
        public CurrentTransaction()
        {
            this.CurrentTransactionDetails = new HashSet<CurrentTransactionDetail>();
        }

        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public int? PaymentId { get; set; }
        public string OrderNo { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal? AmountPaid { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Payment Payment { get; set; }
        public virtual ICollection<CurrentTransactionDetail> CurrentTransactionDetails { get; set; }
    }
}
