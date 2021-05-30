namespace TeaShop_BetaTea.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductSize : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductModels", "Size", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductModels", "Size");
        }
    }
}
