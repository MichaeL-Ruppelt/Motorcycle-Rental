using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MotorCycleRentail.Infra.Persistence.Sql.Contexts.Mapping;
public class MotorcycleMapping : IEntityTypeConfiguration<Motorcycle>
{
    private readonly IConfiguration _configuration;

    public MotorcycleMapping(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(EntityTypeBuilder<Motorcycle> builder)
    {
        builder.ToTable("motorcycles", _configuration.GetSection("PostgressSchema").Value);
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("id")
            .HasMaxLength(36)
            .IsFixedLength(true);

        builder.Property(c => c.CreatedAt)
            .HasColumnName("created_at");

        builder.Property(c => c.Identifier)
            .HasColumnName("identifier")
            .HasMaxLength(100);

        builder.Property(c => c.Year)
            .HasColumnName("year");

        builder.Property(c => c.Model)
            .HasColumnName("model")
            .HasMaxLength(200);

        builder.Property(c => c.LicensePlate)
            .HasColumnName("license_plate")
            .HasMaxLength(7);

        builder.Property(c => c.IsDeleted)
            .HasColumnName("is_deleted");


        #region Index
        builder.HasIndex(c => c.IsDeleted);
        #endregion Index

        #region Relationship
        //builder.HasMany(x => x.Rentals)
        //    .WithOne()
        //    .HasForeignKey(x => x.MotorcycleIdentifier)
        //    .OnDelete(DeleteBehavior.Restrict);
        #endregion Relationship
    }
}
