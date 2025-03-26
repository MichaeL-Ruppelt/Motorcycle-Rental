namespace MotorCycleRentail.Dto.Request;

public record RentalUpdateReturnRequest
{
    /// <summary>
    /// Data de devolução do veículo.
    /// </summary>
    [property: Required(ErrorMessage = "The expected ReturnDate is required.")]
    [property: JsonPropertyName("data_devolucao")]
    public DateTime ReturnDate { get; init; }
}
