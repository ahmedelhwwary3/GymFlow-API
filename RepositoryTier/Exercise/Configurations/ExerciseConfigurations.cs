
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace RepositoryTier.Exercise.Configurations
{
    internal class ExerciseConfigurations:IEntityTypeConfiguration<Entities.Exercise>
    {
        public void Configure(EntityTypeBuilder<Entities.Exercise> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK__Exercise__3214EC07938161E8");

            builder.HasIndex(e => e.Name, "UQ_Exercises_Name").IsUnique();

            builder.Property(e => e.Description).HasMaxLength(1000);
            builder.Property(e => e.Name).HasMaxLength(150);
        } 
    }
}
