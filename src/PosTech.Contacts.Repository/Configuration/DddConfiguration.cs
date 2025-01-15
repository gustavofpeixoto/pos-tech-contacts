using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PosTech.Contacts.ApplicationCore.Entities;

namespace PosTech.Contacts.Infrastructure.Configuration
{
    public class DddConfiguration : IEntityTypeConfiguration<Ddd>
    {
        public void Configure(EntityTypeBuilder<Ddd> builder)
        {
            builder.HasKey(x => x.Id);

            builder
                .HasOne(ddd => ddd.Region)
                .WithMany(region => region.Ddds)
                .HasForeignKey(p => p.RegionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(ddd => ddd.Contacts)
                .WithOne(contact => contact.Ddd)
                .HasForeignKey(contact => contact.DddId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
