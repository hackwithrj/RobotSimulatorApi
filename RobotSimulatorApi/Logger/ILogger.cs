namespace RobotSimulator.Logger
{
    public interface ISeriLogger
    {
        // Log Information
        void LogInformation(string message);
        // Log Error
        void LogError(string message);
    }
}
