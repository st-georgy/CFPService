using CFPService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CFPService.Infrastructure.Data.Configurations
{
    public class DraftConfiguration : IEntityTypeConfiguration<Draft>
    {
        public void Configure(EntityTypeBuilder<Draft> builder)
        {
            builder.ToTable("Draft");

            builder.HasKey(d => new { d.AuthorId, d.ApplicationId });

            builder.HasAlternateKey(d => d.AuthorId);

            builder.HasOne<Application>()
                .WithMany()
                .HasForeignKey(d => d.ApplicationId);
        }
    }
}
