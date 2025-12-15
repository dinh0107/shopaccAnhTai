namespace shopacc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addsome3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ConfigSites", "Name", c => c.String());
            AddColumn("dbo.ConfigSites", "Avatar", c => c.String());
            AddColumn("dbo.ConfigSites", "Profile", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ConfigSites", "Profile");
            DropColumn("dbo.ConfigSites", "Avatar");
            DropColumn("dbo.ConfigSites", "Name");
        }
    }
}
