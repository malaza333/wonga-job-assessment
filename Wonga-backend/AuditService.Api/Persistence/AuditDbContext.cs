using AuditService.Api.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuditService.Api.Persistence
{
    public class AuditDbContext : DbContext
    {
        public AuditDbContext(DbContextOptions<AuditDbContext> options)
            : base(options)
        {
        }

        public DbSet<AuditEvent> AuditEvents => Set<AuditEvent>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuditEvent>(builder =>
            {
                builder.ToTable("audit_events");

                builder.HasKey(x => x.Id);

                builder.Property(x => x.EventType)
                    .HasMaxLength(100)
                    .IsRequired();

                builder.Property(x => x.Email)
                    .HasMaxLength(255)
                    .IsRequired();

                builder.Property(x => x.CreatedAtUtc)
                    .IsRequired();

                builder.HasIndex(x => x.UserId);
                builder.HasIndex(x => x.CreatedAtUtc);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
