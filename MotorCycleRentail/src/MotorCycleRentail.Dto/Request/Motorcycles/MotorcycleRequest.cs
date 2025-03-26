namespace MotorCycleRentail.Dto.Request;

public record MotorcycleRequest
{
    /// <summary>
    /// Identificador único da moto. Não é o Id.
    /// </summary>
    [property: Required(ErrorMessage = "The identifier is required.")]
    [property: JsonPropertyName("identificador")]
    public string Identifier { get; init; }

    /// <summary>
    /// Ano da Moto.
    /// </summary>
    [property: Required(ErrorMessage = "The year is required.")]
    [property: Range(1900, 2100, ErrorMessage = "The year must be between 1900 and 2100.")]
    [property: JsonPropertyName("ano")]
    public int Year { get; init; }

    /// <summary>
    /// Modelo da Moto.
    /// </summary>
    [property: Required(ErrorMessage = "The model is required.")]
    [property: JsonPropertyName("modelo")]
    public string Model { get; init; }

    /// <summary>
    /// Placa da Moto.
    /// </summary>
    [property: Required(ErrorMessage = "The license plate is required.")]
    [property: RegularExpression(@"^(?:[A-Z]{3}-\d{4}|[A-Z0-9]{7})$", ErrorMessage = "The license plate must be in the format AAA-1111 or contain exactly 7 alphanumeric characters in any order.")]
    [property: JsonPropertyName("placa")]
    public string LicensePlate { get; init; }
};
