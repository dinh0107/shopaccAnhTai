namespace shopacc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "TypeAcc", c => c.Int(nullable: false));
            DropColumn("dbo.Products", "DaBan");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "DaBan", c => c.Boolean(nullable: false));
            DropColumn("dbo.Products", "TypeAcc");
        }
    }
}
