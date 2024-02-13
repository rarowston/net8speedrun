using Microsoft.FeatureManagement;
using net8Speedrun.Constants;
using net8Speedrun.FileManagement;
using Polly;
using Microsoft.Extensions.Http.Resilience;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

/* #region keyed Services */
// Old method
builder.Services.AddScoped<IFileService, FileService>();

// Set up feature flagging
builder.Services.AddFeatureManagement(builder.Configuration.GetSection("FeatureFlags"));

builder.Services.AddKeyedScoped<IFileService, BlobService>(ServiceKeys.IFileServiceKeys.BLOB_SERVICE_KEY);
builder.Services.AddKeyedScoped<IFileService, FileService>(ServiceKeys.IFileServiceKeys.FILE_SERVICE_KEY);
builder.Services.AddKeyedScoped<IFileService, StubFileService>(ServiceKeys.IFileServiceKeys.STUB_SERVICE_KEY);

/* #endregion */

/* #region resilience */

builder.Services.AddHttpClient();
builder.Services.AddHttpClient("resilient")
    .AddStandardResilienceHandler();

// You can customised options
builder.Services.AddHttpClient("resilient-custom")
    .AddStandardResilienceHandler(options =>
    {
        // Configure standard resilience options here
        options.Retry.ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
            .Handle<HttpRequestException>()
            .HandleResult(response => response.StatusCode == HttpStatusCode.InternalServerError)
            .HandleResult(response => response.StatusCode == HttpStatusCode.NotFound); // e.g. retry on NotFound error codes
    });


// // You can also create a custom pipeline if you have more specific requirements
// builder.Services.AddHttpClient("resilient-full-custom")
//     .AddResilienceHandler("custom-pipeline", builder =>
//     {
//         builder.AddRetry(new HttpRetryStrategyOptions
//         {
//             MaxRetryAttempts = 4,
//             Delay = TimeSpan.FromSeconds(2),
//             BackoffType = DelayBackoffType.Exponential
//         });
//         builder.AddTimeout(TimeSpan.FromSeconds(10));
//     });

/* #endregion resilience */

var app = builder.Build();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
