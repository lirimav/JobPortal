namespace JobPortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTwoColumns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "UserId", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "InsertedOn", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "InsertedOn");
            DropColumn("dbo.AspNetUsers", "UserId");
        }
    }
}
