namespace MVC_ASPNET_Identity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class TableRenameandPropertyAddition : DbMigration
    {
        public override void Up()
        {

            //Refer earlier migrations[currently excluded from the project] for lessons learnt
            //Below commentin Bracket is completely applicable for that [Changing DB Object name and adding properties after the Db objects are created with default setup]
            //{
            //During table renaming there is some known / possible issue in prepared Configuration file after 
            //add - migration "" command.so it
            //needs some manual change as required ,then need to run update - database - verbose / script command -
            //Reference http://stackoverflow.com/questions/30114751/renaming-identity-tables-with-ef6-migrations-failing
            //The DropForeignKey() method was below RenameTable() method but refrring old table for Dropping Foreignkey

            //Now the Table USERROLES structure completely changed because of this - PK - FK Combination 2 column split to 4 columns of same menaing
            //Also all tables have separate foreign key referenced and created
            //Need to check on possibility of fixing the issue in migration file
            ////***Marking the possible changes in Migration Configuration  file
            //}

            
//Error Number:1919,State:1,Class:16
//Column 'UserId' in table 'dbo.UserClaims' is of a type that is invalid for use as a key column in an index.

            CreateTable(
                "dbo.Roles",
                c => new
                {
                    RoleId = c.String(nullable: false, maxLength: 128),
                    Name = c.String(),
                })
                .PrimaryKey(t => t.RoleId);

            CreateTable(
                "dbo.UserRoles",
                c => new
                {
                    UserId = c.String(nullable: false, maxLength: 128),
                    RoleId = c.String(nullable: false, maxLength: 128)//,

                    //Commented additional PK FK Combination creation 
                    //IdentityRole_Id = c.String(maxLength: 128), 
                    //IdentityUser_Id = c.String(maxLength: 128),
                })
                .PrimaryKey(t => new { t.UserId, t.RoleId })

                //Addded below for making uniform Key references
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.RoleId)
                .Index(t => t.UserId);


            //Commented additional PK FK Combination creation 
            //.ForeignKey("dbo.Roles", t => t.IdentityRole_Id)
            //.ForeignKey("dbo.Users", t => t.IdentityUser_Id)
            //.Index(t => t.IdentityRole_Id)
            //.Index(t => t.IdentityUser_Id);

            CreateTable(
                "dbo.Users",
                c => new
                {
                    UserId = c.String(nullable: false, maxLength: 128),
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
                    NewProperty = c.String(),
                    Discriminator = c.String(nullable: false, maxLength: 128),
                })
                .PrimaryKey(t => t.UserId);

            CreateTable(
                "dbo.UserClaims",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),

                    //UserId = c.String(),//For the error Column 'UserId' in table 'dbo.UserClaims' is of a type that is invalid for use as a key column in an index.Added some length to column varchar
                    UserId = c.String(nullable: false, maxLength: 128),

                    ClaimType = c.String(),
                    ClaimValue = c.String(),
                    //IdentityUser_Id = c.String(maxLength: 128),
                })
                .PrimaryKey(t => t.Id)
                //Modified Keys as below
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            //Removed Key configurations
            //.ForeignKey("dbo.Users", t => t.IdentityUser_Id)
            //.Index(t => t.IdentityUser_Id);

            CreateTable(
                "dbo.UserLogins",
                c => new
                {
                    LoginProvider = c.String(nullable: false, maxLength: 128),
                    ProviderKey = c.String(nullable: false, maxLength: 128),
                    UserId = c.String(nullable: false, maxLength: 128),
                    //IdentityUser_Id = c.String(maxLength: 128),
                })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                //Modified Keys as below
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            //Removed Key configurations
            //.ForeignKey("dbo.Users", t => t.IdentityUser_Id)
            //.Index(t => t.IdentityUser_Id);

        }

        public override void Down()
        {
            //DropForeignKey("dbo.UserRoles", "IdentityUser_Id", "dbo.Users");
            //DropForeignKey("dbo.UserLogins", "IdentityUser_Id", "dbo.Users");
            //DropForeignKey("dbo.UserClaims", "IdentityUser_Id", "dbo.Users");
            //DropForeignKey("dbo.UserRoles", "IdentityRole_Id", "dbo.Roles");
            //DropIndex("dbo.UserLogins", new[] { "IdentityUser_Id" });
            //DropIndex("dbo.UserClaims", new[] { "IdentityUser_Id" });
            //DropIndex("dbo.UserRoles", new[] { "IdentityUser_Id" });
            //DropIndex("dbo.UserRoles", new[] { "IdentityRole_Id" });
            //DropTable("dbo.UserLogins");
            //DropTable("dbo.UserClaims");
            //DropTable("dbo.Users");
            //DropTable("dbo.UserRoles");
            //DropTable("dbo.Roles");
        }
    }
}
