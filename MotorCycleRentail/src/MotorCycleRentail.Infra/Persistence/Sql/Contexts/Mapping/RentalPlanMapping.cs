using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MotorCycleRentail.Infra.Persistence.Sql.Contexts.Mapping;

public class RentalPlanMapping : IEntityTypeConfiguration<RentalPlan>
{
    private readonly IConfiguration _configuration;

    public RentalPlanMapping(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(EntityTypeBuilder<RentalPlan> builder)
    {
        builder.ToTable("rental_plan", _configuration.GetSection("PostgressSchema").Value);
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("id")
            .HasMaxLength(36)
            .IsFixedLength(true);

        builder.Property(c => c.CreatedAt)
            .HasColumnName("created_at");

        builder.Property(c => c.UpdateAt)
            .HasColumnName("update_at")
            .IsRequired(false);

        builder.Property(c => c.PlanDays)
            .HasColumnName("plan_days");

        builder.Property(c => c.PlanValue)
            .HasColumnName("plan_value")
            .HasPrecision(18, 2)
            .HasColumnType("money");

        builder.Property(c => c.FineValue)
            .HasColumnName("fine_value")
            .HasPrecision(18, 2);

        builder.Property(c => c.IsDeleted)
            .HasColumnName("is_deleted");

        #region Index
        builder.HasIndex(c => c.PlanDays);
        builder.HasIndex(c => c.IsDeleted);
        #endregion Index

    }
}


