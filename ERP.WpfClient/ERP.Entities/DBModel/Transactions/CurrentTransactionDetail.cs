using ERP.Entities.DBModel.Stocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Entities.DBModel.Transactions
{
    public class CurrentTransactionDetail
    {
        public int Id { get; set; }
        public int? CurrentTransactionId { get; set; }
        public int? StockId { get; set; }
        public string ItemName { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? Discount { get; set; }
        public decimal? TotalPrice { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public virtual CurrentTransaction CurrentTransaction { get; set; }
        public virtual Stock Stock { get; set; }
    }
}
