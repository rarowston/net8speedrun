using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// builder.Logging.AddJsonConsole(options =>
// {
//     options.IncludeScopes = false;
//     options.TimestampFormat = "HH:mm:ss ";
//     options.JsonWriterOptions = new JsonWriterOptions
//     {
//         Indented = true
//     };
// });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
