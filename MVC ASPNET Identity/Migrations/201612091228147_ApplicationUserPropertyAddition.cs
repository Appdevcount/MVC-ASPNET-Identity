namespace MVC_ASPNET_Identity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationUserPropertyAddition : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "NewProperty", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "NewProperty");
        }
    }
}
