namespace ERP.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rmvtbl : DbMigration
    {
        public override void Up()
        {
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
            
            DropTable("dbo.cashbooks");
            DropTable("dbo.tests");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.tests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.cashbooks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.CashBookOnes");
        }
    }
}
