namespace Interapptive.Shared.Threading
{
    /// <summary>
    /// Update the progress of a progress item
    /// </summary>
    public interface IProgressUpdater
    {
        /// <summary>
        /// Update the progress
        /// </summary>
        void Update();
    }
}