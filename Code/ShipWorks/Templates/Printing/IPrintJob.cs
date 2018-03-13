using System.Windows.Forms;

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
        /// Preview the print job
        /// </summary>
        void PreviewAsync(Form parent);

        /// <summary>
        /// Print the print job
        /// </summary>
        void PrintAsync();
    }
}
