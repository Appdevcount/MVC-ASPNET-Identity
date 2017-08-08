namespace MVC_ASPNET_Identity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TableRenameAndPropertyAddition : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Custom.Role",
                c => new
                    {
                        RoleId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "Custom.UserRole",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("Custom.Role", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("Custom.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "Custom.User",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        NewProperty = c.String(),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "Custom.UserClaim",
                c => new
                    {
                        UserClaimId = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.UserClaimId)
                .ForeignKey("Custom.User", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "Custom.UserLogin",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("Custom.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Custom.UserRole", "UserId", "Custom.User");
            DropForeignKey("Custom.UserLogin", "UserId", "Custom.User");
            DropForeignKey("Custom.UserClaim", "UserId", "Custom.User");
            DropForeignKey("Custom.UserRole", "RoleId", "Custom.Role");
            DropIndex("Custom.UserLogin", new[] { "UserId" });
            DropIndex("Custom.UserClaim", new[] { "UserId" });
            DropIndex("Custom.UserRole", new[] { "RoleId" });
            DropIndex("Custom.UserRole", new[] { "UserId" });
            DropTable("Custom.UserLogin");
            DropTable("Custom.UserClaim");
            DropTable("Custom.User");
            DropTable("Custom.UserRole");
            DropTable("Custom.Role");
        }
    }
}
