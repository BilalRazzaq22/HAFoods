using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Entities.DBModel.CashBook
{
    public class CashBookTwo
    {
        public int Id { get; set; }
        public string DebiterType { get; set; }
        public int? DebiterCustomerId { get; set; }
        public int? DebiterSupplierId { get; set; }
        public string DebiterDescription { get; set; }
        public decimal DebiterAmount { get; set; }
        public string CrediterType { get; set; }
        public int? CrediterCustomerId { get; set; }
        public int? CrediterSupplierId { get; set; }
        public string CrediterDescription { get; set; }
        public DateTime CashBookTwoDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
