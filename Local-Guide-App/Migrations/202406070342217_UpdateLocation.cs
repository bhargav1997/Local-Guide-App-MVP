namespace Local_Guide_App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateLocation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Locations", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Locations", new[] { "UserId" });
            DropColumn("dbo.Locations", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Locations", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Locations", "UserId");
            AddForeignKey("dbo.Locations", "UserId", "dbo.AspNetUsers", "Id");
        }
    }
}
