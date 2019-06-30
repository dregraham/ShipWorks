namespace Interapptive.Shared.Threading
{
    /// <summary>
    /// Update the progress of a progress item
    /// </summary>
    public interface IProgressUpdater
    {
        /// <summary>
        /// Update the progress by a single item
        /// </summary>
        void Update();

        /// <summary>
        /// Update the progress by the specified amount
        /// </summary>
        void Update(int finishedCount);
    }
}