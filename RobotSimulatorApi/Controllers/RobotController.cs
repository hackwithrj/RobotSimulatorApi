// Controllers/CommandsController.cs
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotSimulator.Contracts;
using RobotSimulator.Logger;
using System.IO;
using System.Net.Http.Headers;
using System.Text;

namespace RobotSimulator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RobotController : ControllerBase
    {
        private readonly IRobotService _robotService;
        private readonly ISeriLogger _logger;

        public RobotController(IRobotService robotService, ISeriLogger logger)
        {
            _robotService = robotService;
            _logger = logger;
        }

        [HttpPost]
        [Route("/api/robot-simulator")]
        public IActionResult PostFile(IFormFile file)
        {
            _logger.LogInformation("Entering post method");
            if (file == null || file.Length == 0)
            {
                _logger.LogError("File not provided.");
                return BadRequest("File not provided.");
            }

            using (var streamReader = new StreamReader(file.OpenReadStream()))
            {
                string[] commands = streamReader.ReadToEnd().Split("\r\n");

                foreach (string command in commands)
                {
                    if (!string.IsNullOrWhiteSpace(command))
                    {
                        _robotService.ProcessCommand(command);
                    }
                }

                string finalReport = _robotService.reportValue;
                if (string.IsNullOrWhiteSpace(finalReport))
                {
                    finalReport = "Error occured while generating report";
                }
                using (
                    MemoryStream memoryStream = new MemoryStream(
                        Encoding.UTF8.GetBytes(finalReport)
                    )
                )
                {
                    var response = new HttpResponseMessage
                    {
                        Content = new StreamContent(memoryStream)
                    };
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
                    _logger.LogInformation("Success response sent");
                    return File(memoryStream.ToArray(), "text/plain", "robot_report.txt");
                }
            }
        }
    }
}
