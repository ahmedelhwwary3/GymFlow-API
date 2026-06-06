using GymManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Data.Configurations
{
    internal class ExerciseConfigurations:IEntityTypeConfiguration<Exercise>
    {
        public void Configure(EntityTypeBuilder<Exercise> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK__Exercise__3214EC07938161E8");

            builder.HasIndex(e => e.Name, "UQ_Exercises_Name").IsUnique();

            builder.Property(e => e.Description).HasMaxLength(1000);
            builder.Property(e => e.Name).HasMaxLength(150);
        } 
    }
}
