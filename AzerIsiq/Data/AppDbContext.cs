using AzerIsiq.Dtos;
using AzerIsiq.Models;
using Microsoft.EntityFrameworkCore;

namespace AzerIsiq.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        
        public DbSet<Region> Regions { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Substation> Substations { get; set; }
        public DbSet<Tm> Tms { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<OtpCode> OtpCodes { get; set; }
        public DbSet<LogEntry> LogEntries { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, RoleName = "User" },
                new Role { Id = 2, RoleName = "Admin" }
            );
            
            modelBuilder.Entity<District>()
                .HasOne(d => d.Region)
                .WithMany(r => r.Districts)
                .HasForeignKey(d => d.RegionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Substation>()
                .HasOne(s => s.District)
                .WithMany(d => d.Substations)
                .HasForeignKey(s => s.DistrictId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Tm>()
                .HasOne(t => t.Substation)
                .WithMany(s => s.Tms)
                .HasForeignKey(t => t.SubstationId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Substation>()
                .HasOne(s => s.Location)
                .WithMany(l => l.Substations)
                .HasForeignKey(s => s.LocationId)
                .OnDelete(DeleteBehavior.SetNull);
            
            modelBuilder.Entity<LogEntry>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(le => le.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<OtpCode>()
                .HasOne(o => o.User)
                .WithMany(u => u.OtpCodes)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
            
            modelBuilder.Entity<Image>()
                .HasOne(i => i.Substation)
                .WithMany(s => s.Images)
                .HasForeignKey(i => i.SubstationId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<Image>()
                .HasOne(i => i.Tm)
                .WithMany(t => t.Images)
                .HasForeignKey(i => i.TmId)
                .OnDelete(DeleteBehavior.NoAction);
            
            modelBuilder.Entity<Location>()
                .Property(l => l.Latitude)
                .HasColumnType("decimal(9,6)");

            modelBuilder.Entity<Location>()
                .Property(l => l.Longitude)
                .HasColumnType("decimal(9,6)");
        }
    }
}