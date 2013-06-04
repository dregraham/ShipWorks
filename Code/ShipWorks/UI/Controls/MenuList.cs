using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Divelements.SandRibbon.Rendering;
using System.Diagnostics;
using System.Drawing;
using Interapptive.Shared;
using System.Drawing.Drawing2D;
using ShipWorks.ApplicationCore.Appearance;
using Interapptive.Shared.Win32;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// Provides a list that looks like a menu for selecting settings\options\etc. sections.
    /// </summary>
    public class MenuList : ListBox
    {
        int hoverIndex = -1;

        /// <summary>
        /// Constructor
        /// </summary>
        public MenuList()
        {
            this.DrawMode = DrawMode.OwnerDrawFixed;
            this.ItemHeight = 26;

            SetStyle(ControlStyles.ResizeRedraw, true);
        }

        /// <summary>
        /// Handle low level windows messages
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == NativeMethods.WM_ERASEBKGND)
            {
                PaintBackground(m);
                return;
            }

            base.WndProc(ref m);
        }

        /// <summary>
        /// Custom paint the background to avoid flickering
        /// </summary>
        private void PaintBackground(Message m)
        {
            IntPtr dc = m.WParam;
            if (dc == IntPtr.Zero)
            {
                m.Result = IntPtr.Zero;
                return;
            }

            int adjustedTop = Items.Count * ItemHeight - AutoScrollOffset.Y;
            if (adjustedTop <= Bottom)
            {
                Graphics g = Graphics.FromHdcInternal(dc);
                g.PageUnit = GraphicsUnit.Pixel;

                Rectangle bounds = new Rectangle(0, adjustedTop, Width, Bottom - adjustedTop);
                g.FillRectangle(SystemBrushes.Window, bounds);

                g.Dispose();
            }

            m.Result = (IntPtr) 1;
        }

        /// <summary>
        /// Overridden to draw each item.
        /// </summary>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (Items.Count == 0)
            {
                base.OnDrawItem(e);
                return;
            }

            // The text to draw
            string text = GetItemText(Items[e.Index]);

            // Shrink the bounds
            Rectangle bounds = e.Bounds;
            bounds.Inflate(-1, -1);

            using (RibbonRenderer renderer = AppearanceHelper.CreateRibbonRenderer())
            {
                using (BufferedGraphics bg = BufferedGraphicsManager.Current.Allocate(e.Graphics, e.Bounds))
                {
                    // Use original bounds here
                    bg.Graphics.FillRectangle(SystemBrushes.Window, e.Bounds);

                    using (RenderingContext context = AppearanceHelper.CreateRibbonRenderingContext(bg.Graphics, renderer, Font, false, renderer.ColorTable.RibbonText, renderer.ColorTable.RibbonDisabledText))
                    {
                        // Draw selected
                        if ((e.State & DrawItemState.Selected) != 0)
                        {
                            // Its hovered
                            if (hoverIndex == e.Index)
                            {
                                renderer.DrawStandaloneHighlight(context, bounds, 1.0, .8, DrawState.Normal);
                            }
                            // Normal
                            else
                            {
                                if (Focused)
                                {
                                    renderer.DrawStandaloneHighlight(context, bounds, 1.0, 0.9, DrawState.Pushed);
                                }
                                else
                                {
                                    renderer.DrawStandaloneHighlight(context, bounds, 1.0, 0.5, DrawState.Normal);
                                }
                            }

                        }
                        // Draw hovered
                        else if (hoverIndex == e.Index)
                        {
                            renderer.DrawStandaloneHighlight(context, bounds, 1.0, 0, DrawState.Normal);
                        }

                        Rectangle textBounds = bounds;
                        textBounds.Offset(10, 0);

                        renderer.DrawText(context, textBounds, text, context.ControlFont, context.StandardNearsideText, DrawState.Normal, context.DefaultTextColor);
                    }

                    bg.Render();
                }
            }
        }

        /// <summary>
        /// Track mouse movements
        /// </summary>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            int newHoverIndex = this.IndexFromPoint(this.PointToClient(Control.MousePosition));

            if (newHoverIndex != hoverIndex)
            {
                int oldHoverIndex = hoverIndex;
                hoverIndex = newHoverIndex;

                if (oldHoverIndex != -1)
                {
                    Invalidate(GetItemRectangle(oldHoverIndex));
                }

                if (hoverIndex != -1)
                {
                    Invalidate(GetItemRectangle(hoverIndex));
                }
            }
        }

        /// <summary>
        /// The mouse is leaving the bounds of the control
        /// </summary>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            // Ensure the hover look goes away
            if (hoverIndex != -1)
            {
                int oldHoverIndex = hoverIndex;
                hoverIndex = -1;

                Invalidate(GetItemRectangle(oldHoverIndex));
            }
        }
    }
}
