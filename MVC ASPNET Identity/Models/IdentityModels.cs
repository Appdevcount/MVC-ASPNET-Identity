using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.ModelConfiguration;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_ASPNET_Identity.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, 
    // please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    //public class ApplicationUser : IdentityUser
    public class ApplicationUser : IdentityUser<string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        //After changing default Id[Guid] property in ASP.NETUSERS to UserId[Guid] - Id generation not happening at code side
        //Id property was mapped/renamed to UserId , so the Identity API was generating GUID as like ID property earlier
        //Making manual assignment to ID property to check if its assigned to UserID in table 
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString(); //Autogenerating Guid at Code on inserting - helped
        }
        //or
        //Below DB side autogeneration causes exception because directly I make changes and - The model backing the 'ApplicationDbContext' context has changed since the database was created. Consider using Code First Migrations to update the database 
        //Not trying migration option for DB side generation as we have alternate solution as above 
        //[DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.Identity)]
        //public override string Id
        //{
        //    get
        //    {
        //        return base.Id;
        //    }
        //    set
        //    {
        //        base.Id = value;
        //    }
        //}
        public string NewProperty { get; set; }
    }

    //public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public ApplicationDbContext()
            : base("SQLSERVER2014")
        //: base("DefaultConnection", throwIfV1Schema: false)
        {
            //This stops DB Recreation everytime
            //Database.SetInitializer<ApplicationDbContext>(null);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            ////Added this method for overiding the Default ASPNET Identity Table names

            //modelBuilder.Entity<IdentityUser>().ToTable("Users").HasKey(p => p.Id).Property(p => p.Id).HasColumnName("UserId");
            ////modelBuilder.Entity<ApplicationUser>().ToTable("MyUsers").Property(p => p.Id).HasColumnName("UserId");
            ////UserRoles is a Many to Many Relationship
            ////modelBuilder.Entity<IdentityRole>()
            ////  .HasMany<IdentityUser>(s => s.Users)
            ////  .WithMany(c => c.Roles)
            ////  .Map(cs =>
            ////  {
            ////      cs.MapLeftKey("Id");
            ////      cs.MapRightKey("Id");
            ////      cs.ToTable("StudentCourse");
            ////  });
            //modelBuilder.Entity<IdentityUserRole>().ToTable("UserRoles")
            //    .HasKey(p => new { p.UserId, p.RoleId });
            //modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins").HasKey(p => new { p.LoginProvider, p.ProviderKey, p.UserId });
            //modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims").HasKey(p => new { p.Id });
            //modelBuilder.Entity<IdentityRole>().ToTable("Roles").HasKey(p => p.Id).Property(p => p.Id).HasColumnName("RoleId");



            //So after wasting a couple hours on this, I copied my rules and pasted them below the base.OnModelCreating(modelBuilder);
            //line and everything worked properly.
            //I didn't realize that the base method needed to be called first
            //base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<IdentityUser>().ToTable("User");
            //modelBuilder.Entity<IdentityRole>().ToTable("Role");
            //modelBuilder.Entity<IdentityUserRole>().ToTable("UserRole");
            //modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaim");
            //modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogin");


            //Refering https://www.captechconsulting.com/blogs/Customizing-the-ASPNET-Identity-Data-Model-with-the-Entity-Framework-Fluent-API--Part-1 
            //For customization of ASP.NET Identity DB Objects - Creating Custom Object model inheriting Identity Data Model and using EF Fluent API on the Custom Object Models
            modelBuilder.HasDefaultSchema("Custom");

            modelBuilder.Entity<ApplicationUserLogin>().Map(c =>
            {
                c.ToTable("UserLogin");
                c.Properties(p => new
                {
                    p.UserId,
                    p.LoginProvider,
                    p.ProviderKey
                });
            }).HasKey(p => new { p.LoginProvider, p.ProviderKey, p.UserId });

            // Mapping for ApiRole
            modelBuilder.Entity<ApplicationRole>().Map(c =>
            {
                c.ToTable("Role");
                c.Property(p => p.Id).HasColumnName("RoleId");
                c.Properties(p => new
                {
                    p.Name
                });
            }).HasKey(p => p.Id);
            modelBuilder.Entity<ApplicationRole>().HasMany(c => c.Users).WithRequired().HasForeignKey(c => c.RoleId);

            modelBuilder.Entity<ApplicationUser>().Map(c =>
            {
                c.ToTable("User");
                c.Property(p => p.Id).HasColumnName("UserId");

                c.Properties(p => new
                {
                    p.AccessFailedCount,
                    p.Email,
                    p.EmailConfirmed,
                    p.PasswordHash,
                    p.PhoneNumber,
                    p.PhoneNumberConfirmed,
                    p.TwoFactorEnabled,
                    p.SecurityStamp,
                    p.LockoutEnabled,
                    p.LockoutEndDateUtc,
                    p.UserName,
                    p.NewProperty
                });
            }).HasKey(c => c.Id);


            //EntityTypeConfiguration<ApplicationUser> SEConfig = modelBuilder.Entity<ApplicationUser>();
            //SEConfig.

            modelBuilder.Entity<ApplicationUser>().HasMany(c => c.Logins).WithOptional().HasForeignKey(c => c.UserId);
            modelBuilder.Entity<ApplicationUser>().HasMany(c => c.Claims).WithOptional().HasForeignKey(c => c.UserId);
            modelBuilder.Entity<ApplicationUser>().HasMany(c => c.Roles).WithRequired().HasForeignKey(c => c.UserId);

            modelBuilder.Entity<ApplicationUserRole>().Map(c =>
            {
                c.ToTable("UserRole");
                c.Properties(p => new
                {
                    p.UserId,
                    p.RoleId
                });
            })
            .HasKey(c => new { c.UserId, c.RoleId });

            modelBuilder.Entity<ApplicationUserClaim>().Map(c =>
            {
                c.ToTable("UserClaim");
                c.Property(p => p.Id).HasColumnName("UserClaimId");
                c.Properties(p => new
                {
                    p.UserId,
                    p.ClaimValue,
                    p.ClaimType
                });
            }).HasKey(c => c.Id);



        }
    }
}