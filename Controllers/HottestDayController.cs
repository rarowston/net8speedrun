using System.Net;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;

namespace net8Speedrun.Controllers;

[ApiController]
[Route("[controller]")]
public partial class HottestDayController : ControllerBase
{

    private readonly ILogger<HottestDayController> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string? _apiUrl;

    public HottestDayController(ILogger<HottestDayController> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _apiUrl = configuration.GetValue<string>("SecondaryApiUrl");
    }

    [HttpGet("Standard/Reliable")]
    public async Task<IActionResult> GetStandardReliableApiCallAsync()
    {

        HttpClient httpClient = _httpClientFactory.CreateClient();
        HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, _apiUrl + "/WeatherForecast/reliable");
        HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
        LogSentApiRequest(_logger, response.StatusCode);

        response.EnsureSuccessStatusCode();

        IEnumerable<WeatherForecast>? weatherForecastEnumerable = await response.Content.ReadFromJsonAsync<IEnumerable<WeatherForecast>>();
        if (weatherForecastEnumerable == null)
        {
            return NoContent();
        }

        return Ok(weatherForecastEnumerable.MaxBy(forecast => forecast.TemperatureC));
    }

    [HttpGet("Standard/Unreliable")]
    public async Task<IActionResult> GetStandardUnreliableApiCallAsync()
    {

        HttpClient httpClient = _httpClientFactory.CreateClient();
        HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, _apiUrl + "/WeatherForecast/unreliable");
        HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
        LogSentApiRequest(_logger, response.StatusCode);

        response.EnsureSuccessStatusCode();

        IEnumerable<WeatherForecast>? weatherForecastEnumerable = await response.Content.ReadFromJsonAsync<IEnumerable<WeatherForecast>>();
        if (weatherForecastEnumerable == null)
        {
            return NoContent();
        }

        return Ok(weatherForecastEnumerable.MaxBy(forecast => forecast.TemperatureC));
    }

    [HttpGet("Resilient/Reliable")]
    public async Task<IActionResult> GetResilientReliableApiCallAsync()
    {

        HttpClient httpClient = _httpClientFactory.CreateClient("resilient");
        HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, _apiUrl + "/WeatherForecast/reliable");
        HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
        LogSentApiRequest(_logger, response.StatusCode);

        response.EnsureSuccessStatusCode();

        IEnumerable<WeatherForecast>? weatherForecastEnumerable = await response.Content.ReadFromJsonAsync<IEnumerable<WeatherForecast>>();
        if (weatherForecastEnumerable == null)
        {
            return NoContent();
        }

        return Ok(weatherForecastEnumerable.MaxBy(forecast => forecast.TemperatureC));
    }

    [HttpGet("Resilient/Unreliable")]
    public async Task<IActionResult> GetResilientUnreliableApiCallAsync()
    {

        HttpClient httpClient = _httpClientFactory.CreateClient("resilient");
        HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, _apiUrl + "/WeatherForecast/unreliable");
        HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
        LogSentApiRequest(_logger, response.StatusCode);

        response.EnsureSuccessStatusCode();

        IEnumerable<WeatherForecast>? weatherForecastEnumerable = await response.Content.ReadFromJsonAsync<IEnumerable<WeatherForecast>>();
        if (weatherForecastEnumerable == null)
        {
            return NoContent();
        }

        return Ok(weatherForecastEnumerable.MaxBy(forecast => forecast.TemperatureC));
    }

    [HttpGet("Resilient/NotExistent")]
    public async Task<IActionResult> GetResilientNonExistentApiCallAsync()
    {

        HttpClient httpClient = _httpClientFactory.CreateClient("resilient");
        HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, _apiUrl + "/WeatherForecast/NonExistent");
        HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
        LogSentApiRequest(_logger, response.StatusCode);

        response.EnsureSuccessStatusCode();

        IEnumerable<WeatherForecast>? weatherForecastEnumerable = await response.Content.ReadFromJsonAsync<IEnumerable<WeatherForecast>>();
        if (weatherForecastEnumerable == null)
        {
            return NoContent();
        }

        return Ok(weatherForecastEnumerable.MaxBy(forecast => forecast.TemperatureC));
    }

    [HttpGet("Resilient/Borked")]
    public async Task<IActionResult> GetResilientBorkedApiCallAsync()
    {

        HttpClient httpClient = _httpClientFactory.CreateClient("resilient");
        HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, _apiUrl + "/WeatherForecast/borked");
        HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
        LogSentApiRequest(_logger, response.StatusCode);

        response.EnsureSuccessStatusCode();

        IEnumerable<WeatherForecast>? weatherForecastEnumerable = await response.Content.ReadFromJsonAsync<IEnumerable<WeatherForecast>>();
        if (weatherForecastEnumerable == null)
        {
            return NoContent();
        }

        return Ok(weatherForecastEnumerable.MaxBy(forecast => forecast.TemperatureC));
    }

    [HttpGet("Resilient/Slow")]
    public async Task<IActionResult> GetResilientSlowApiCallAsync()
    {

        HttpClient httpClient = _httpClientFactory.CreateClient("resilient");
        HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, _apiUrl + "/WeatherForecast/slow");
        HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
        LogSentApiRequest(_logger, response.StatusCode);

        response.EnsureSuccessStatusCode();

        IEnumerable<WeatherForecast>? weatherForecastEnumerable = await response.Content.ReadFromJsonAsync<IEnumerable<WeatherForecast>>();
        if (weatherForecastEnumerable == null)
        {
            return NoContent();
        }

        return Ok(weatherForecastEnumerable.MaxBy(forecast => forecast.TemperatureC));
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "Received response of {statusCode} from secondary API within {endpoint}")]
    private static partial void LogSentApiRequest(ILogger logger, HttpStatusCode statusCode, [CallerMemberName] string endpoint = "");
}
