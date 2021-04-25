using ERP.Entities.DBModel.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Entities.DBModel.Stocks
{
    public class Stock
    {
        public Stock()
        {
            this.CurrentTransactionDetails = new HashSet<CurrentTransactionDetail>();
        }

        public int Id { get; set; }
        public string ItemName { get; set; }
        public string UrduName { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal SalePrice { get; set; }
        public int? Quantity { get; set; }
        public string Category { get; set; }
        public int? Packing { get; set; }
        public string Remarks { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public virtual ICollection<CurrentTransactionDetail> CurrentTransactionDetails { get; set; }
    }
}
