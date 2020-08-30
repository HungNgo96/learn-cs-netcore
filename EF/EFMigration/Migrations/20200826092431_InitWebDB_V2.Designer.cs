﻿// <auto-generated />
using EFMigration.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EFMigration.Migrations
{
    [DbContext(typeof(WebContext))]
    [Migration("20200826092431_InitWebDB_V2")]
    partial class InitWebDB_V2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EFMigration.Models.Article", b =>
                {
                    b.Property<int>("ArticleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content")
                        .HasColumnType("ntext");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("ArticleId");

                    b.ToTable("article");
                });

            modelBuilder.Entity("EFMigration.Models.ArticleTag", b =>
                {
                    b.Property<int>("ArticleTagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ArticleId")
                        .HasColumnType("int");

                    b.Property<string>("TagId")
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.HasKey("ArticleTagId");

                    b.HasIndex("TagId");

                    b.HasIndex("ArticleId", "TagId")
                        .IsUnique()
                        .HasFilter("[TagId] IS NOT NULL");

                    b.ToTable("articletag");
                });

            modelBuilder.Entity("EFMigration.Models.Tag", b =>
                {
                    b.Property<string>("TagId")
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("Content")
                        .HasColumnType("ntext");

                    b.HasKey("TagId");

                    b.ToTable("tags");
                });

            modelBuilder.Entity("EFMigration.Models.ArticleTag", b =>
                {
                    b.HasOne("EFMigration.Models.Article", "article")
                        .WithMany()
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EFMigration.Models.Tag", "tag")
                        .WithMany()
                        .HasForeignKey("TagId");
                });
#pragma warning restore 612, 618
        }
    }
}