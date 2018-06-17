using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.Api.Models;

namespace User.Api.Data
{
    public class UserContext:DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>()
                .ToTable("Users")
                .HasKey(u => u.Id);

            modelBuilder.Entity<UserProperty>().Property(u => u.Key).HasMaxLength(100);
            modelBuilder.Entity<UserProperty>().Property(u => u.Value).HasMaxLength(100);
            modelBuilder.Entity<UserProperty>()
                .ToTable("UserProperties")
                .HasKey(u => new { u.Key, u.AppUserId, u.Value });


            modelBuilder.Entity<UserTag>()
                .ToTable("UserTags")
                .HasKey(u => new { u.UserId, u.Tag });
            

            modelBuilder.Entity<BPFile>()
                .ToTable("BPFiles")
                .HasKey(u => new { u.Id });


            base.OnModelCreating(modelBuilder);
        }

        public DbSet<AppUser> Users { get; set; }
    }
}
