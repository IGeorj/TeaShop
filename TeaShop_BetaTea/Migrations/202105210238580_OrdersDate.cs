namespace TeaShop_BetaTea.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrdersDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderModels", "Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderModels", "Date");
        }
    }
}
