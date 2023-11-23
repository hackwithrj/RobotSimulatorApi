namespace RobotSimulator.Contracts
{
    public interface IRobotService
    {
        int X { get; set; }
        int Y { get; set; }
        string Facing { get; set; }
        bool IsPlaced { get; set; }
        public string reportValue { get; set; }

        /// <summary>
        /// The function `ProcessCommand` takes in a command and performs the
        /// corresponding action based on the command.
        /// </summary>
        /// <param name="command">The command is a string that represents the action to be performed by
        /// the robot. It can be one of the following commands: "PLACE", "MOVE", "LEFT", "RIGHT", or
        /// "REPORT".</param>
        public void ProcessCommand(string command);
        /// <summary>
        /// The Place function sets the position and direction of an object and marks it as placed.
        /// </summary>
        /// <param name="x">The x-coordinate of the position where the object is being placed.</param>
        /// <param name="y">The y parameter represents the vertical position or coordinate of the object
        /// being placed.</param>
        /// <param name="facing">The "Direction" parameter is an enumeration type that represents the
        /// direction the object is facing. It can have values such as "North", "South", "East", or
        /// "West".</param>
        void Place(int x, int y, string facing);

        /// <summary>
        /// The Move function updates the position of an object based on its current direction.
        /// </summary>
        /// <returns>
        /// If the robot is not placed on the board, the method will return without performing any
        /// actions.
        /// </returns>
        void Move();
        /// <summary>
        /// The TurnLeft function updates the direction the object is facing by rotating it 90 degrees
        /// counterclockwise.
        /// </summary>
        void TurnLeft();
        /// <summary>
        /// The TurnRight function updates the direction the object is facing by incrementing it by 1
        /// and wrapping around to 0 if it exceeds 3.
        /// </summary>
        void TurnRight();
        /// <summary>
        /// The Report function assigns value to the report class variable.
        /// </summary>
        void Report();
    }
}
