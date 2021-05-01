﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Entities.DBModel.PurchaseOrders
{
    public class PurchaseOrderItem
    {
        public int Id { get; set; }
        public int? PurchaseOrderId { get; set; }
        public int? StockId { get; set; }
        public string ItemName { get; set; }
        public int PurchaseQuantity { get; set; }
        public decimal BuyPrice { get; set; }
        public int Packing { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
