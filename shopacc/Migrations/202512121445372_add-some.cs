namespace shopacc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addsome : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ConfigSites", "Zalo", c => c.String(maxLength: 500));
            AddColumn("dbo.ConfigSites", "Body", c => c.String());
            AddColumn("dbo.ConfigSites", "Footer", c => c.String());
            AddColumn("dbo.Products", "DaBan", c => c.Boolean(nullable: false));
            AddColumn("dbo.Products", "ChoThue", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Products", "Name", c => c.String());
            DropColumn("dbo.ConfigSites", "Place");
            DropColumn("dbo.Products", "CoverImage");
            DropColumn("dbo.Products", "Home");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Home", c => c.Boolean(nullable: false));
            AddColumn("dbo.Products", "CoverImage", c => c.String(maxLength: 500));
            AddColumn("dbo.ConfigSites", "Place", c => c.String());
            AlterColumn("dbo.Products", "Name", c => c.String(nullable: false, maxLength: 200));
            DropColumn("dbo.Products", "ChoThue");
            DropColumn("dbo.Products", "DaBan");
            DropColumn("dbo.ConfigSites", "Footer");
            DropColumn("dbo.ConfigSites", "Body");
            DropColumn("dbo.ConfigSites", "Zalo");
        }
    }
}
