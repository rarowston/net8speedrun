
namespace net8Speedrun.FileManagement;

public partial interface IFileService
{
    public Task<Stream> GetFileAsync(string fileName);
    public Task<bool> UploadFileAsync(string fileName, Stream FileContents);
}