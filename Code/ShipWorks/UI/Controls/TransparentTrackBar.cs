using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms.VisualStyles;
using Interapptive.Shared.Win32;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// TrackBar that draws its background transparently
    /// </summary>
    public class TransparentTrackBar : TrackBar
    {
        TrackBarThumbState thumbState = TrackBarThumbState.Normal;

        bool isDragging = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public TransparentTrackBar()
        {
            if (TrackBarRenderer.IsSupported)
            {
                SetStyle(ControlStyles.UserPaint, true);
                SetStyle(ControlStyles.AllPaintingInWmPaint, true);
                SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            }
        }

        /// <summary>
        /// Custom painting
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            using (BufferedGraphics bg = BufferedGraphicsManager.Current.Allocate(e.Graphics, e.ClipRectangle))
            {
                Graphics g = bg.Graphics;

                // Draw the background
                IntPtr hdc = g.GetHdc();
                DrawParentBackground(hdc);
                g.ReleaseHdc();

                // Draw the ticks and track
                if (Orientation == Orientation.Horizontal)
                {
                    TrackBarRenderer.DrawHorizontalTrack(g, ChannelBounds);
                }
                else
                {
                    TrackBarRenderer.DrawVerticalTrack(g, ChannelBounds);
                }

                // Draw the thumb
                TrackBarRenderer.DrawBottomPointingThumb(g, ThumbBounds,
                    Enabled ? thumbState : TrackBarThumbState.Disabled);

                bg.Render();
            }
        }

        /// <summary>
        /// Mouse is moving
        /// </summary>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            UpdateThumbState(e.Location);
        }

        /// <summary>
        /// Mouse is down
        /// </summary>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (ThumbBounds.Contains(e.Location))
            {
                isDragging = true;
            }

            UpdateThumbState(e.Location);
        }

        /// <summary>
        /// Mouse is up
        /// </summary>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            isDragging = false;

            UpdateThumbState(e.Location);
        }

        /// <summary>
        /// Update the state of the thumb
        /// </summary>
        private void UpdateThumbState(Point point)
        {
            if (!TrackBarRenderer.IsSupported)
            {
                return;
            }

            TrackBarThumbState newState = TrackBarThumbState.Normal;

            if (isDragging)
            {
                newState = TrackBarThumbState.Pressed;
            }
            else
            {
                if (ThumbBounds.Contains(point))
                {
                    newState = TrackBarThumbState.Hot;
                }
            }

            if (newState != thumbState)
            {
                thumbState = newState;

                Invalidate();
            }
        }

        /// <summary>
        /// Get the bounds of the Thumb
        /// </summary>
        private Rectangle ThumbBounds
        {
            get
            {
                return GetPartBounds(NativeMethods.TBM_GETTHUMBRECT);
            }
        }

        /// <summary>
        /// Get the bounds of the channel area
        /// </summary>
        private Rectangle ChannelBounds
        {
            get
            {
                return GetPartBounds(NativeMethods.TBM_GETCHANNELRECT);
            }
        }

        /// <summary>
        /// Get the bounds of the given TrackBar part
        /// </summary>
        private Rectangle GetPartBounds(int part)
        {
            NativeMethods.RECT rect = new NativeMethods.RECT();
            NativeMethods.SendMessage(Handle, part, IntPtr.Zero, ref rect);

            return Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom);
        }

        /// <summary>
        /// Draw the parent background of this control to the given HDC
        /// </summary>
        private void DrawParentBackground(IntPtr hdc)
        {
            // Offset the viewport for the parent to paint the section right under us
            NativeMethods.POINT oldOrg = new NativeMethods.POINT();
            NativeMethods.SetViewportOrgEx(hdc, -Left, -Top, ref oldOrg);

            // Tell the parent to paint
            NativeMethods.SendMessage(Parent.Handle, NativeMethods.WM_PRINTCLIENT, hdc, (IntPtr) NativeMethods.PRF_CLIENT);

            // Put the viewport back
            NativeMethods.SetViewportOrgEx(hdc, oldOrg.x, oldOrg.y, ref oldOrg);
        }
    }
}
