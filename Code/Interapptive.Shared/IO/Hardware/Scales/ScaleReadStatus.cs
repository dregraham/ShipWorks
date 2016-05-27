namespace Interapptive.Shared.IO.Hardware.Scales
{
    /// <summary>
    /// The status of reading a scale
    /// </summary>
    public enum ScaleReadStatus
    {
        /// <summary>
        /// No status was retrieved from the scale.
        /// </summary>
        /// <remarks>
        /// If this status is returned, assume that none of the properties are valid.
        /// </remarks>
        NoStatus,

        /// <summary>
        /// The scale was successfully read
        /// </summary>
        Success,

        /// <summary>
        /// No scale was found to read
        /// </summary>
        NotFound,

        /// <summary>
        /// A scale was found, but there was a problem reading it
        /// </summary>
        ReadError
    }
}
