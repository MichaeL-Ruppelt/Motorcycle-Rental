namespace MotorCycleRentail.Domain.Entities;

public class BaseEntity : IBaseEntity
{
    protected BaseEntity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.Now;
    }

    public Guid Id { get; set; }

    public DateTime CreatedAt { get; private set; }

    protected void SetCreateDate()
    {
        CreatedAt = DateTime.Now;
    }
}
