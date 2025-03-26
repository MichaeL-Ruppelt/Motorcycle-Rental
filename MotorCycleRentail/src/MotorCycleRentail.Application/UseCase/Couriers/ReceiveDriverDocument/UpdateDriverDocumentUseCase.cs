using MotorCycleRentail.Common.Helpers;
using MotorCycleRentail.Dto.Request;

namespace MotorCycleRentail.Application.Usecase;

public class UpdateDriverDocumentUseCase : IUpdateDriverDocumentUseCase
{
    #region ctor
    private readonly ILogger<UpdateDriverDocumentUseCase> _logger;
    private readonly ICourierRepository _courierRepository;
    private readonly IFileStorageService _fileStorageService;

    public UpdateDriverDocumentUseCase(ILogger<UpdateDriverDocumentUseCase> logger, 
                                       ICourierRepository courierRepository, 
                                       IFileStorageService fileStorageService)
    {
        _logger = logger;
        _courierRepository = courierRepository;
        _fileStorageService = fileStorageService;
    }
    #endregion ctor

    public async Task<bool> ExecuteAsync(string courierIdentifier, string newCnhImage, CancellationToken ct = default)
    {
        if (!Base64Validator.IsValidBase64Image(newCnhImage))
            return false;

        var courier = await _courierRepository.GetByIdentifierAsync(courierIdentifier, ct);
        if (courier is null)
        {
            _logger.LogWarning($"Courier dont exist. ID: {courierIdentifier}");
            return false;
        }

        string oldCnhImageId = courier.CnhImageId;

        string CnhImageId = await _fileStorageService.SaveFileAsync(courier.Id.ToString(), newCnhImage);
        courier.CnhImageId = CnhImageId;
        
        _courierRepository.UpdateAsync(courier, ct);

        if (!_fileStorageService.DeleteFile(oldCnhImageId))
            _logger.LogWarning($"Canot delete file. FileId: {oldCnhImageId}");

        return true;
    }
}
