using Domain.Model;
using FitCore_Manager.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { } 
            public DbSet<User>Users { get; set; }
            public DbSet<UserMembership> UserMembership { get; set; }
            public DbSet<MembershipPlans> membershipPlans {  get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<User>()
            //    .Property(e => e.Role)
            //    .HasDefaultValue("User");

            modelBuilder.Entity<User>()
                .Property(x => x.isBlocked)
                .HasDefaultValue(false);

            modelBuilder.Entity<User>()
               .Property(x => x.CreatedAt)
               .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<User>()
                .HasOne(s=>s.UserMembership)
                .WithOne(s => s.User)
                .HasForeignKey<UserMembership>(s=>s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserMembership>()
                .HasOne(s=>s.MembershipPlans)
                .WithMany(s=>s.UserMembership)
                .HasForeignKey(s=>s.PlanId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MembershipPlans>()
                .Property(p => p.Price)
                .HasPrecision(10, 2);
        }
    }
}
