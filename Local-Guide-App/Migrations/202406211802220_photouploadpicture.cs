namespace Local_Guide_App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class photouploadpicture : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Locations", "LocationHasPic", c => c.Boolean(nullable: false));
            AddColumn("dbo.Locations", "PicExtension", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Locations", "PicExtension");
            DropColumn("dbo.Locations", "LocationHasPic");
        }
    }
}
