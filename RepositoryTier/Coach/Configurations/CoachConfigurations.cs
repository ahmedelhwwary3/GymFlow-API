
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using models= RepositoryTier.Models;

namespace RepositoryTier.Coach.Configurations
{
    internal class CoachConfigurations : IEntityTypeConfiguration<models.Coach>
    {
        public void Configure(EntityTypeBuilder<models.Coach> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK__Coaches__3214EC07E0D177E5"); 

            builder.Property(e => e.Salary).HasColumnType("decimal(18, 2)"); 
        }
    }
}
