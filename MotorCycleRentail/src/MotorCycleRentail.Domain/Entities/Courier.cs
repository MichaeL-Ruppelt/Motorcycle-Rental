namespace MotorCycleRentail.Domain.Entities;

public class Courier : BaseEntity
{
    public DateTime? UpdateAt { get; set; }
    public string Identifier { get; set; }
    public string Name { get; set; }
    public string Cnpj { get; set; }
    public DateTime Birthdate { get; set; }
    public string CnhNumber { get; set; }
    public string CnhType { get; set; }
    public string CnhImageId { get; set; }
    public bool IsDeleted { get; set; } = false;
}
