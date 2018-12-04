using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using Microsoft.Win32;

namespace Interapptive.Shared.IO
{
    /// <summary>
    /// Select files
    /// </summary>
    [Component]
    public class FileSelector : IFileSelector
    {
        /// <summary>
        /// Get a file path to open
        /// </summary>
        public GenericResult<string> GetFilePathToOpen(string filter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = filter
            };

            return openFileDialog.ShowDialog() == true ?
                openFileDialog.FileName :
                GenericResult.FromError<string>("Selection canceled");
        }
    }
}
