namespace MotorCycleRentail.Application.Services;

public interface IFileStorageService : IService
{
    Task<string> SaveFileAsync(string courierId, string base64Data);
    string? GetFileBase64(string fileId);
    bool DeleteFile(string fileId);
}
