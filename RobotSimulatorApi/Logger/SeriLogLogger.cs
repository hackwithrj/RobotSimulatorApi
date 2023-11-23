// SerilogLogger.cs
using Serilog;

namespace RobotSimulator.Logger
{
    public class SerilogLogger : ISeriLogger
    {
        private readonly Serilog.ILogger logger;
        public SerilogLogger()
        {
            logger = new LoggerConfiguration().WriteTo
                .Console()
                .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }
        public void LogInformation(string message)
        {
            logger.Information(message);
        }

        public void LogError(string message)
        {
            logger.Error(message);
        }
    }
}
