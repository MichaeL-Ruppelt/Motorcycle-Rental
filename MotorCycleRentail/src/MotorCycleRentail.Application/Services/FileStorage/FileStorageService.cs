using Microsoft.Extensions.Logging;

namespace FileStorageAPI.Services;

public class FileStorageService : IFileStorageService
{
    private readonly ILogger<FileStorageService> _logger;
    private readonly string _storagePath;

    public FileStorageService(ILogger<FileStorageService> logger)
    {
        _logger = logger;
        _storagePath = Path.Combine(Directory.GetCurrentDirectory(), "DriverDocuments");
        Directory.CreateDirectory(_storagePath); 
    }

    /// <summary>
    /// Salva ou substitui um arquivo, gerando um GUID como nome.
    /// </summary>
    public async Task<string> SaveFileAsync(string courierId, string base64Data)
    {
        byte[] fileBytes = Convert.FromBase64String(base64Data);

        string fileId = Guid.NewGuid().ToString();
        string fileExtension = GetFileExtensionFromBytes(fileBytes);
        string filePath = Path.Combine(_storagePath, fileId + fileExtension);

        await File.WriteAllBytesAsync(filePath, fileBytes);

        _logger.LogInformation($"Arquivo Salvo: CourierId:{courierId} , FileId:{fileId}.");

        return fileId;
    }

    /// <summary>
    /// Obtém um arquivo pelo ID.
    /// </summary>
    public string? GetFileBase64(string fileId)
    {
        string filePath = Path.Combine(_storagePath, fileId);

        if (!File.Exists(filePath))
        {
            _logger.LogInformation($"Arquivo Inexistente: FileId:{fileId}.");
            return null;
        }

        byte[] fileBytes = File.ReadAllBytes(filePath);
        return Convert.ToBase64String(fileBytes);
    }

    /// <summary>
    /// Remove um arquivo pelo ID.
    /// </summary>
    public bool DeleteFile(string fileId)
    {
        string filePath = Path.Combine(_storagePath, fileId);

        if (!File.Exists(filePath))
        {
            _logger.LogInformation($"File does not exist: FileId:{fileId}.");
            return false;
        }

        File.Delete(filePath);
        _logger.LogInformation($"File removed: FileId:{fileId}.");
        return true;
    }

    private string GetFileExtensionFromBytes(byte[] fileBytes)
    {
        // Verifica os primeiros bytes do arquivo para identificar o tipo
        if (fileBytes.Length > 4)
        {
            // PNG: Começa com 89 50 4E 47
            if (fileBytes[0] == 0x89 && fileBytes[1] == 0x50 && fileBytes[2] == 0x4E && fileBytes[3] == 0x47)
            {
                return ".png";
            }
            // JPEG: Começa com FF D8
            if (fileBytes[0] == 0xFF && fileBytes[1] == 0xD8)
            {
                return ".jpg";
            }
            // GIF: Começa com 47 49 46
            if (fileBytes[0] == 0x47 && fileBytes[1] == 0x49 && fileBytes[2] == 0x46)
            {
                return ".gif";
            }
            // PDF: Começa com 25 50 44 46
            if (fileBytes[0] == 0x25 && fileBytes[1] == 0x50 && fileBytes[2] == 0x44 && fileBytes[3] == 0x46)
            {
                return ".pdf";
            }
        }

        // Se não conseguir identificar, retorna nulo (sem extensão)
        return null;
    }
}
