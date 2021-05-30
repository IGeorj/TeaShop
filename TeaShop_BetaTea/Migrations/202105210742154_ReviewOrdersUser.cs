namespace TeaShop_BetaTea.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReviewOrdersUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductModels", "Brand", c => c.String());
            AddColumn("dbo.ProductModels", "Type", c => c.String());
            AddColumn("dbo.ProductModels", "Color", c => c.String());
            AddColumn("dbo.ProductModels", "Weight", c => c.Int(nullable: false));
            AddColumn("dbo.ProductModels", "Country", c => c.String());
            AddColumn("dbo.ReviewModels", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.ReviewModels", "UserId");
            AddForeignKey("dbo.ReviewModels", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReviewModels", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.ReviewModels", new[] { "UserId" });
            DropColumn("dbo.ReviewModels", "UserId");
            DropColumn("dbo.ProductModels", "Country");
            DropColumn("dbo.ProductModels", "Weight");
            DropColumn("dbo.ProductModels", "Color");
            DropColumn("dbo.ProductModels", "Type");
            DropColumn("dbo.ProductModels", "Brand");
        }
    }
}
