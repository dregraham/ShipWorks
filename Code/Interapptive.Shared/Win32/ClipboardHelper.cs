using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Interapptive.Shared.UI;
using log4net;

namespace Interapptive.Shared.Win32
{
    /// <summary>
    /// Wrapper class around the Clipboard for help with not crashing
    /// </summary>
    public static class ClipboardHelper
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ClipboardHelper));

        /// <summary>
        /// Wraps Clipboard.SetText to handle and display exceptions due to clipboard being locked.
        /// </summary>
        public static void SetText(string text, TextDataFormat format, IWin32Window owner)
        {
            try
            {
                Clipboard.SetText(text, format);
            }
            catch (ExternalException ex)
            {
                log.Error("Clipboard.SetText", ex);

                MessageHelper.ShowError(owner, "Could not copy because the clipboard is in use by another application.");
            }
        }
    }
}
