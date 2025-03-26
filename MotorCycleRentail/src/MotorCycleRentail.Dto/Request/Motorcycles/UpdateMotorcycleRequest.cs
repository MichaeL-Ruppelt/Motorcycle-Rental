
namespace MotorCycleRentail.Dto.Request;

public record UpdateMotorcycleRequest
{
    /// <summary>
    /// Placa da Moto.
    /// </summary>
    [property: Required(ErrorMessage = "The license plate is required.")]
    [property: RegularExpression(@"^(?:[A-Z]{3}-\d{4}|[A-Z0-9]{7})$", ErrorMessage = "The license plate must be in the format AAA-1111 or contain exactly 7 alphanumeric characters in any order.")]
    [property: JsonPropertyName("placa")]
    public string LicensePlate { get; init; }
};
