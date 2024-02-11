using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.FeatureManagement;
using net8Speedrun.Constants;
using net8Speedrun.FileManagement;

namespace net8Speedrun.Controllers;

[ApiController]
[Route("[controller]")]
public class FileInteractionController : ControllerBase
{
    private readonly ILogger<FileInteractionController> _logger;
    private readonly IFeatureManager _featureManager;
    private readonly IServiceProvider _serviceProvider;

    public FileInteractionController(ILogger<FileInteractionController> logger, IFeatureManager featureManager, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _featureManager = featureManager;
        _serviceProvider = serviceProvider;
    }

    [HttpGet]
    public async Task<IActionResult> GetFileAsync([FromQuery] string fileName) // FYI: This is a demo. Don't do this in a production system. You can't trust user input
    {
        // Choose the appropriate IFileService
        IFileService fileService = await GetAppropriateFileServiceAsync();

        /* #region DetermineContentType */
        string? contentType;
        FileExtensionContentTypeProvider provider = new FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(fileName, out contentType)) { contentType = "application/octet-stream"; }
        /* #endregion */

        Stream fileStream = await fileService.GetFileAsync(fileName); // Seriously, don't do this for a real system - use an internal ID or, at minimum, do some sanitization.
        return File(fileStream, contentType);
    }

    [HttpPost]
    public async Task<IActionResult> SubmitFile()
    {
        IFormFileCollection files = HttpContext.Request.Form.Files;
        IFormFile? file = files.FirstOrDefault();
        if (file == null)
        {
            return BadRequest();
        }

        // Choose the appropriate IFileService
        IFileService fileService = await GetAppropriateFileServiceAsync();

        if (await fileService.UploadFileAsync(file.FileName, file.OpenReadStream()))
        {
            return Accepted();
        }
        else
        {
            return BadRequest();
        }

    }

    private async Task<IFileService> GetAppropriateFileServiceAsync()
    {
        if (await _featureManager.IsEnabledAsync(FeatureFlags.BLOB_STORAGE))
        {
            return _serviceProvider.GetRequiredKeyedService<IFileService>(ServiceKeys.IFileServiceKeys.BLOB_SERVICE_KEY);
        }
        else if (await _featureManager.IsEnabledAsync(FeatureFlags.FILE_STORAGE))
        {
            return _serviceProvider.GetRequiredKeyedService<IFileService>(ServiceKeys.IFileServiceKeys.FILE_SERVICE_KEY);
        }
        else
        {
            return _serviceProvider.GetRequiredKeyedService<IFileService>(ServiceKeys.IFileServiceKeys.STUB_SERVICE_KEY);
        }
    }

    /* #region Bonus Endpoints*/

    /// <summary>
    /// This forces the system to use the file version of the service by requesting hte object from keyed services. 
    /// </summary>
    [HttpPost("ForceFile")]
    public async Task<IActionResult> SubmitFileToDisk([FromKeyedServices(ServiceKeys.IFileServiceKeys.FILE_SERVICE_KEY)] IFileService fileService)
    {
        IFormFileCollection files = HttpContext.Request.Form.Files;
        IFormFile? file = files.FirstOrDefault();
        if (file == null)
        {
            return BadRequest();
        }

        if (await fileService.UploadFileAsync(file.FileName, file.OpenReadStream()))
        {
            return Accepted();
        }
        else
        {
            return BadRequest();
        }
    }

    /// <summary>
    /// This forces the system to use the file version of the service by requesting hte object from keyed services. 
    /// </summary>
    [HttpPost("ForceStub")]
    public async Task<IActionResult> SubmitFileStub([FromKeyedServices(ServiceKeys.IFileServiceKeys.STUB_SERVICE_KEY)] IFileService fileService)
    {
        IFormFileCollection files = HttpContext.Request.Form.Files;
        IFormFile? file = files.FirstOrDefault();
        if (file == null)
        {
            return BadRequest();
        }

        if (await fileService.UploadFileAsync(file.FileName, file.OpenReadStream()))
        {
            return Accepted();
        }
        else
        {
            return BadRequest();
        }
    }

    /* #endregion */

}