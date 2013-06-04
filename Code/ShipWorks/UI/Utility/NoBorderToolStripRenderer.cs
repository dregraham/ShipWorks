using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.UI.Utility
{
    /// <summary>
    /// Renderer to remove the bottom border from the ToolBar
    /// </summary>
    public class NoBorderToolStripRenderer : ToolStripSystemRenderer
    {
        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {

        }
    }
}
