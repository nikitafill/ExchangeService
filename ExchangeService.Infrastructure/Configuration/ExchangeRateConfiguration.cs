using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ExchangeService.Domain.Models;

namespace ExchangeService.Infrastructure.Configuration
{
    public class ExchangeRateConfiguration : IEntityTypeConfiguration<Rate>
    {
        public void Configure(EntityTypeBuilder<Rate> builder)
        {
            builder.ToTable(nameof(Rate));

            builder.HasKey(e => new { e.Date, e.Cur_ID });

            builder.Property(e => e.Date).IsRequired();
            builder.Property(e => e.Cur_ID).IsRequired();
            builder.Property(e => e.Cur_Abbreviation).IsRequired();
            builder.Property(e => e.Cur_Scale).IsRequired();
            builder.Property(e => e.Cur_Name).IsRequired();
            builder.Property(e => e.Cur_OfficialRate).IsRequired();
        }
    }
}
