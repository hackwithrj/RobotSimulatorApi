using System;

namespace RobotSimulator.Exceptions
{
    public class InvalidCommandException : Exception
    {
        public InvalidCommandException(string message)
            : base(message) { }
    }

    public class InvalidDirectionException : Exception
    {
        public InvalidDirectionException(string message)
            : base(message) { }
    }

    public class FileNotFoundException : Exception
    {
        public FileNotFoundException(string message)
            : base(message) { }
    }
}
