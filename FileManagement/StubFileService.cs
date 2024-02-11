using System.Text;

namespace net8Speedrun.FileManagement;

public class StubFileService : IFileService
{
    private readonly ILogger<StubFileService> _logger;

    public StubFileService(ILogger<StubFileService> logger)
    {
        _logger = logger;
    }

    public async Task<Stream> GetFileAsync(string fileName)
    {
        FileServiceLogger.LogFileAction(_logger, fileName, "Stub Service");
        MemoryStream stream = new MemoryStream();
        byte[] bytes = Encoding.UTF8.GetBytes("Hello, this is a stub document");

        await stream.WriteAsync(bytes);
        stream.Position = 0;
        return stream;
    }

    public async Task<bool> UploadFileAsync(string fileName, Stream FileContents)
    {
        FileServiceLogger.LogFileAction(_logger, fileName, "Stub Service");
        await Task.Delay(10); // 10ms delay
        return true;
    }
}