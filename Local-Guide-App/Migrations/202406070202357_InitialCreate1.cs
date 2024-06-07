namespace Local_Guide_App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        LocationId = c.Int(nullable: false, identity: true),
                        LocationName = c.String(),
                        LocationDescription = c.String(),
                        Category = c.String(),
                        Address = c.String(),
                        Ratings = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.LocationId);
            
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        ReviewId = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        Rating = c.Int(nullable: false),
                        LocationId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ReviewId)
                .ForeignKey("dbo.Locations", t => t.LocationId, cascadeDelete: true)
                .Index(t => t.LocationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reviews", "LocationId", "dbo.Locations");
            DropIndex("dbo.Reviews", new[] { "LocationId" });
            DropTable("dbo.Reviews");
            DropTable("dbo.Locations");
        }
    }
}
