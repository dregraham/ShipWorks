namespace Interapptive.Shared.IO.Hardware.Scales
{
    /// <summary>
    /// The result of trying to read from a scale
    /// </summary>
    public struct ScaleReadResult
    {
        /// <summary>
        /// Constructor for a problem
        /// </summary>
        private ScaleReadResult(ScaleReadStatus status, double weight, string message)
        {
            Status = status;
            Message = message;
            Weight = weight;
        }

        /// <summary>
        /// The status of reading the scale.  Indicates success or reason for failure.
        /// </summary>
        public ScaleReadStatus Status { get; }

        /// <summary>
        /// The weight read from the scale, of status is Success
        /// </summary>
        public double Weight { get; }

        /// <summary>
        /// The error message, if the status is not Success
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Get a successful read result
        /// </summary>
        public static ScaleReadResult Success(double weight) =>
            new ScaleReadResult(ScaleReadStatus.Success, weight, null);

        /// <summary>
        /// Get a not found read result
        /// </summary>
        public static ScaleReadResult NotFound(string message) =>
            new ScaleReadResult(ScaleReadStatus.NotFound, double.NegativeInfinity, message);

        /// <summary>
        /// Get an error read result
        /// </summary>
        public static ScaleReadResult ReadError(string message) =>
            new ScaleReadResult(ScaleReadStatus.ReadError, double.NegativeInfinity, message);
    }
}
