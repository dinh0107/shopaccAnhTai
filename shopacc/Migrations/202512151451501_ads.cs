namespace shopacc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ads : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Banners", "Image2", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Banners", "Image2");
        }
    }
}
