using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MotorCycleRentail.Infra.Persistence.Sql.Contexts.Mapping;

public class CourierMapping : IEntityTypeConfiguration<Courier>
{
    private readonly IConfiguration _configuration;

    public CourierMapping(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(EntityTypeBuilder<Courier> builder)
    {
        builder.ToTable("couriers", _configuration.GetSection("PostgressSchema").Value);
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

        builder.Property(c => c.Identifier)
            .HasColumnName("identifier")
            .HasMaxLength(100);

        builder.Property(c => c.Name)
            .HasColumnName("name")
            .HasMaxLength(255);

        builder.Property(c => c.Cnpj)
            .HasColumnName("cnpj")
            .HasMaxLength(14);

        builder.Property(c => c.Birthdate)
            .HasColumnName("birthdate")
            .HasColumnType("DATE");

        builder.Property(c => c.CnhNumber)
            .HasColumnName("cnh_number")
            .HasMaxLength(11);

        builder.Property(c => c.CnhType)
            .HasColumnName("cnh_type")
            .HasMaxLength(2);

        builder.Property(c => c.CnhImageId)
            .HasColumnName("cnh_image_id");

        builder.Property(c => c.IsDeleted)
            .HasColumnName("is_deleted");


        #region Index
        builder.HasIndex(c => c.IsDeleted);
        builder.HasIndex(c => c.CnhNumber);
        #endregion Index
    }
}

