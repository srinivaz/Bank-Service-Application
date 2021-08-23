using BankService.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankService.Data
{
    public class BankDBContext : DbContext
    {
        public BankDBContext(DbContextOptions<BankDBContext> options) : base(options)
        {
            Database.Migrate();
        }
        public DbSet<Customer> customers { get; set; }
        public DbSet<Loan> loans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Loan>()
                .Property(p => p.LoanAmount)                
                .HasColumnType("decimal(18,4)");
            modelBuilder.Entity<Loan>()
                .Property(p => p.RateOfInterest)
                .HasColumnType("decimal(18,4)");
        }
    }
}
