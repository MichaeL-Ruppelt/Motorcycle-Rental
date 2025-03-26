namespace MotorCycleRentail.Domain.Entities;

public class Rental : BaseEntity
{
    public DateTime? UpdateAt { get; set; }
    public int PlanDays { get; set; }
    public string CourierIdentifier { get; set; }
    public string MotorcycleIdentifier { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime ExpectedEndDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public bool IsDeleted { get; set; } = false;

}
