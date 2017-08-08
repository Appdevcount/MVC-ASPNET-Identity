namespace MVC_ASPNET_Identity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TableRename : DbMigration
    {
        public override void Up()
        {
            //During table renaming there is some known / possible issue in prepared Configuration file after 
            //add - migration "" command.so it
            //needs some manual change as required ,then need to run update - database - verbose / script command -
            //Reference http://stackoverflow.com/questions/30114751/renaming-identity-tables-with-ef6-migrations-failing
            //The DropForeignKey() method was below RenameTable() method but refrring old table for Dropping Foreignkey

            //Now the Table USERROLES structure completely changed because of this - PK - FK Combination 2 column split to 4 columns of same menaing
            //Also all tables have separate foreign key referenced and created
            //Need to check on possibility of fixing the issue in migration file
            ////***Marking the possible changes in Migration Configuration  file

            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");

            RenameTable(name: "dbo.AspNetRoles", newName: "Roles");
            RenameTable(name: "dbo.AspNetUserRoles", newName: "UserRoles");
            RenameTable(name: "dbo.AspNetUsers", newName: "Users");
            RenameTable(name: "dbo.AspNetUserClaims", newName: "UserClaims");
            RenameTable(name: "dbo.AspNetUserLogins", newName: "UserLogins");

            DropIndex("dbo.Roles", "RoleNameIndex");
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.Users", "UserNameIndex");
            DropIndex("dbo.UserClaims", new[] { "UserId" });
            DropIndex("dbo.UserLogins", new[] { "UserId" });
            RenameColumn(table: "dbo.Roles", name: "Id", newName: "RoleId");
            RenameColumn(table: "dbo.Users", name: "Id", newName: "UserId");
            //**AddColumn("dbo.UserRoles", "IdentityRole_Id", c => c.String(maxLength: 128));
            //**AddColumn("dbo.UserRoles", "IdentityUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Users", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            //**AddColumn("dbo.UserClaims", "IdentityUser_Id", c => c.String(maxLength: 128));
            //**AddColumn("dbo.UserLogins", "IdentityUser_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.Roles", "Name", c => c.String());
            AlterColumn("dbo.Users", "Email", c => c.String());
            AlterColumn("dbo.Users", "UserName", c => c.String());
            AlterColumn("dbo.UserClaims", "UserId", c => c.String());
            //**CreateIndex("dbo.UserRoles", "IdentityRole_Id");
            //**CreateIndex("dbo.UserRoles", "IdentityUser_Id");
            //**CreateIndex("dbo.UserClaims", "IdentityUser_Id");
            //**CreateIndex("dbo.UserLogins", "IdentityUser_Id");
            //**AddForeignKey("dbo.UserRoles", "IdentityRole_Id", "dbo.Roles", "RoleId");
            //**AddForeignKey("dbo.UserClaims", "IdentityUser_Id", "dbo.Users", "UserId");
            //**AddForeignKey("dbo.UserLogins", "IdentityUser_Id", "dbo.Users", "UserId");
            //**AddForeignKey("dbo.UserRoles", "IdentityUser_Id", "dbo.Users", "UserId");
            //ENABLE**//AddForeignKey("dbo.UserRoles", "UserId", "dbo.Users", "UserId");
            //ENABLE**//AddForeignKey("dbo.UserClaims", "UserId", "dbo.Users", "UserId");
            //ENABLE**//AddForeignKey("dbo.UserLogins", "UserId", "dbo.Users", "UserId");
            //ENABLE**//AddForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles", "RoleId");

        }

        public override void Down()
        {
            //**DropForeignKey("dbo.UserRoles", "IdentityUser_Id", "dbo.Users");
            //**DropForeignKey("dbo.UserLogins", "IdentityUser_Id", "dbo.Users");
            //**DropForeignKey("dbo.UserClaims", "IdentityUser_Id", "dbo.Users");
            //**DropForeignKey("dbo.UserRoles", "IdentityRole_Id", "dbo.Roles");
            //**DropIndex("dbo.UserLogins", new[] { "IdentityUser_Id" });
            //**DropIndex("dbo.UserClaims", new[] { "IdentityUser_Id" });
            //**DropIndex("dbo.UserRoles", new[] { "IdentityUser_Id" });
            //**DropIndex("dbo.UserRoles", new[] { "IdentityRole_Id" });
            AlterColumn("dbo.UserClaims", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Users", "UserName", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.Users", "Email", c => c.String(maxLength: 256));
            AlterColumn("dbo.Roles", "Name", c => c.String(nullable: false, maxLength: 256));
            //**DropColumn("dbo.UserLogins", "IdentityUser_Id");
            //**DropColumn("dbo.UserClaims", "IdentityUser_Id");
            DropColumn("dbo.Users", "Discriminator");
            //**DropColumn("dbo.UserRoles", "IdentityUser_Id");
            //**DropColumn("dbo.UserRoles", "IdentityRole_Id");
            RenameColumn(table: "dbo.Users", name: "UserId", newName: "Id");
            RenameColumn(table: "dbo.Roles", name: "RoleId", newName: "Id");
            CreateIndex("dbo.UserLogins", "UserId");
            CreateIndex("dbo.UserClaims", "UserId");
            CreateIndex("dbo.Users", "UserName", unique: true, name: "UserNameIndex");
            CreateIndex("dbo.UserRoles", "RoleId");
            CreateIndex("dbo.UserRoles", "UserId");
            CreateIndex("dbo.Roles", "Name", unique: true, name: "RoleNameIndex");
            AddForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles", "Id", cascadeDelete: true);
            RenameTable(name: "dbo.UserLogins", newName: "AspNetUserLogins");
            RenameTable(name: "dbo.UserClaims", newName: "AspNetUserClaims");
            RenameTable(name: "dbo.Users", newName: "AspNetUsers");
            RenameTable(name: "dbo.UserRoles", newName: "AspNetUserRoles");
            RenameTable(name: "dbo.Roles", newName: "AspNetRoles");
        }
    }
}
