namespace TeaShop_BetaTea.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderModels", "Status", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderModels", "Status");
        }
    }
}
