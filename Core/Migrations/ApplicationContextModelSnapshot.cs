﻿// <auto-generated />
using System;
using Core.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Core.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Core.Entity.FriendsUserToUser", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<long>("LeftUserId")
                        .HasColumnType("bigint");

                    b.Property<long>("RightUserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("LeftUserId");

                    b.HasIndex("RightUserId");

                    b.ToTable("FriendsUserToUsers");
                });

            modelBuilder.Entity("Core.Entity.UserToUserGroup", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<long>("VkUserGroupId")
                        .HasColumnType("bigint");

                    b.Property<long>("VkUserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("VkUserGroupId");

                    b.HasIndex("VkUserId");

                    b.ToTable("UserToUserGroup");
                });

            modelBuilder.Entity("Core.Entity.VkMessage", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<long>("DialogId")
                        .HasColumnType("bigint");

                    b.Property<long>("ExternalId")
                        .HasColumnType("bigint");

                    b.Property<string>("Text")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("VkMessages");
                });

            modelBuilder.Entity("Core.Entity.VkUser", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<long>("ExternalId")
                        .HasColumnType("bigint");

                    b.Property<string>("FullName")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeactivated")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("LastCheck")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("PhotoUrl")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("VkUsers");
                });

            modelBuilder.Entity("Core.Entity.VkUserGroup", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<long>("ExternalId")
                        .HasColumnType("bigint");

                    b.Property<int>("GroupType")
                        .HasColumnType("integer");

                    b.Property<bool>("IsDeactivated")
                        .HasColumnType("boolean");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("UserGroups");
                });

            modelBuilder.Entity("Core.Entity.FriendsUserToUser", b =>
                {
                    b.HasOne("Core.Entity.VkUser", "LeftUser")
                        .WithMany()
                        .HasForeignKey("LeftUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Entity.VkUser", "RightUser")
                        .WithMany()
                        .HasForeignKey("RightUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Entity.UserToUserGroup", b =>
                {
                    b.HasOne("Core.Entity.VkUserGroup", "VkUserGroup")
                        .WithMany()
                        .HasForeignKey("VkUserGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Entity.VkUser", "VkUser")
                        .WithMany()
                        .HasForeignKey("VkUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Entity.VkMessage", b =>
                {
                    b.HasOne("Core.Entity.VkUser", "VkUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
