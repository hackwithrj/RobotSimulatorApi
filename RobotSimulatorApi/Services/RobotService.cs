using System;
using System.Linq.Expressions;
using RobotSimulator.Contracts;
using RobotSimulator.Exceptions;
using RobotSimulator.Logger;

namespace RobotSimulator.Services
{
    public class RobotService : IRobotService
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Facing { get; set; }
        public bool IsPlaced { get; set; }
        public string reportValue { get; set; }
        private readonly ISeriLogger _logger;

        // Dictionary to map direction names to their corresponding integer values
        private readonly Dictionary<string, int> directionValues = new Dictionary<string, int>
        {
            { Constants.NORTH, 0 },
            { Constants.EAST, 1 },
            { Constants.SOUTH, 2 },
            { Constants.WEST, 3 }
        };

        public RobotService(ISeriLogger logger)
        {
            _logger = logger;
            IsPlaced = false;
        }

        public void ProcessCommand(string command)
        {
            try
            {
                string[] parts = command.Split(' ');

                if (parts.Length > 0)
                {
                    string action = parts[0].ToUpper();
                    switch (action)
                    {
                        case Constants.PLACE:
                            if (parts.Length == 2)
                            {
                                string[] parameters = parts[1].Split(',');
                                if (parameters.Length == 3)
                                {
                                    int x = int.Parse(parameters[0]);
                                    int y = int.Parse(parameters[1]);
                                    string facing = parameters[2];

                                    // Validate facing direction
                                    if (!IsValidFacingDirection(facing))
                                    {
                                        _logger.LogError("$Invalid direction direction: {facing}");
                                        throw new InvalidDirectionException(
                                            $"Invalid direction direction: {facing}"
                                        );
                                    }
                                    Place(x, y, facing);
                                }
                            }
                            break;
                        case Constants.MOVE:
                            Move();
                            break;
                        case Constants.LEFT:
                            TurnLeft();
                            break;
                        case Constants.RIGHT:
                            TurnRight();
                            break;
                        case Constants.REPORT:
                            Report();
                            break;
                        default:
                            _logger.LogError($"Invalid command: {command}");
                            throw new InvalidCommandException($"Invalid command: {command}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        public void Place(int x, int y, string facing)
        {
            X = x;
            Y = y;
            Facing = facing;
            IsPlaced = true;
        }

        public void Move()
        {
            if (!IsPlaced)
                return;

            int facingValue = directionValues[Facing];
            switch (facingValue)
            {
                case 0: // NORTH
                    if (Y < 4)
                        Y++;
                    break;
                case 1: // EAST
                    if (X < 4)
                        X++;
                    break;
                case 2: // SOUTH
                    if (Y > 0)
                        Y--;
                    break;
                case 3: // WEST
                    if (X > 0)
                        X--;
                    break;
            }
        }

        public void TurnLeft()
        {
            if (IsPlaced)
            {
                int facingValue = directionValues[Facing];
                Facing = GetDirectionName((facingValue + 3) % 4);
            }
        }

        public void TurnRight()
        {
            if (IsPlaced)
            {
                int facingValue = directionValues[Facing];
                Facing = GetDirectionName((facingValue + 1) % 4);
            }
        }

        public void Report()
        {
            if (IsPlaced)
            {
                reportValue = $"{X},{Y},{Facing}";
                Console.WriteLine(reportValue);
            }
        }

        /// <summary>
        /// The function takes an integer value and returns the corresponding direction name from a
        /// dictionary.
        /// </summary>
        /// <param name="value">The value parameter is an integer value that represents a
        /// direction.</param>
        /// <returns>
        /// The method is returning a string value.
        /// </returns>
        private string GetDirectionName(int value)
        {
            foreach (var pair in directionValues)
            {
                if (pair.Value == value)
                {
                    return pair.Key;
                }
            }
            return null;
        }

        /// <summary>
        /// The function checks if a given facing direction is valid by comparing it to a list of valid
        /// directions.
        /// </summary>
        /// <param name="facing">A string representing a direction.</param>
        /// <returns>
        /// The method is returning a boolean value.
        /// </returns>
        private static bool IsValidFacingDirection(string facing)
        {
            return Array.IndexOf(Constants.ValidDirections, facing.ToUpper()) != -1;
        }
    }
}
