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
    public class ClipboardHelper
    {
        private readonly IMessageHelper messageHelper;
        static readonly ILog log = LogManager.GetLogger(typeof(ClipboardHelper));

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="messageHelper"></param>
        public ClipboardHelper(IMessageHelper messageHelper)
        {
            this.messageHelper = messageHelper;
        }

        /// <summary>
        /// Wraps Clipboard.SetText to handle and display exceptions due to clipboard being locked.
        /// </summary>
        public void SetText(string text, TextDataFormat format, IWin32Window owner)
        {
            try
            {
                Clipboard.SetText(text, format);
            }
            catch (ExternalException ex)
            {
                log.Error("Clipboard.SetText", ex);

                messageHelper.ShowError("Could not copy because the clipboard is in use by another application.");
            }
        }
    }
}
