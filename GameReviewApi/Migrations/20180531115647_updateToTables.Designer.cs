﻿// <auto-generated />
using GameReviewApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace GameReviewApi.Migrations
{
    [DbContext(typeof(ReviewContext))]
    [Migration("20180531115647_updateToTables")]
    partial class updateToTables
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GameReviewApi.Entities.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author")
                        .IsRequired();

                    b.Property<string>("CommentContent")
                        .IsRequired();

                    b.Property<DateTime>("DatePosted");

                    b.Property<int>("ReviewId");

                    b.HasKey("Id");

                    b.HasIndex("ReviewId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("GameReviewApi.Entities.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Developer")
                        .IsRequired();

                    b.Property<string>("Genre")
                        .IsRequired();

                    b.Property<string>("Publisher")
                        .IsRequired();

                    b.Property<DateTime>("ReleaseDate");

                    b.Property<int>("ReviewId");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ReviewId")
                        .IsUnique();

                    b.ToTable("Games");
                });

            modelBuilder.Entity("GameReviewApi.Entities.Review", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author")
                        .IsRequired();

                    b.Property<string>("Body")
                        .IsRequired();

                    b.Property<string>("Conclusion")
                        .IsRequired();

                    b.Property<DateTime>("DatePosted");

                    b.Property<string>("Introduction")
                        .IsRequired();

                    b.Property<string>("ReviewTitle")
                        .IsRequired();

                    b.Property<string>("VideoUrl")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("GameReviewApi.Entities.Comment", b =>
                {
                    b.HasOne("GameReviewApi.Entities.Review", "Review")
                        .WithMany("Comments")
                        .HasForeignKey("ReviewId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GameReviewApi.Entities.Game", b =>
                {
                    b.HasOne("GameReviewApi.Entities.Review", "Review")
                        .WithOne("Game")
                        .HasForeignKey("GameReviewApi.Entities.Game", "ReviewId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
