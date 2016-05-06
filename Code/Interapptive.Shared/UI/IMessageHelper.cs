using System;
using System.Windows;
using System.Windows.Forms;

namespace Interapptive.Shared.UI
{
    /// <summary>
    /// Display messages to the user without taking a dependency on the UI
    /// </summary>
    public interface IMessageHelper
    {
        /// <summary>
        /// Show an error
        /// </summary>
        void ShowError(string message);

        /// <summary>
        /// Show an information message
        /// </summary>
        void ShowInformation(string message);

        /// <summary>
        /// Show an error, takes an owner
        /// </summary>
        void ShowError(IWin32Window owner, string message);

        /// <summary>
        /// Show an information message, takes an owner
        /// </summary>
        void ShowInformation(IWin32Window owner, string message);
    }
}