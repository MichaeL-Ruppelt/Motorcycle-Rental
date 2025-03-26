using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorCycleRentail.Infra.Persistence.Sql.Contexts.Mapping;

public class RentalMapping : IEntityTypeConfiguration<Rental>
{
    private readonly IConfiguration _configuration;

    public RentalMapping(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(EntityTypeBuilder<Rental> builder)
    {
        builder.ToTable("rentals", _configuration.GetSection("PostgressSchema").Value);
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
            .HasColumnName("plan_id");

        builder.Property(c => c.CourierIdentifier)
            .HasColumnName("courier_identifier")
            .HasMaxLength(255);

        builder.Property(c => c.MotorcycleIdentifier)
            .HasColumnName("motorcycle_identifier")
            .HasMaxLength(255);

        builder.Property(c => c.StartDate)
            .HasColumnName("start_date");

        builder.Property(c => c.EndDate)
            .HasColumnName("end_date");

        builder.Property(c => c.ExpectedEndDate)
            .HasColumnName("preview_end_date");

        builder.Property(c => c.ReturnDate)
            .HasColumnName("return_date")
            .IsRequired(false);

        builder.Property(c => c.IsDeleted)
            .HasColumnName("is_deleted");

        #region Index
        builder.HasIndex(c => c.IsDeleted);
        #endregion Index

    }
}
