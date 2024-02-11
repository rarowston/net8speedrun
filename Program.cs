using Microsoft.FeatureManagement;
using net8Speedrun.Constants;
using net8Speedrun.FileManagement;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Set up feature flagging
builder.Services.AddFeatureManagement(builder.Configuration.GetSection("FeatureFlags"));

builder.Services.AddKeyedScoped<IFileService, BlobService>(ServiceKeys.IFileServiceKeys.BLOB_SERVICE_KEY);
builder.Services.AddKeyedScoped<IFileService, FileService>(ServiceKeys.IFileServiceKeys.FILE_SERVICE_KEY);
builder.Services.AddKeyedScoped<IFileService, StubFileService>(ServiceKeys.IFileServiceKeys.STUB_SERVICE_KEY);

var app = builder.Build();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
