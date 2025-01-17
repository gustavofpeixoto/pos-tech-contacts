using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PosTech.Contacts.ApplicationCore.Entities;

namespace PosTech.Contacts.Infrastructure.Configuration
{
    public class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasColumnType("varchar(200)");
            builder.Property(x => x.Surname).HasColumnType("varchar(500)");
            builder.Property(x => x.Email).HasColumnType("varchar(500)");
            builder.Property(x => x.Phone).HasColumnType("varchar(25)");
        }
    }
}
