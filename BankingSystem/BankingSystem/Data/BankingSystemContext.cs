#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BankingSystem.Models;

namespace BankingSystem.Data
{
    public class BankingSystemContext : DbContext
    {
        public BankingSystemContext(DbContextOptions<BankingSystemContext> options)
            : base(options)
        {
        }

        public DbSet<BankingSystem.Models.Customer> Customers { get; set; }

        public DbSet<BankingSystem.Models.Account> Accounts { get; set; }

        public DbSet<BankingSystem.Models.Transaction> Transactions { get; set; }

        public DbSet<BankingSystem.Models.Transfer> Transfers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().ToTable("Account");
            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<Transaction>().ToTable("Transaction");
            modelBuilder.Entity<Transfer>().ToTable("Transfer");
        }

    }
}