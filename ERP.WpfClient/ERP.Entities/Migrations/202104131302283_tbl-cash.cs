namespace ERP.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tblcash : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.cashbooks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.cashbooks");
        }
    }
}
