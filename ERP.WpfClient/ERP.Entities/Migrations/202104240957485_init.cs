namespace ERP.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CashBookOnes", "Amount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.CurrentTransactionDetails", "Quantity", c => c.Int(nullable: false));
            AlterColumn("dbo.CurrentTransactionDetails", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.CurrentTransactionDetails", "Discount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.CurrentTransactionDetails", "TotalPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.CurrentTransactions", "TotalPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.CurrentTransactions", "TotalDiscount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.CurrentTransactions", "GrandTotal", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Stocks", "BuyPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Stocks", "SalePrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.CustomerOrders", "TotalAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.CustomerOrders", "AmountPaid", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.CustomerOrders", "RemainingAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CustomerOrders", "RemainingAmount", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.CustomerOrders", "AmountPaid", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.CustomerOrders", "TotalAmount", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Stocks", "SalePrice", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Stocks", "BuyPrice", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.CurrentTransactions", "GrandTotal", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.CurrentTransactions", "TotalDiscount", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.CurrentTransactions", "TotalPrice", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.CurrentTransactionDetails", "TotalPrice", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.CurrentTransactionDetails", "Discount", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.CurrentTransactionDetails", "Price", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.CurrentTransactionDetails", "Quantity", c => c.Int());
            AlterColumn("dbo.CashBookOnes", "Amount", c => c.Decimal(precision: 18, scale: 2));
        }
    }
}
