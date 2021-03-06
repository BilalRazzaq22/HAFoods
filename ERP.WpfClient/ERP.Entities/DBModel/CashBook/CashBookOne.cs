using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Entities.DBModel.CashBook
{
    public class CashBookOne
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int? SupplierId { get; set; }
        public int? CustomerId { get; set; }
        public decimal Amount { get; set; }
        public int? PaymentId { get; set; }
        public string Description { get; set; }
        public DateTime CashBookOneDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
