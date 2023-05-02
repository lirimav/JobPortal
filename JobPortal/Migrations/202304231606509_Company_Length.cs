namespace JobPortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Company_Length : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Jobs", "Company", c => c.String(nullable: false, maxLength: 40));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Jobs", "Company", c => c.String(nullable: false, maxLength: 20));
        }
    }
}
