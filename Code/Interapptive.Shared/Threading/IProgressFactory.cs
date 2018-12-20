namespace Interapptive.Shared.Threading
{
    /// <summary>
    /// Factory for progress related objects
    /// </summary>
    public interface IProgressFactory
    {
        /// <summary>
        /// Create a progress reporter
        /// </summary>
        IProgressReporter CreateReporter(string name);
    }
}
