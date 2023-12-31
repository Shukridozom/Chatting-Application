﻿// <auto-generated />
using System;
using ChattingApplication.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ChattingApplication.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20231023221817_EditDefaultValuesForConfirmationCodes_Table")]
    partial class EditDefaultValuesForConfirmationCodes_Table
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ChattingApplication.Core.Domains.ConfirmationCode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(6)
                        .HasColumnType("varchar(6)");

                    b.Property<DateTime>("ExpireDate")
                        .HasColumnType("datetime(6)");

                    b.Property<byte>("RemainingCodesForThisDay")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint unsigned")
                        .HasDefaultValue((byte)4);

                    b.Property<byte>("Trials")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint unsigned")
                        .HasDefaultValue((byte)3);

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("ConfirmationCodes");
                });

            modelBuilder.Entity("ChattingApplication.Core.Domains.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<bool>("IsVerified")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("RegisteredDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ChattingApplication.Core.Domains.ConfirmationCode", b =>
                {
                    b.HasOne("ChattingApplication.Core.Domains.User", "User")
                        .WithOne("ConfirmationCode")
                        .HasForeignKey("ChattingApplication.Core.Domains.ConfirmationCode", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ChattingApplication.Core.Domains.User", b =>
                {
                    b.Navigation("ConfirmationCode")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
