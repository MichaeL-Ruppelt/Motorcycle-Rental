using MotorCycleRentail.Common.Helpers;

namespace MotorCycleRentail.Application.Usecase;

public class CreateCourierUsecase : ICreateCourierUsecase
{
    #region ctor
    private readonly ILogger<CreateCourierUsecase> _logger;
    private readonly ICourierRepository _courierRepository;
    private readonly IFileStorageService _fileStorageService;

    public CreateCourierUsecase(ILogger<CreateCourierUsecase> logger, 
                                ICourierRepository courierRepository, 
                                IFileStorageService fileStorageService)
    {
        _logger = logger;
        _courierRepository = courierRepository;
        _fileStorageService = fileStorageService;
    }
    #endregion ctor

    public async Task<bool> ExecuteAsync(CourierRequest request, CancellationToken ct = default)
    {
        if (!Base64Validator.IsValidBase64Image(request.CnhImage))
            return false;

        if (await _courierRepository.CheckIfCourierAlreadyExist(request.CnhNumber, request.Cnpj, ct))
        {
            _logger.LogWarning($"Courier already exist. CNH {request.CnhNumber}, CNPJ: {request.Cnpj}");
            return false;
        }
        
        var newCourier = BuildNewCourier(request);

        string CnhImageId = await _fileStorageService.SaveFileAsync(newCourier.Id.ToString(), request.CnhImage);
        newCourier.CnhImageId = CnhImageId;

        if (!IsDataValid(newCourier))
            return false;

        await _courierRepository.InsertAsync(newCourier, ct);

        return true;
    }

    #region Auxiliar Methods
    private static Courier BuildNewCourier(CourierRequest request)
    {
        return new Courier
        {
            Identifier = request.Identifier,
            Name = request.Name,
            Cnpj = request.Cnpj,
            Birthdate = request.Birthdate,
            CnhNumber = request.CnhNumber,
            CnhType = request.CnhType
        };
    }
    private bool IsDataValid(Courier newCourier)
    {
        var dataValidation = new CourierValidation().Validate(newCourier);
        if (!dataValidation.IsValid)
        {
            _logger.LogWarning($"Invalid data: {string.Join(", ", dataValidation.Errors)}");
            return false;
        }

        return true;
    }
    #endregion Auxiliar Methods
}
