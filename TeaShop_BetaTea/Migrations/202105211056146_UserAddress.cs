namespace TeaShop_BetaTea.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserAddress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Street", c => c.String());
            AddColumn("dbo.AspNetUsers", "House", c => c.String());
            AddColumn("dbo.AspNetUsers", "Apartament", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Apartament");
            DropColumn("dbo.AspNetUsers", "House");
            DropColumn("dbo.AspNetUsers", "Street");
        }
    }
}
