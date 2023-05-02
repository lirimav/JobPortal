namespace JobPortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class jobApplications_model : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JobApplications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClientId = c.String(),
                        JobsId = c.Int(nullable: false),
                        AppliedOn = c.DateTime(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Jobs", t => t.JobsId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.JobsId)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobApplications", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.JobApplications", "JobsId", "dbo.Jobs");
            DropIndex("dbo.JobApplications", new[] { "User_Id" });
            DropIndex("dbo.JobApplications", new[] { "JobsId" });
            DropTable("dbo.JobApplications");
        }
    }
}
