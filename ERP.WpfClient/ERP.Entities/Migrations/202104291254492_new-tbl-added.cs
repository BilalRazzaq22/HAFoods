namespace ERP.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newtbladded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CashBookTwoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DebiterType = c.String(),
                        DebiterCustomerId = c.Int(),
                        DebiterSupplierId = c.Int(),
                        DebiterDescription = c.String(),
                        DebiterAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CrediterType = c.String(),
                        CrediterCustomerId = c.Int(),
                        CrediterSupplierId = c.Int(),
                        CrediterDescription = c.String(),
                        CashBookTwoDate = c.DateTime(nullable: false),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CashBookTwoes");
        }
    }
}
