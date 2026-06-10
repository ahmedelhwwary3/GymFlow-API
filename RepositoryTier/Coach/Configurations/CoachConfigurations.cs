
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders; 

namespace RepositoryTier.Coach.Configurations
{
    internal class CoachConfigurations : IEntityTypeConfiguration<Entities.Coach>
    {
        public void Configure(EntityTypeBuilder<Entities.Coach> builder)
        { 

            builder.Property(e => e.Salary).HasColumnType("decimal(18, 2)");

            builder.HasQueryFilter(e => !e.IsDeleted && e.DeletedAt != null);
        }
    }
}
