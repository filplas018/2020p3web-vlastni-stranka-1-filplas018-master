using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using P3_app_plass.Models;

namespace P3_app_plass.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Garage> Garages { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<IdentityRole>().HasData(new IdentityRole { Id = "ADMIN", Name = "Administrátor", NormalizedName = "ADMINISTRÁTOR" });
            var hasher = new PasswordHasher<IdentityUser>();
            builder.Entity<IdentityUser>().HasData(new IdentityUser
            {
                Id = "ADMINUSER",
                Email = "filip.plass@pslib.cz",
                NormalizedEmail = "FILIP.PLASS@PSLIB.CZ",
                EmailConfirmed = true,
                LockoutEnabled = false,
                UserName = "filip.plass@pslib.cz",
                NormalizedUserName = "FILIP.PLASS@PSLIB.CZ",
                PasswordHash = hasher.HashPassword(null, "Admin_1234"),
                SecurityStamp = string.Empty
            });   
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string> { RoleId = "ADMIN", UserId = "ADMINUSER" });
            builder.Entity<Vehicle>().HasData(new Vehicle { Id = 1, GarageId = 1, Brand = "Fiat", Model = "Stilo 1.4 16V 6 speed", Description = "70 kw\n3 door" });
            builder.Entity<Garage>().HasData(new Garage { Id = 1, Name = "Italian Cars", UserId = "ADMINUSER" });


        }
    }
}
