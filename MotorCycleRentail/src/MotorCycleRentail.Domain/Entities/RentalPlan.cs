namespace MotorCycleRentail.Domain.Entities;
public class RentalPlan : BaseEntity
{
    public DateTime? UpdateAt { get; set; }
    public int PlanDays { get; set; }
    public decimal PlanValue { get; set; }
    public decimal FineValue { get; set; }
    public bool IsDeleted { get; set; } = false;
}
