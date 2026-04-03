using Microsoft.EntityFrameworkCore;
using TMS.Domain.Entities;

namespace TMS.Infrastructure.Persistence;

/// <summary>
/// SQL Server persistence context for TMS data.
/// </summary>
public class TmsDbContext : DbContext
{
    public TmsDbContext(DbContextOptions<TmsDbContext> options) : base(options)
    {
    }

    public DbSet<TrainerProfile> Trainers => Set<TrainerProfile>();
    public DbSet<TrainerSkill> TrainerSkills => Set<TrainerSkill>();
    public DbSet<TrainingSession> TrainingSessions => Set<TrainingSession>();
    public DbSet<TrainingAssignment> TrainingAssignments => Set<TrainingAssignment>();
    public DbSet<LeaveRecord> LeaveRecords => Set<LeaveRecord>();
    public DbSet<HolidayCalendarDay> HolidayCalendarDays => Set<HolidayCalendarDay>();
    public DbSet<NotificationTemplate> NotificationTemplates => Set<NotificationTemplate>();
    public DbSet<NotificationConfiguration> NotificationConfigurations => Set<NotificationConfiguration>();
    public DbSet<NotificationLog> NotificationLogs => Set<NotificationLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TrainerProfile>()
            .HasMany(x => x.Skills)
            .WithOne()
            .HasForeignKey(x => x.TrainerId);

        modelBuilder.Entity<TrainerProfile>()
            .HasIndex(x => x.EmployeeCode)
            .IsUnique();

        modelBuilder.Entity<TrainingAssignment>()
            .HasIndex(x => new { x.SessionId, x.TrainerId, x.Role })
            .IsUnique();

        modelBuilder.Entity<NotificationTemplate>()
            .HasIndex(x => new { x.EventKey, x.Channel })
            .IsUnique();

        modelBuilder.Entity<NotificationConfiguration>()
            .HasIndex(x => x.EventKey)
            .IsUnique();

        base.OnModelCreating(modelBuilder);
    }
}
