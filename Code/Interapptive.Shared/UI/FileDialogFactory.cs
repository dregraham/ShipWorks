using System;
using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;

namespace Interapptive.Shared.UI
{
    /// <summary>
    /// Factory for creating file dialogs
    /// </summary>
    [Component]
    public class FileDialogFactory : IFileDialogFactory
    {
        private readonly Func<Control> ownerFunc;

        /// <summary>
        /// Constructor
        /// </summary>
        public FileDialogFactory(Func<Control> ownerFunc)
        {
            this.ownerFunc = ownerFunc;
        }

        /// <summary>
        /// Create a new Save File Dialog
        /// </summary>
        public ISaveFileDialog CreateSaveFileDialog()
        {
            return new ShipWorksSaveFileDialog(ownerFunc);
        }

        /// <summary>
        /// Create a new Open File Dialog
        /// </summary>
        public IOpenFileDialog CreateOpenFileDialog()
        {
            return new ShipWorksOpenFileDialog(ownerFunc);
        }
    }
}