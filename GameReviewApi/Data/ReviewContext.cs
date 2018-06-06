using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GameReviewApi.Entities;

namespace GameReviewApi.Data
{
    public class ReviewContext : DbContext
    {
        public ReviewContext(DbContextOptions<ReviewContext> options) : base(options)
        {
            Database.Migrate();
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Comment> Comments { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Review>()
        //        .HasOne(g => g.Game)
        //        .WithOne(r => r.Review)
        //        .HasForeignKey<Game>(g => g.ReviewId);

        //    modelBuilder.Entity<Review>()
        //        .HasMany(c => c.Comments)
        //        .WithOne(r => r.Review)
        //        .HasForeignKey<Comment>(c => c.ReviewId);

        //    modelBuilder.Entity<Game>().ToTable("Games");
        //    modelBuilder.Entity<Review>().ToTable("Reviews");
        //    modelBuilder.Entity<Comment>().ToTable("Comments");
        //}
    }
}
