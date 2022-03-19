﻿using BankApplication.Models;

using BankApplication.Models.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BankApplication.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Banking> Banking { get; set; }
        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<Operator> Operator { get; set; }
        public virtual DbSet<Manager> Manager { get; set; }
        public virtual DbSet<EnterpriseSpecialist> EnterpriseSpecialist { get; set; }
        public virtual DbSet<Administrator> Administrator { get; set; }
        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<Credit> Credit { get; set; }
        public virtual DbSet<UserCreation> UserCreation { get; set; }
        public virtual DbSet<CreditCreation> CreditCreation { get; set; }

        
        public virtual DbSet<MoneyStatictic> MoneyStatictic { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

           
            modelBuilder.Entity<Banking>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Name).IsRequired();
            });
           


            modelBuilder.Entity<Client>(entity =>
             {
                 entity.Property(e => e.Id).ValueGeneratedOnAdd();

                 entity.Property(e => e.UserId).HasMaxLength(450);

                 entity.HasOne(d => d.Bank)
                     .WithMany(p => p.Clients)
                     .HasForeignKey(d => d.BankingId)
                     .OnDelete(DeleteBehavior.ClientSetNull)
                     .HasConstraintName("FK_Clients_Bankings");

                 entity.HasOne(d => d.User)
                     .WithMany(p => p.Clients)
                     .HasForeignKey(d => d.UserId)
                     .OnDelete(DeleteBehavior.ClientSetNull)
                     .HasConstraintName("FK_Clients_Users");
             });
           
             modelBuilder.Entity<Operator>(entity =>
             {
                 entity.Property(e => e.Id).ValueGeneratedOnAdd();

                 entity.Property(e => e.UserId).HasMaxLength(450);

                 entity.HasOne(d => d.Bank)
                     .WithMany(p => p.Operators)
                     .HasForeignKey(d => d.BankingId)
                     .OnDelete(DeleteBehavior.ClientSetNull)
                     .HasConstraintName("FK_Operators_Bankings");

                 entity.HasOne(d => d.User)
                     .WithMany(p => p.Operators)
                     .HasForeignKey(d => d.UserId)
                     .OnDelete(DeleteBehavior.ClientSetNull)
                     .HasConstraintName("FK_Operators_Users");
             });
            
             modelBuilder.Entity<Manager>(entity =>
             {
                 entity.Property(e => e.Id).ValueGeneratedOnAdd();

                 entity.Property(e => e.UserId).HasMaxLength(450);

                 entity.HasOne(d => d.Bank)
                     .WithMany(p => p.Managers)
                     .HasForeignKey(d => d.BankingId)
                     .OnDelete(DeleteBehavior.ClientSetNull)
                     .HasConstraintName("FK_Managers_Bankings");

                 entity.HasOne(d => d.User)
                     .WithMany(p => p.Managers)
                     .HasForeignKey(d => d.UserId)
                     .OnDelete(DeleteBehavior.ClientSetNull)
                     .HasConstraintName("FK_Managers_Users");
             });
            
             modelBuilder.Entity<EnterpriseSpecialist>(entity =>
             {
                 entity.Property(e => e.Id).ValueGeneratedOnAdd();

                 entity.Property(e => e.UserId).HasMaxLength(450);

                 entity.HasOne(d => d.Bank)
                     .WithMany(p => p.EnterpriseSpecialists)
                     .HasForeignKey(d => d.BankingId)
                     .OnDelete(DeleteBehavior.ClientSetNull)
                     .HasConstraintName("FK_EnterpriseSpecialists_Bankings");

                 entity.HasOne(d => d.User)
                     .WithMany(p => p.EnterpriseSpecialists)
                     .HasForeignKey(d => d.UserId)
                     .OnDelete(DeleteBehavior.ClientSetNull)
                     .HasConstraintName("FK_EnterpriseSpecialists_Users");
             });
            
             modelBuilder.Entity<Administrator>(entity =>
             {
                 entity.Property(e => e.Id).ValueGeneratedOnAdd();

                 entity.Property(e => e.UserId).HasMaxLength(450);

                 entity.HasOne(d => d.Bank)
                     .WithMany(p => p.Administrators)
                     .HasForeignKey(d => d.BankingId)
                     .OnDelete(DeleteBehavior.ClientSetNull)
                     .HasConstraintName("FK_.Administrators_Bankings");

                 entity.HasOne(d => d.User)
                     .WithMany(p => p.Administrators)
                     .HasForeignKey(d => d.UserId)
                     .OnDelete(DeleteBehavior.ClientSetNull)
                     .HasConstraintName("FK_.Administrators_Users");
             });

            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();



                entity.HasOne(d => d.Client)
                      .WithMany(p => p.Accounts)
                      .HasForeignKey(d => d.ClientId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK_.Accounts_Clients");
            });

            modelBuilder.Entity<Installment>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();



                entity.HasOne(d => d.Client)
                      .WithMany(p => p.Installments)
                      .HasForeignKey(d => d.ClientId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK_.Installments_Clients");

                entity.HasOne(d => d.Account)
                      .WithMany(p => p.Installments)
                      .HasForeignKey(d => d.AccountId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK_.Installments_Accounts");

            });

            modelBuilder.Entity<Credit>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();



                entity.HasOne(d => d.Client)
                      .WithMany(p => p.Credits)
                      .HasForeignKey(d => d.ClientId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK_.Credits_Clients");

                entity.HasOne(d => d.Account)
                     .WithMany(p => p.Credits)
                     .HasForeignKey(d => d.AccountId)
                     .OnDelete(DeleteBehavior.ClientSetNull)
                     .HasConstraintName("FK_.Credits_Accounts");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();


            });

            modelBuilder.Entity<CreditCreation>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();


            });
           

            modelBuilder.Entity<MoneyStatictic>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

            });

           








            base.OnModelCreating(modelBuilder);
        }
    }
}