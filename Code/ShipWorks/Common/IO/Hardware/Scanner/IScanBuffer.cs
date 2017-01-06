namespace ShipWorks.Common.IO.Hardware.Scanner
{
    /// <summary>
    /// Buffer characters that make up a scan
    /// </summary>
    public interface IScanBuffer
    {
        /// <summary>
        /// Append input to the current scan
        /// </summary>
        void Append(string input);
    }
}