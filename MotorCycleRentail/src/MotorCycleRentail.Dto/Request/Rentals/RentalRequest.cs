namespace MotorCycleRentail.Dto.Request;

public record RentalRequest
{
    /// <summary>
    /// Identificador único do entregador.
    /// </summary>
    [property: Required(ErrorMessage = "The identifier is required.")]
    [property: JsonPropertyName("entregador_id")]
    public string CourierIdentifier { get; init; }

    /// <summary>
    /// Identificador único da moto.
    /// </summary>
    [property: Required(ErrorMessage = "The motorcycle ID is required.")]
    [property: JsonPropertyName("moto_id")]
    public string MotorcycleIdentifier { get; init; }

    /// <summary>
    /// Data de início do aluguel.
    /// </summary>
    [property: Required(ErrorMessage = "The start date is required.")]
    [property: JsonPropertyName("data_inicio")]
    public DateTime StartDate { get; init; }

    /// <summary>
    /// Data de término do aluguel.
    /// </summary>
    [property: Required(ErrorMessage = "The end date is required.")]
    [property: JsonPropertyName("data_termino")]
    public DateTime EndDate { get; init; }

    /// <summary>
    /// Data prevista para término do aluguel.
    /// </summary>
    [property: Required(ErrorMessage = "The expected end date is required.")]
    [property: JsonPropertyName("data_previsao_termino")]
    public DateTime ExpectedEndDate { get; init; }

    /// <summary>
    /// Plano do aluguel.
    /// </summary>
    [property: Required(ErrorMessage = "The plan is required.")]
    [property: JsonPropertyName("plano")]
    public int PlanDays { get; init; }
};
