namespace Interapptive.Shared.IO.Hardware.Scales
{
    /// <summary>
    /// The result of trying to read from a scale
    /// </summary>
    public struct ScaleReadResult
    {
        const double noValue = double.NegativeInfinity;

        /// <summary>
        /// Constructor for a problem
        /// </summary>
        private ScaleReadResult(ScaleReadStatus status, double weight, string message, 
            ScaleType scaleType, double length, double width, double height)
        {
            Status = status;
            Message = message;
            Weight = weight;
            ScaleType = scaleType;
            Length = length;
            Width = width;
            Height = height;
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
        /// The length read from the scale, of status is Success
        /// </summary>
        public double Length { get; }
        /// <summary>
        /// The width read from the scale, of status is Success
        /// </summary>
        public double Width { get; }
        /// <summary>
        /// The height read from the scale, of status is Success
        /// </summary>
        public double Height { get; }
        /// <summary>
        /// The error message, if the status is not Success
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Type of scale that generated the reading
        /// </summary>
        public ScaleType ScaleType { get; }

        /// <summary>
        /// Returns true when length, width, and height have values
        /// </summary>
        public bool HasVolumeDimensions =>
            Length != noValue && Width != noValue && Height != noValue;

        /// <summary>
        /// Success with just weight
        /// </summary>
        public static ScaleReadResult Success(double weight, ScaleType scaleType) =>
            Success(weight, noValue, noValue, noValue, scaleType);

        /// <summary>
        /// Get a successful read result
        /// </summary>
        public static ScaleReadResult Success(double weight, double length, double width, double height, ScaleType scaleType) =>
            new ScaleReadResult(ScaleReadStatus.Success, weight, null, scaleType, length, width, height);

        /// <summary>
        /// Get a not found read result
        /// </summary>
        public static ScaleReadResult NotFound(string message) =>
            Error(ScaleReadStatus.NotFound, message, ScaleType.None);

        /// <summary>
        /// Get an error read result
        /// </summary>
        public static ScaleReadResult ReadError(string message, ScaleType scaleType) =>
            Error(ScaleReadStatus.ReadError, message, scaleType);

        /// <summary>
        /// Get a no status result
        /// </summary>
        public static ScaleReadResult NoStatus() =>
            Error(ScaleReadStatus.NoStatus, string.Empty, ScaleType.None);
            
        /// <summary>
        /// Helper class because all the dimensions are NegativeInfinity when we encounter an error
        /// </summary>
        private static ScaleReadResult Error(ScaleReadStatus status, string message, ScaleType scaleType) =>
            new ScaleReadResult(status, noValue, message, scaleType, 
                noValue, noValue, noValue);
    }
}
