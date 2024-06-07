namespace Local_Guide_App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLocation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Locations", "UserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Locations", "LocationName", c => c.String(nullable: false));
            CreateIndex("dbo.Locations", "UserId");
            AddForeignKey("dbo.Locations", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Locations", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Locations", new[] { "UserId" });
            AlterColumn("dbo.Locations", "LocationName", c => c.String());
            DropColumn("dbo.Locations", "UserId");
        }
    }
}
