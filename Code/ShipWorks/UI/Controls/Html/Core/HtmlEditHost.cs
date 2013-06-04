using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using System.Drawing;
using Interapptive.Shared;
using Interapptive.Shared.Win32;

namespace ShipWorks.UI.Controls.Html.Core
{
	/// <summary>
	/// Class that allows hooking into element movements
	/// </summary>
	[ComVisible(true)]
	public class HtmlEditHost : HtmlApi.IHTMLEditHost 
    {
        public event ElementSnapRectEventHandler ElementMoving;
        public event ElementSnapRectEventHandler ElementSizing;

        /// <summary>
        /// Implements IHTMLEditHost interface for hooking element movment and sizing.
        /// </summary>
		public void SnapRect(HtmlApi.IHTMLElement pIElement, ref NativeMethods.RECT prcNew, HtmlApi.ELEMENT_CORNER eHandle) 
        {
            ElementSnapRectEventArgs eventArgs = new ElementSnapRectEventArgs(pIElement, prcNew);

            if (eHandle == HtmlApi.ELEMENT_CORNER.ELEMENT_CORNER_NONE)
            {
                if (ElementMoving != null)
                {
                    ElementMoving(this, eventArgs);
                }
            }
            else
            {
                if (ElementSizing != null)
                {
                    ElementSizing(this, eventArgs);
                }
            }

            prcNew.left = eventArgs.NewRect.Left;
            prcNew.right = eventArgs.NewRect.Right;
            prcNew.top = eventArgs.NewRect.Top;
            prcNew.bottom = eventArgs.NewRect.Bottom;
        }
	}
}
