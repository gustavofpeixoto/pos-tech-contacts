using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PosTech.Contacts.ApplicationCore.Entities.Command;

namespace PosTech.Contacts.Infrastructure.Configuration
{
    public class RegionConfiguration : IEntityTypeConfiguration<Region>
    {
        public void Configure(EntityTypeBuilder<Region> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.RegionName).HasColumnType("varchar(500)");
            builder.ToTable("Regions");
        }
    }
}
