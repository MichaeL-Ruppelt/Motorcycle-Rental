
namespace MotorCycleRentail.Domain.Entities;

public class Motorcycle : BaseEntity
{
    public string Identifier { get; set; }
    public int Year { get; set; }
    public string Model { get; set; }
    public string LicensePlate { get; set; }
    public bool IsDeleted { get; set; } = false;

    #region Relationships
    //public ICollection<Rental> Rentals { get; set; }
    #endregion Relationships
}
