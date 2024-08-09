using System.Collections.Generic;
using System.IO;
using System.Reflection.Emit;
using ExchangeService.Domain.Models;
using ExchangeService.Infrastructure.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExchangeService.Infrastructure.DbContexts
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ExchangeRate> ExchangeRates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ExchangeRateConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}