using Interapptive.Shared.Utility;

namespace Interapptive.Shared.IO
{
    /// <summary>
    /// Select a file
    /// </summary>
    public interface IFileSelector
    {
        /// <summary>
        /// Get a file path to open
        /// </summary>
        GenericResult<string> GetFilePathToOpen(string filter);

        /// <summary>
        /// Get a file path to save
        /// </summary>
        GenericResult<string> GetFilePathToSave(string filter);
    }
}
