//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ERP.Entities.DBModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class Stock
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string UrduName { get; set; }
        public Nullable<decimal> BuyPrice { get; set; }
        public Nullable<decimal> SalePrice { get; set; }
        public Nullable<int> Quantity { get; set; }
        public string Category { get; set; }
        public Nullable<int> Packing { get; set; }
        public string Remarks { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}