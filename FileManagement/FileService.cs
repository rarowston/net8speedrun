
namespace net8Speedrun.FileManagement;

public class FileService : IFileService
{
    private readonly ILogger<FileService> _logger;
    private readonly IWebHostEnvironment _environment;

    public FileService(ILogger<FileService> logger, IWebHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
    }

    public async Task<Stream> GetFileAsync(string fileName)
    {
        FileServiceLogger.LogFileAction(_logger, fileName, "File Service");
        await Task.Delay(0); //remove warning
        string fileDirectory = "file-storage";
        string computedPath = Path.Join(fileDirectory, fileName);
        return new FileStream(computedPath, FileMode.Open, FileAccess.Read);
    }

    public async Task<bool> UploadFileAsync(string fileName, Stream fileContents)
    {
        FileServiceLogger.LogFileAction(_logger, fileName, "File Service");
        string fileDirectory = "file-storage";
        string computedPath = Path.Join(fileDirectory, fileName);
        using (var fileStream = new FileStream(computedPath, FileMode.OpenOrCreate, FileAccess.Write))
        {
            await fileContents.CopyToAsync(fileStream);
        }
        return true;
    }
}