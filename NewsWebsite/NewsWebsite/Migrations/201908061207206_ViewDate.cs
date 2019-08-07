namespace NewsWebsite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ViewDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Views", "WatchDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Views", "WatchDate");
        }
    }
}
