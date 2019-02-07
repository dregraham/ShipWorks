using System.Windows.Forms;
using ShipWorks.Common.Threading;

namespace ShipWorks.Templates.Printing
{
    /// <summary>
    /// Represents a print job
    /// </summary>
    public interface IPrintJob
    {
        /// <summary>
        /// Event for the preview completing
        /// </summary>
        event PrintActionCompletedEventHandler PreviewCompleted;

        /// <summary>
        /// Raised when printing is complete.
        /// </summary>
        event PrintActionCompletedEventHandler PrintCompleted;

        /// <summary>
        /// Raised when the preview window becomes visible
        /// </summary>
        event PrintPreviewShownEventHandler PreviewShown;

        /// <summary>
        /// Exposes the current set of operations the print job is working on and their status.
        /// </summary>
        ProgressProvider ProgressProvider { get; }

        /// <summary>
        /// Preview the print job
        /// </summary>
        void PreviewAsync(Form parent);

        /// <summary>
        /// Preview the job asynchronously.  The userState is passed when the PreviewCompleted callback is invoked.
        /// </summary>
        void PreviewAsync(Form parent, object userState);

        /// <summary>
        /// Print the print job
        /// </summary>
        void PrintAsync();

        /// <summary>
        /// Print the job asynchronously.  The userState is passed when the PrintCompleted callback is invoked.
        /// </summary>
        void PrintAsync(object userState);
    }
}
