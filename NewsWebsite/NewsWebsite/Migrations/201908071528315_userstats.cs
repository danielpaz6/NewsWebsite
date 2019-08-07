namespace NewsWebsite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userstats : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DistributionByCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CategoryId = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.CategoryId)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DistributionByCategories", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.DistributionByCategories", "CategoryId", "dbo.Categories");
            DropIndex("dbo.DistributionByCategories", new[] { "User_Id" });
            DropIndex("dbo.DistributionByCategories", new[] { "CategoryId" });
            DropTable("dbo.DistributionByCategories");
        }
    }
}
