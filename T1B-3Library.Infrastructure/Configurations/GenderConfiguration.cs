using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using T1B_3Library.Domain.Entities;

namespace T1B_3Library.Infrastructure.Configurations
{
    public class GenderConfiguration : IEntityTypeConfiguration<Gender>
    {
        public void Configure(EntityTypeBuilder<Gender> builder)
        {
            builder.HasKey(g => g.Id);

            builder.Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}