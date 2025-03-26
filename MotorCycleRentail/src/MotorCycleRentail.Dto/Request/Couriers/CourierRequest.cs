namespace MotorCycleRentail.Dto.Request;

public record CourierRequest
{
    /// <summary>
    /// Identificador único do entregador. Não é o Id.
    /// </summary>
    [property: Required(ErrorMessage = "The identifier is required.")]
    [property: JsonPropertyName("identificador")]
    public string Identifier { get; init; }

    /// <summary>
    /// Nome do entregador.
    /// </summary>
    [property: Required(ErrorMessage = "The name is required.")]
    [property: JsonPropertyName("nome")]
    public string Name { get; init; }

    /// <summary>
    /// CNPJ do entregador.
    /// </summary>
    [property: Required(ErrorMessage = "The CNPJ is required.")]
    [property: RegularExpression(@"^\D*(\d{14})\D*$", ErrorMessage = "The CNPJ must contain exactly 14 digits.")]
    [property: JsonPropertyName("cnpj")]
    public string Cnpj { get; init; }

    /// <summary>
    /// Data de nascimento do entregador.
    /// </summary>
    [property: Required(ErrorMessage = "The birthdate is required.")]
    [property: JsonPropertyName("data_nascimento")]
    public DateTime Birthdate { get; init; }

    /// <summary>
    /// Número da CNH do entregador.
    /// </summary>
    [property: Required(ErrorMessage = "The CNH number is required.")]
    [property: RegularExpression(@"^\D*(\d{11})\D*$", ErrorMessage = "The CNH number must contain exactly 11 digits.")]
    [property: JsonPropertyName("numero_cnh")]
    public string CnhNumber { get; init; }

    /// <summary>
    /// Tipo da CNH do entregador.
    /// </summary>
    [property: Required(ErrorMessage = "The CNH type is required.")]
    [property: RegularExpression(@"^(?i:A|B|AB)$", ErrorMessage = "The CNH type must be 'A', 'B', or 'AB'.")]
    [property: JsonPropertyName("tipo_cnh")]
    public string CnhType { get; init; }

    /// <summary>
    /// Imagem da CNH em base64.
    /// </summary>
    [property: Required(ErrorMessage = "The CNH image is required.")]
    [property: JsonPropertyName("imagem_cnh")]
    public string CnhImage { get; init; }
};
