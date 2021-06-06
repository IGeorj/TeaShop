namespace TeaShop_BetaTea.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LikeSystem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LikeModels",
                c => new
                    {
                        LikeId = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.LikeId)
                .ForeignKey("dbo.ProductModels", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.ProductId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LikeModels", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.LikeModels", "ProductId", "dbo.ProductModels");
            DropIndex("dbo.LikeModels", new[] { "UserId" });
            DropIndex("dbo.LikeModels", new[] { "ProductId" });
            DropTable("dbo.LikeModels");
        }
    }
}
