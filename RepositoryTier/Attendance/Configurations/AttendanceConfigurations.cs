
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;  

namespace RepositoryTier.Attendance.Configurations
{
    internal class AttendanceConfigurations : IEntityTypeConfiguration<Entities.Attendance>
    {
        public void Configure(EntityTypeBuilder<Entities.Attendance> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK__Attendan__3214EC0794DDB513");

            builder.HasIndex(e => e.MemberId, "IX_Attendances_MemberId");

            builder.HasIndex(e => new { e.MemberId, e.AttendanceDate }, "UQ_Attendances_MemberId_AttendanceDate").IsUnique();

            builder.Property(e => e.Notes).HasMaxLength(500);

            builder.HasOne(d => d.Member).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Attendances_Members");
        }
    }
}
