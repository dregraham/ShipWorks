using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.UI.Controls.Html.Core;
using System.Drawing;
using Interapptive.Shared;
using Interapptive.Shared.Win32;

namespace ShipWorks.UI.Controls.Html
{
    /// <summary>
    /// Delegate for moving and sizing events
    /// </summary>
    public delegate void ElementSnapRectEventHandler(object sender, ElementSnapRectEventArgs e);

    /// <summary>
    /// EventArgs for the ElementMoving and ElementSizing event
    /// </summary>
    public class ElementSnapRectEventArgs : EventArgs
    {
        HtmlApi.IHTMLElement element;

        Rectangle currentRect;
        Rectangle newRect;

        /// <summary>
        /// Constructor
        /// </summary>
        public ElementSnapRectEventArgs(HtmlApi.IHTMLElement element, NativeMethods.RECT newRect)
        {
            this.element = element;
            this.newRect = new Rectangle(newRect.left, newRect.top, newRect.right - newRect.left, newRect.bottom - newRect.top);
            this.currentRect = new Rectangle(element.OffsetLeft, element.OffsetTop, element.OffsetWidth, element.OffsetHeight);
        }

        /// <summary>
        /// The element that is being moved.
        /// </summary>
        public HtmlApi.IHTMLElement Element
        {
            get { return element; }
        }

        /// <summary>
        /// The current position of the elemtn before being changed
        /// </summary>
        public Rectangle CurrentRect
        {
            get
            {
                return currentRect;
            }
        }

        /// <summary>
        /// The new position of the element
        /// </summary>
        public Rectangle NewRect
        {
            get
            {
                return newRect;
            }
            set
            {
                newRect = value;
            }
        }
    }
}
