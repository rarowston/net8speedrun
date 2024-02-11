using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;

namespace net8Speedrun.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public partial class DataInteractionController : ControllerBase
    {
        private static string Data = "Default";
        private static int Number = 0;
        private readonly ILogger<DataInteractionController> _logger;

        public DataInteractionController(ILogger<DataInteractionController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetInt")]
        public int GetInt()
        {
            return Number;
        }

        [HttpGet("GetString")]
        public string GetString()
        {
            return Data;
        }

        [HttpPost("SetString")]
        public bool SetString([FromBody] string newData)
        {
            if (string.IsNullOrWhiteSpace(newData))
            {
                return false;
            }
            else
            {
                Data = newData;
                return true;
            }
        }

        [HttpPost("SetInt")]
        public bool SetInt([FromBody] int newInt)
        {
            LogRequest(_logger);
            Number = newInt;
            return true;
        }

        [LoggerMessage(Level = LogLevel.Information, Message = "{endpoint} has been hit")]
        private static partial void LogRequest(ILogger logger, [CallerMemberName] string endpoint = "");

        /* #region WithModel */

        private static WeatherForecast? Forecast;

        [HttpGet("GetForecast")]
        public WeatherForecast? GetForecast()
        {
            LogRequestWithForecast(_logger, Forecast);
            return Forecast;
        }

        [HttpPost("SetForecast")]
        public bool SetForecast([FromBody] WeatherForecast newForecast)
        {
            LogRequestWithForecast(_logger, newForecast);
            Forecast = newForecast;
            return true;
        }

        [LoggerMessage(Level = LogLevel.Information, Message = "{endpoint} has been hit ")]
        private static partial void LogRequestWithForecast(ILogger logger, [LogProperties] WeatherForecast? forecast, [CallerMemberName] string endpoint = "");

        /* #endregion */
    }
}
