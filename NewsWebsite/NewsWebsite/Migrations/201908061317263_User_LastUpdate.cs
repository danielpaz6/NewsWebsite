namespace NewsWebsite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class User_LastUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "StatsLastUpdate", c => c.DateTime(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "StatsLastUpdate");
        }
    }
}
