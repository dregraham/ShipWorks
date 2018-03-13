using System.Windows.Forms;

namespace ShipWorks.Templates.Printing
{
    /// <summary>
    /// Represents a print job
    /// </summary>
    public interface IPrintJob
    {
        /// <summary>
        /// Preview the print job
        /// </summary>
        void PreviewAsync(Form parent);

        event PrintActionCompletedEventHandler PreviewCompleted;

        void PrintAsync();
    }
}
