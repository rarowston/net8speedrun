using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace net8Speedrun.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DataInteractionOldController : ControllerBase
    {
        private static string Data = "Default";
        private static int Number = 0;
        private readonly ILogger<DataInteractionController> _logger;

        public DataInteractionOldController(ILogger<DataInteractionController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetInt")]
        public int GetInt()
        {
            _logger.LogInformation("Int was retrieved via API as {number}", Number);
            return Number;
        }

        [HttpGet("GetString")]
        public string GetString()
        {
            _logger.LogInformation("String was retrieved via API as {data}", Data);
            return Data;
        }

        [HttpPost("SetString")]
        public bool SetString([FromBody] string newData)
        {
            if (string.IsNullOrWhiteSpace(newData))
            {
                _logger.LogWarning("Setting string via API to {newData}", newData);
                return false;
            }
            else
            {
                _logger.LogInformation("Setting string via API to {newData}", newData);
                Data = newData;
                return true;
            }
        }

        [HttpPost("SetInt")]
        public bool SetInt([FromBody] int newInt)
        {
            _logger.LogInformation("Setting int via API to {newInt}", newInt);

            Number = newInt;
            return true;
        }
    }
}
