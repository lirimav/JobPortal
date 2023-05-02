namespace JobPortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Categories_Professions_FK_Model : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Jobs", "CategoriesId", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "CategoriesId", c => c.Int(nullable: false));
            AddColumn("dbo.Professions", "CategoriesId", c => c.Int(nullable: false));
            CreateIndex("dbo.Jobs", "CategoriesId");
            CreateIndex("dbo.AspNetUsers", "CategoriesId");
            CreateIndex("dbo.Professions", "CategoriesId");
            AddForeignKey("dbo.Jobs", "CategoriesId", "dbo.Categories", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUsers", "CategoriesId", "dbo.Categories", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Professions", "CategoriesId", "dbo.Categories", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Professions", "CategoriesId", "dbo.Categories");
            DropForeignKey("dbo.AspNetUsers", "CategoriesId", "dbo.Categories");
            DropForeignKey("dbo.Jobs", "CategoriesId", "dbo.Categories");
            DropIndex("dbo.Professions", new[] { "CategoriesId" });
            DropIndex("dbo.AspNetUsers", new[] { "CategoriesId" });
            DropIndex("dbo.Jobs", new[] { "CategoriesId" });
            DropColumn("dbo.Professions", "CategoriesId");
            DropColumn("dbo.AspNetUsers", "CategoriesId");
            DropColumn("dbo.Jobs", "CategoriesId");
        }
    }
}
