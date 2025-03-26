namespace MotorCycleRentail.Dto.Response.Motorcycle;

public record MotorcycleResponse
{
    /// <summary>
    /// Identificador único da moto. Não é o Id.
    /// </summary>
    [property: JsonPropertyName("identificador")]
    public string Identifier { get; init; }

    /// <summary>
    /// Ano da Moto.
    /// </summary>
    [property: JsonPropertyName("ano")]
    public int Year { get; init; }

    /// <summary>
    /// Modelo da Moto.
    /// </summary>
    [property: JsonPropertyName("modelo")]
    public string Model { get; init; }

    /// <summary>
    /// Placa da Moto.
    /// </summary>
    [property: JsonPropertyName("placa")]
    public string LicensePlate { get; init; }
};
