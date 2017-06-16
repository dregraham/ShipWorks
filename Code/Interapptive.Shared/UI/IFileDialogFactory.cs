namespace Interapptive.Shared.UI
{
    /// <summary>
    /// Interface that represents a factory for creating file dialogs
    /// </summary>
    public interface IFileDialogFactory
    {
        /// <summary>
        /// Create a Save File Dialog
        /// </summary>
        /// <returns></returns>
        ISaveFileDialog CreateSaveFileDialog();

        /// <summary>
        /// Create a Open File Dialog
        /// </summary>
        IOpenFileDialog CreateOpenFileDialog();
    }
}