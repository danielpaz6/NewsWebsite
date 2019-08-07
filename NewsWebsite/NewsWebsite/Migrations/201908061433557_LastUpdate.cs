namespace NewsWebsite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LastUpdate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "StatsLastUpdate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "StatsLastUpdate", c => c.DateTime(nullable: false));
        }
    }
}
