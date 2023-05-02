namespace JobPortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Jobs_Location_Dates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Jobs", "Location", c => c.String(nullable: false));
            AddColumn("dbo.Jobs", "ExpirationDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Jobs", "ExpirationDate");
            DropColumn("dbo.Jobs", "Location");
        }
    }
}
