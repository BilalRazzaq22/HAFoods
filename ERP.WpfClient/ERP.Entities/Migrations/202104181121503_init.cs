namespace ERP.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AppVersion = c.String(),
                        IsDBCreated = c.Boolean(nullable: false),
                        AppStartDate = c.DateTime(),
                        AppEndDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CashBookOnes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        SupplierId = c.Int(),
                        CustomerId = c.Int(),
                        Amount = c.Decimal(precision: 18, scale: 2),
                        PaymentId = c.Int(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CurrentTransactionDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CurrentTransactionId = c.Int(),
                        StockId = c.Int(),
                        ItemName = c.String(),
                        Quantity = c.Int(),
                        Price = c.Decimal(precision: 18, scale: 2),
                        Discount = c.Decimal(precision: 18, scale: 2),
                        TotalPrice = c.Decimal(precision: 18, scale: 2),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CurrentTransactions", t => t.CurrentTransactionId)
                .ForeignKey("dbo.Stocks", t => t.StockId)
                .Index(t => t.CurrentTransactionId)
                .Index(t => t.StockId);
            
            CreateTable(
                "dbo.CurrentTransactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(),
                        PaymentId = c.Int(),
                        OrderNo = c.String(),
                        TotalPrice = c.Decimal(precision: 18, scale: 2),
                        TotalDiscount = c.Decimal(precision: 18, scale: 2),
                        GrandTotal = c.Decimal(precision: 18, scale: 2),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .ForeignKey("dbo.Payments", t => t.PaymentId)
                .Index(t => t.CustomerId)
                .Index(t => t.PaymentId);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        ContactNo = c.String(),
                        Address = c.String(),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PaymentType = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Stocks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemName = c.String(),
                        UrduName = c.String(),
                        BuyPrice = c.Decimal(precision: 18, scale: 2),
                        SalePrice = c.Decimal(precision: 18, scale: 2),
                        Quantity = c.Int(),
                        Category = c.String(),
                        Packing = c.Int(),
                        Remarks = c.String(),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Suppliers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        ContactNo = c.String(),
                        Address = c.String(),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                        UserGroup = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CurrentTransactionDetails", "StockId", "dbo.Stocks");
            DropForeignKey("dbo.CurrentTransactions", "PaymentId", "dbo.Payments");
            DropForeignKey("dbo.CurrentTransactions", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.CurrentTransactionDetails", "CurrentTransactionId", "dbo.CurrentTransactions");
            DropIndex("dbo.CurrentTransactionDetails", new[] { "StockId" });
            DropIndex("dbo.CurrentTransactions", new[] { "PaymentId" });
            DropIndex("dbo.CurrentTransactions", new[] { "CustomerId" });
            DropIndex("dbo.CurrentTransactionDetails", new[] { "CurrentTransactionId" });
            DropTable("dbo.Users");
            DropTable("dbo.Suppliers");
            DropTable("dbo.Stocks");
            DropTable("dbo.Payments");
            DropTable("dbo.Customers");
            DropTable("dbo.CurrentTransactions");
            DropTable("dbo.CurrentTransactionDetails");
            DropTable("dbo.CashBookOnes");
            DropTable("dbo.AppSettings");
        }
    }
}
