using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Vu_Assign3_Blog.Core.Models;

namespace Vu_Assign3_Blog.Infrastructure.Data
{
    public class Assign3DbContext : DbContext
    {
        public Assign3DbContext(DbContextOptions<Assign3DbContext> options)
        : base(options)
        {
        }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Title).IsRequired().HasMaxLength(200);
                entity.Property(p => p.Content).IsRequired();
                entity.Property(p => p.Author);
                entity.Property(p => p.CreatedDate).IsRequired();
                entity.Property(e => e.UpdatedDate);



            });
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
                entity.Property(c => c.Email).IsRequired().HasMaxLength(150);
                entity.Property(c => c.Content).IsRequired().HasMaxLength(1000);
                entity.Property(c => c.CreatedDate);

                entity.HasOne(e => e.Post)
                          .WithMany(e => e.Comments)
                          .HasForeignKey(c => c.PostId)
                          .OnDelete(DeleteBehavior.Cascade);


            });
        }
    }
}