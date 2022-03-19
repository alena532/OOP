﻿// <auto-generated />
using System;
using BankApplication.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BankApplication.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20220319122040_mymigr")]
    partial class mymigr
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("BankApplication.Models.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Sum")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("Account");
                });

            modelBuilder.Entity("BankApplication.Models.Banking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Banking");
                });

            modelBuilder.Entity("BankApplication.Models.Credit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<double>("CurrentSum")
                        .HasColumnType("float");

                    b.Property<int>("LengthMonth")
                        .HasColumnType("int");

                    b.Property<double>("MonthProcent")
                        .HasColumnType("float");

                    b.Property<double>("MustSum")
                        .HasColumnType("float");

                    b.Property<DateTime>("TimeBegin")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("TimeFinish")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("TimeNow")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("ClientId");

                    b.ToTable("Credit");
                });

            modelBuilder.Entity("BankApplication.Models.CreditCreation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<string>("BankName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<int>("Month")
                        .HasColumnType("int");

                    b.Property<int>("MonthProcent")
                        .HasColumnType("int");

                    b.Property<double>("NeededSum")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("CreditCreation");
                });

            modelBuilder.Entity("BankApplication.Models.Entity.Administrator", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("BankingId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("BankingId");

                    b.HasIndex("UserId");

                    b.ToTable("Administrator");
                });

            modelBuilder.Entity("BankApplication.Models.Entity.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("BankingId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("BankingId");

                    b.HasIndex("UserId");

                    b.ToTable("Client");
                });

            modelBuilder.Entity("BankApplication.Models.Entity.EnterpriseSpecialist", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("BankingId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("BankingId");

                    b.HasIndex("UserId");

                    b.ToTable("EnterpriseSpecialist");
                });

            modelBuilder.Entity("BankApplication.Models.Entity.Manager", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("BankingId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("BankingId");

                    b.HasIndex("UserId");

                    b.ToTable("Manager");
                });

            modelBuilder.Entity("BankApplication.Models.Entity.Operator", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("BankingId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("BankingId");

                    b.HasIndex("UserId");

                    b.ToTable("Operator");
                });

            modelBuilder.Entity("BankApplication.Models.Installment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<int>("CurrentSum")
                        .HasColumnType("int");

                    b.Property<int>("MustSum")
                        .HasColumnType("int");

                    b.Property<DateTime>("TimeBegin")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("TimeFinish")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("TimeNow")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("ClientId");

                    b.ToTable("Installment");
                });

            modelBuilder.Entity("BankApplication.Models.MoneyStatictic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("ClientI")
                        .HasColumnType("int");

                    b.Property<string>("NameOpration")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("MoneyStatictic");
                });

            modelBuilder.Entity("BankApplication.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<int?>("BIC")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("IdentificationNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LegalAdress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LegalName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<int?>("Number")
                        .HasColumnType("int");

                    b.Property<int?>("PAN")
                        .HasColumnType("int");

                    b.Property<int?>("PassportNumber")
                        .HasColumnType("int");

                    b.Property<string>("PassportSeries")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("BankApplication.Models.UserCreation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("BankName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdentificationNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PassportNumber")
                        .HasColumnType("int");

                    b.Property<string>("PassportSeries")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("UserCreation");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("BankApplication.Models.Account", b =>
                {
                    b.HasOne("BankApplication.Models.Entity.Client", "Client")
                        .WithMany("Accounts")
                        .HasForeignKey("ClientId")
                        .IsRequired()
                        .HasConstraintName("FK_.Accounts_Clients");

                    b.Navigation("Client");
                });

            modelBuilder.Entity("BankApplication.Models.Credit", b =>
                {
                    b.HasOne("BankApplication.Models.Account", "Account")
                        .WithMany("Credits")
                        .HasForeignKey("AccountId")
                        .IsRequired()
                        .HasConstraintName("FK_.Credits_Accounts");

                    b.HasOne("BankApplication.Models.Entity.Client", "Client")
                        .WithMany("Credits")
                        .HasForeignKey("ClientId")
                        .IsRequired()
                        .HasConstraintName("FK_.Credits_Clients");

                    b.Navigation("Account");

                    b.Navigation("Client");
                });

            modelBuilder.Entity("BankApplication.Models.Entity.Administrator", b =>
                {
                    b.HasOne("BankApplication.Models.Banking", "Bank")
                        .WithMany("Administrators")
                        .HasForeignKey("BankingId")
                        .IsRequired()
                        .HasConstraintName("FK_.Administrators_Bankings");

                    b.HasOne("BankApplication.Models.User", "User")
                        .WithMany("Administrators")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK_.Administrators_Users");

                    b.Navigation("Bank");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BankApplication.Models.Entity.Client", b =>
                {
                    b.HasOne("BankApplication.Models.Banking", "Bank")
                        .WithMany("Clients")
                        .HasForeignKey("BankingId")
                        .IsRequired()
                        .HasConstraintName("FK_Clients_Bankings");

                    b.HasOne("BankApplication.Models.User", "User")
                        .WithMany("Clients")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK_Clients_Users");

                    b.Navigation("Bank");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BankApplication.Models.Entity.EnterpriseSpecialist", b =>
                {
                    b.HasOne("BankApplication.Models.Banking", "Bank")
                        .WithMany("EnterpriseSpecialists")
                        .HasForeignKey("BankingId")
                        .IsRequired()
                        .HasConstraintName("FK_EnterpriseSpecialists_Bankings");

                    b.HasOne("BankApplication.Models.User", "User")
                        .WithMany("EnterpriseSpecialists")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK_EnterpriseSpecialists_Users");

                    b.Navigation("Bank");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BankApplication.Models.Entity.Manager", b =>
                {
                    b.HasOne("BankApplication.Models.Banking", "Bank")
                        .WithMany("Managers")
                        .HasForeignKey("BankingId")
                        .IsRequired()
                        .HasConstraintName("FK_Managers_Bankings");

                    b.HasOne("BankApplication.Models.User", "User")
                        .WithMany("Managers")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK_Managers_Users");

                    b.Navigation("Bank");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BankApplication.Models.Entity.Operator", b =>
                {
                    b.HasOne("BankApplication.Models.Banking", "Bank")
                        .WithMany("Operators")
                        .HasForeignKey("BankingId")
                        .IsRequired()
                        .HasConstraintName("FK_Operators_Bankings");

                    b.HasOne("BankApplication.Models.User", "User")
                        .WithMany("Operators")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK_Operators_Users");

                    b.Navigation("Bank");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BankApplication.Models.Installment", b =>
                {
                    b.HasOne("BankApplication.Models.Account", "Account")
                        .WithMany("Installments")
                        .HasForeignKey("AccountId")
                        .IsRequired()
                        .HasConstraintName("FK_.Installments_Accounts");

                    b.HasOne("BankApplication.Models.Entity.Client", "Client")
                        .WithMany("Installments")
                        .HasForeignKey("ClientId")
                        .IsRequired()
                        .HasConstraintName("FK_.Installments_Clients");

                    b.Navigation("Account");

                    b.Navigation("Client");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("BankApplication.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("BankApplication.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BankApplication.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("BankApplication.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BankApplication.Models.Account", b =>
                {
                    b.Navigation("Credits");

                    b.Navigation("Installments");
                });

            modelBuilder.Entity("BankApplication.Models.Banking", b =>
                {
                    b.Navigation("Administrators");

                    b.Navigation("Clients");

                    b.Navigation("EnterpriseSpecialists");

                    b.Navigation("Managers");

                    b.Navigation("Operators");
                });

            modelBuilder.Entity("BankApplication.Models.Entity.Client", b =>
                {
                    b.Navigation("Accounts");

                    b.Navigation("Credits");

                    b.Navigation("Installments");
                });

            modelBuilder.Entity("BankApplication.Models.User", b =>
                {
                    b.Navigation("Administrators");

                    b.Navigation("Clients");

                    b.Navigation("EnterpriseSpecialists");

                    b.Navigation("Managers");

                    b.Navigation("Operators");
                });
#pragma warning restore 612, 618
        }
    }
}