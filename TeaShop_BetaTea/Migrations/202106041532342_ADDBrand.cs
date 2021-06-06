namespace TeaShop_BetaTea.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ADDBrand : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BrandModels",
                c => new
                    {
                        BrandId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.BrandId);
            
            AddColumn("dbo.ProductModels", "BrandModel_BrandId", c => c.Int());
            CreateIndex("dbo.ProductModels", "BrandModel_BrandId");
            AddForeignKey("dbo.ProductModels", "BrandModel_BrandId", "dbo.BrandModels", "BrandId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductModels", "BrandModel_BrandId", "dbo.BrandModels");
            DropIndex("dbo.ProductModels", new[] { "BrandModel_BrandId" });
            DropColumn("dbo.ProductModels", "BrandModel_BrandId");
            DropTable("dbo.BrandModels");
        }
    }
}
