namespace ERP.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updtbl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CashBookOnes", "CreatedDate", c => c.DateTime());
            AddColumn("dbo.CashBookOnes", "CreatedBy", c => c.String());
            AddColumn("dbo.CashBookOnes", "UpdatedDate", c => c.DateTime());
            AddColumn("dbo.CashBookOnes", "UpdatedBy", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CashBookOnes", "UpdatedBy");
            DropColumn("dbo.CashBookOnes", "UpdatedDate");
            DropColumn("dbo.CashBookOnes", "CreatedBy");
            DropColumn("dbo.CashBookOnes", "CreatedDate");
        }
    }
}
