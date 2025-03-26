namespace MotorCycleRentail.Dto.Response;

public record RentalResponse
{
    /// <summary>
    /// Identificador único do aluguel. Não é o Id.
    /// </summary>
    [property: JsonPropertyName("identificador")]
    public string Identifier { get; init; }

    /// <summary>
    /// Valor da diária do aluguel.
    /// </summary>
    [property: JsonPropertyName("valor_diaria")]
    public decimal RentalPrice { get; init; }

    /// <summary>
    /// Identificador único do entregador.
    /// </summary>
    [property: JsonPropertyName("entregador_id")]
    public string CourierIdentifier { get; init; }

    /// <summary>
    /// Identificador único da moto.
    /// </summary>
    [property: JsonPropertyName("moto_id")]
    public string MotorcycleIdentifier { get; init; }

    /// <summary>
    /// Data de início do aluguel.
    /// </summary>
    [property: JsonPropertyName("data_inicio")]
    public DateTime StartDate { get; init; }

    /// <summary>
    /// Data de término do aluguel.
    /// </summary>
    [property: JsonPropertyName("data_termino")]
    public DateTime EndDate { get; init; }

    /// <summary>
    /// Data prevista para término do aluguel.
    /// </summary>
    [property: JsonPropertyName("data_previsao_termino")]
    public DateTime ExpectedEndDate { get; init; }

    /// <summary>
    /// Data de devolução do aluguel. Pode ser nula se ainda não tiver sido devolvida.
    /// </summary>
    [property: JsonPropertyName("data_devolucao")]
    public DateTime? ReturnDate { get; init; }
};
