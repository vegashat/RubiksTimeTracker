using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace RTT.Core.Models.Mapping
{
    public class SolveTimeMap : EntityTypeConfiguration<SolveTime>
    {
        public SolveTimeMap()
        {
            // Primary Key
            this.HasKey(t => new { t.TimeId, t.UserId, t.SolveDate, t.SolveTime1 });

            // Properties
            this.Property(t => t.TimeId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.UserId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("SolveTime");
            this.Property(t => t.TimeId).HasColumnName("TimeId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.SolveDate).HasColumnName("SolveDate");
            this.Property(t => t.SolveTime1).HasColumnName("SolveTime");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.SolveTimes)
                .HasForeignKey(d => d.UserId);

        }
    }
}