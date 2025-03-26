namespace MotorCycleRentail.Dto.Request;

public record UpdateCnhImageRequest
{
    /// <summary>
    /// Imagem da CNH em base64.
    /// </summary>
    [property: Required(ErrorMessage = "The CNH image is required.")]
    [property: JsonPropertyName("imagem_cnh")]
    public string CnhImage { get; init; }
}
