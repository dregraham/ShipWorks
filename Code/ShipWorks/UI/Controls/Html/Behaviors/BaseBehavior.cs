using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Interapptive.Shared;
using ShipWorks.UI.Controls.Html.Core;
using Interapptive.Shared.Win32;

namespace ShipWorks.UI.Controls.Html.Behaviors
{
	/// <summary>
	/// Base class for all behaviors we implement
	/// </summary>
    [ComVisible(true)]
	abstract public class BaseBehavior :  HtmlApi.IHTMLPainter, HtmlApi.IElementBehavior, HtmlApi.IElementBehaviorFactory
    {
        protected HtmlApi.IElementBehaviorSite behaviorSite;
        protected HtmlApi.IHTMLPaintSite paintSite;

        HtmlApi.HtmlPainter paintStyle;
        HtmlApi.HtmlZOrder zOrder;
        Rectangle borderMargin;

        /// <summary>
        /// Constructor
        /// </summary>
		protected BaseBehavior()
		{
            zOrder = HtmlApi.HtmlZOrder.ABOVE_CONTENT;
            paintStyle = HtmlApi.HtmlPainter.TRANSPARENT | HtmlApi.HtmlPainter.SUPPORTS_XFORM;
        }

        #region Base Functions

        /// <summary>
        /// Overriden for a base class to draw
        /// </summary>
        public virtual void Draw(int left, int top, int right, int bottom, Graphics g)
        {

        }

        public virtual Rectangle ExtraMargin 
        { 
            get { return borderMargin; }
            set { borderMargin = value; }
        }

        public virtual HtmlApi.HtmlPainter PaintStyle 
        {
            get { return paintStyle; }
            set { paintStyle = value; }
        }

        public virtual HtmlApi.HtmlZOrder ZOrder 
        {
            get { return zOrder; }
            set { zOrder = value; }
        }

        #endregion

        #region IElementBehavior

        /// <summary>
        /// Behavior is detatching from the element
        /// </summary>
        public void Detach()
        {

        }

        /// <summary>
        /// Initialize with the element site we are attached to
        /// </summary>
        public void Init(HtmlApi.IElementBehaviorSite pBehaviorSite)
        {
            behaviorSite = pBehaviorSite;
            paintSite = (HtmlApi.IHTMLPaintSite) this.behaviorSite;
        }

        /// <summary>
        /// Notify when the element and document are ready
        /// </summary>
        public void Notify(int dwEvent, IntPtr pVar)
        {

        }

        #endregion

        #region IElementBehaviorFactor

        /// <summary>
        /// Factory, we implement the IElementBehavior ourself
        /// </summary>
        public HtmlApi.IElementBehavior FindBehavior(string bstrBehavior, string bstrBehaviorUrl, HtmlApi.IElementBehaviorSite pSite)
        {
            return this;
        }

        #endregion

        #region IHTMLPainter

        [NDependIgnoreTooManyParams]
        public void Draw(
            int leftBounds, 
            int topBounds, 
            int rightBounds, 
            int bottomBounds, 
            int leftUpdate, 
            int topUpdate, 
            int rightUpdate, 
            int bottomUpdate, 
            int lDrawFlags, 
            IntPtr hdc, 
            IntPtr pvDrawObject)
        {
            using (Graphics g = Graphics.FromHdc(hdc))
            {
                g.PageUnit = GraphicsUnit.Pixel;

                this.Draw(leftBounds, topBounds, rightBounds, bottomBounds, g);
            }
        }

        /// <summary>
        /// Configure how the behavior is to be drawn
        /// </summary>
        public void GetPainterInfo(HtmlApi.HTML_PAINTER_INFO htmlPainterInfo)
        {
            if (htmlPainterInfo == null)
            {
                throw new ArgumentNullException("htmlPainterInfo");
            }

            htmlPainterInfo.lFlags = (int) (this.paintStyle);
            htmlPainterInfo.lZOrder = (int) this.zOrder;
            htmlPainterInfo.iidDrawObject = Guid.Empty;
            htmlPainterInfo.rcBounds = new NativeMethods.RECT(this.borderMargin.Left, this.borderMargin.Top, this.borderMargin.Width, this.borderMargin.Height);
        }

        /// <summary>
        /// Called to indicate a hit test
        /// </summary>
        public void HitTestPoint(int ptx, int pty, out int pbHit, out int plPartID)
        {
            pbHit = 0;
            plPartID = 0;
        }

        /// <summary>
        /// Respond to changes in size
        /// </summary>
        public void OnResize(int cx, int cy)
        {

        }

        #endregion
	}
}
