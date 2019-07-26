﻿// <auto-generated />
using System;
using Core.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Core.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20190726073511_AddMessagesDate")]
    partial class AddMessagesDate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Core.Entity.FriendsUserToUser", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("LeftUserId");

                    b.Property<long>("RightUserId");

                    b.HasKey("Id");

                    b.HasIndex("LeftUserId");

                    b.HasIndex("RightUserId");

                    b.ToTable("FriendsUserToUsers");
                });

            modelBuilder.Entity("Core.Entity.Message", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateTime");

                    b.Property<long>("DialogId");

                    b.Property<long>("ExternalId");

                    b.Property<string>("Text");

                    b.Property<string>("Title");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("Core.Entity.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FullName");

                    b.Property<bool>("IsDeactivated");

                    b.Property<DateTime>("LastCheck");

                    b.Property<int>("MuturalCount");

                    b.Property<string>("PhotoUrl");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Core.Entity.FriendsUserToUser", b =>
                {
                    b.HasOne("Core.Entity.User", "LeftUser")
                        .WithMany()
                        .HasForeignKey("LeftUserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Core.Entity.User", "RightUser")
                        .WithMany()
                        .HasForeignKey("RightUserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Core.Entity.Message", b =>
                {
                    b.HasOne("Core.Entity.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Core.Entity.User", b =>
                {
                    b.HasOne("Core.Entity.User")
                        .WithMany("MuturalFriend")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
