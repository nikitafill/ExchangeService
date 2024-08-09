using ExchangeService.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeService.Infrastructure.Configuration
{
    internal sealed class ExchangeRateConfiguration
    {
        public void Configure(EntityTypeBuilder<ExchangeRate> builder)
        {
            builder.ToTable(nameof(ExchangeRate));

            builder.HasKey(e => new { e.Date, e.CurrencyCode });
            builder.Property(e => e.Date).IsRequired();
            builder.Property(e => e.Rate).IsRequired();
            builder.Property(e => e.CurrencyCode).IsRequired();
        }
    }
}
