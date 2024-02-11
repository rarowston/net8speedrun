using System.Runtime.CompilerServices;

public static partial class FileServiceLogger
{
    [LoggerMessage(Level = LogLevel.Information, Message = "{method} called for {fileName} in {implementation}")]
    public static partial void LogFileAction(ILogger logger, string fileName, string implementation = "", [CallerMemberName] string method = "");
}