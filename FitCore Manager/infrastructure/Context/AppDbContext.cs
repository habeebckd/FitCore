using FitCore_Manager.Model;
using Microsoft.EntityFrameworkCore;
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

        }
    }
}
