using ERP.Entities.DBModel.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Entities.DBModel.Suppliers
{
    public class SupplierMarketingOrder
    {
        public SupplierMarketingOrder()
        {
            this.SupplierMarketingOrderItems = new HashSet<SupplierMarketingOrderItem>();
        }

        public int Id { get; set; }
        public int? SupplierId { get; set; }
        public string OrderNo { get; set; }
        public int? PaymentId { get; set; }
        public DateTime PurchaseOrderDate { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal AmountPaid { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public virtual ICollection<SupplierMarketingOrderItem> SupplierMarketingOrderItems { get; set; }
    }
}
