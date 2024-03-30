using CFPService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CFPService.Infrastructure.Data.Configurations
{
    public class ApplicationConfiguration : IEntityTypeConfiguration<Application>
    {
        public void Configure(EntityTypeBuilder<Application> builder)
        {
            builder.ToTable("Application");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Activity)
                .IsRequired();

            builder.Property(a => a.Title)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(a => a.Description)
                .HasMaxLength(300);

            builder.Property(a => a.Outline)
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(a => a.CreatedDate)
                .HasDefaultValueSql("now()");
        }
    }
}
