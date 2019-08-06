namespace NewsWebsite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ArticleSource : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "Source", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Articles", "Source");
        }
    }
}
