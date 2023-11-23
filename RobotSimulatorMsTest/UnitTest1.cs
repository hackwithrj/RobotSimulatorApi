using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RobotSimulator.Contracts;
using RobotSimulator.Controllers;
using RobotSimulator.Exceptions;
using RobotSimulator.Logger;
using RobotSimulator.Services;

namespace RobotSimulatorMsTest
{
    [TestClass]
    public class RobotSimTest
    {
        private IRobotService _robotService;
        private RobotController _robotController;
        private SerilogLogger _logger;

        public RobotSimTest()
        {
            _logger = new SerilogLogger();
            _robotService = new RobotService(_logger);
            _robotController = new RobotController(_robotService, _logger);
        }

        [TestMethod]
        public void TestIsPlaced()
        {
            _robotService.ProcessCommand("PLACE 0,0,NORTH");
            Assert.AreEqual(_robotService.IsPlaced, true);
        }

        [TestMethod]
        public void TestInValidDirection()
        {
            Action act = () =>
            {
                _robotService.ProcessCommand("PLACE 0,0,NORTHH");
            };
            Assert.ThrowsException<InvalidDirectionException>(act);
        }

        [TestMethod]
        public void TestInValidCommand()
        {
            Action act = () =>
            {
                _robotService.ProcessCommand("CENTRE");
            };
            Assert.ThrowsException<InvalidCommandException>(act);
        }

        [TestMethod]
        public void PostFile_ReturnsOkResult()
        {
            StringBuilder commandBuilder = new StringBuilder();
            string[] commands = {
                "PLACE 0,0,NORTH",
                "MOVE",
                "REPORT"
            };
            foreach (string command in commands)
            {
                commandBuilder.AppendLine(command);
            }
            var memoryStream = new MemoryStream();
            using (var writer = new StreamWriter(memoryStream, Encoding.UTF8, 1024, true))
            {
                writer.Write(commandBuilder.ToString());
                writer.Flush();
            }
            memoryStream.Position = 0;

            IFormFile file = new FormFile(memoryStream, 0, memoryStream.Length, "id_from_form", "test.txt");

            var result = _robotController.PostFile(file) as FileContentResult;

            using (MemoryStream memStream = new MemoryStream(result.FileContents))
            using (StreamReader streamReader = new StreamReader(memStream, Encoding.UTF8))
            {
                string text = streamReader.ReadToEnd();
                Assert.AreEqual("0,1,NORTH", text.Trim());
            }

            Assert.AreEqual("text/plain", result.ContentType);
            Assert.AreEqual("robot_report.txt", result.FileDownloadName);
        }

        [TestMethod]
        public void PostFile_AllConditionsReturnsOkResult()
        {
            StringBuilder commandBuilder = new StringBuilder();
            string[] commands = {
                "PLACE 1,2,EAST",
                "MOVE",
                "MOVE",
                "LEFT",
                "MOVE",
                "REPORT"
            };
            foreach (string command in commands)
            {
                commandBuilder.AppendLine(command);
            }
            var memoryStream = new MemoryStream();
            using (var writer = new StreamWriter(memoryStream, Encoding.UTF8, 1024, true))
            {
                writer.Write(commandBuilder.ToString());
                writer.Flush();
            }
            memoryStream.Position = 0;

            IFormFile file = new FormFile(memoryStream, 0, memoryStream.Length, "id_from_form", "test.txt");

            var result = _robotController.PostFile(file) as FileContentResult;

            using (MemoryStream memStream = new MemoryStream(result.FileContents))
            using (StreamReader streamReader = new StreamReader(memStream, Encoding.UTF8))
            {
                string text = streamReader.ReadToEnd();
                Assert.AreEqual("3,3,NORTH", text.Trim());
            }

            Assert.AreEqual("text/plain", result.ContentType);
            Assert.AreEqual("robot_report.txt", result.FileDownloadName);
        }
    }
}
