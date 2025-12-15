namespace shopacc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inittable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tables",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Price = c.String(nullable: false),
                        Lan1 = c.String(),
                        Lan2 = c.String(),
                        Lan3 = c.String(),
                        Hot = c.Boolean(nullable: false),
                        Sort = c.Int(nullable: false),
                        TypeTable = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tables");
        }
    }
}
