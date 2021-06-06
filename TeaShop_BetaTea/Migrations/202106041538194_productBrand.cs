namespace TeaShop_BetaTea.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class productBrand : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProductModels", "BrandModel_BrandId", "dbo.BrandModels");
            DropIndex("dbo.ProductModels", new[] { "BrandModel_BrandId" });
            RenameColumn(table: "dbo.ProductModels", name: "BrandModel_BrandId", newName: "BrandId");
            AlterColumn("dbo.ProductModels", "BrandId", c => c.Int(nullable: false));
            CreateIndex("dbo.ProductModels", "BrandId");
            AddForeignKey("dbo.ProductModels", "BrandId", "dbo.BrandModels", "BrandId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductModels", "BrandId", "dbo.BrandModels");
            DropIndex("dbo.ProductModels", new[] { "BrandId" });
            AlterColumn("dbo.ProductModels", "BrandId", c => c.Int());
            RenameColumn(table: "dbo.ProductModels", name: "BrandId", newName: "BrandModel_BrandId");
            CreateIndex("dbo.ProductModels", "BrandModel_BrandId");
            AddForeignKey("dbo.ProductModels", "BrandModel_BrandId", "dbo.BrandModels", "BrandId");
        }
    }
}
