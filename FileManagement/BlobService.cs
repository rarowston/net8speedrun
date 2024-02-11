namespace net8Speedrun.FileManagement;

public class BlobService : IFileService
{
    private readonly ILogger<BlobService> _logger;

    public BlobService(ILogger<BlobService> logger)
    {
        _logger = logger;
    }

    public Task<Stream> GetFileAsync(string fileName)
    {
        FileServiceLogger.LogFileAction(_logger, fileName, "Blob Service");
        throw new NotImplementedException("Not implemented for this demo");
    }

    public Task<bool> UploadFileAsync(string fileName, Stream FileContents)
    {
        FileServiceLogger.LogFileAction(_logger, fileName, "Blob Service");
        throw new NotImplementedException("Not implemented for this demo");
    }
}