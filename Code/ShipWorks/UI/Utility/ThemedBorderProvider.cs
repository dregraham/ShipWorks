using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Forms.VisualStyles;
using Interapptive.Shared.Win32;

namespace ShipWorks.UI.Utility
{
    /// <summary>
    /// Provides a themed border for the owner control.
    /// </summary>
    public class ThemedBorderProvider : NativeWindow
    {
        Control target;

        /// <summary>
        /// Constructor
        /// </summary>
        public ThemedBorderProvider(Panel target)
            : this((Control) target)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ThemedBorderProvider(UserControl target)
            : this((Control) target)
        {

        }

        /// <summary>
        /// Apply themed border to the given target
        /// </summary>
        public static void Apply(Panel target)
        {
            ThemedBorderProvider tbp = new ThemedBorderProvider(target);
        }

        /// <summary>
        /// Apply themed border to the given target
        /// </summary>
        public static void Apply(UserControl target)
        {
            ThemedBorderProvider tbp = new ThemedBorderProvider(target);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        private ThemedBorderProvider(Control target)
        {
            this.target = target;

            target.HandleCreated += new EventHandler(this.OnHandleCreated);
            target.HandleDestroyed += new EventHandler(this.OnHandleDestroyed);
            target.Resize += new EventHandler(OnTargetResized);

            if (target.IsHandleCreated)
            {
                OnHandleCreated(target, EventArgs.Empty);
            }
        }

        /// <summary>
        /// The target control has been resized
        /// </summary>
        void OnTargetResized(object sender, EventArgs e)
        {
            target.Invalidate();
        }

        /// <summary>
        /// Indicates if the themed border should be drawn
        /// </summary>
        private bool ThemedBorderEnabled
        {
            get
            {
                if (!ThemeInformation.VisualStylesEnabled)
                {
                    return false;
                }

                Panel panel = target as Panel;
                if (panel != null)
                {
                    return panel.BorderStyle == BorderStyle.Fixed3D;
                }

                UserControl userControl = target as UserControl;
                if (userControl != null)
                {
                    return userControl.BorderStyle == BorderStyle.Fixed3D;
                }

                return true;
            }
        }

        /// <summary>
        /// The handle of the owned control is being created
        /// </summary>
        void OnHandleCreated(object sender, EventArgs e)
        {
            // Window is now created, assign handle to NativeWindow.
            AssignHandle(target.Handle);

            // Force an NC_PAINT
            NativeMethods.SetWindowPos(target.Handle, IntPtr.Zero, 0, 0, 0, 0,
                NativeMethods.SWP_NOMOVE |
                NativeMethods.SWP_NOSIZE |
                NativeMethods.SWP_NOZORDER |
                NativeMethods.SWP_NOACTIVATE |
                NativeMethods.SWP_FRAMECHANGED);
        }

        /// <summary>
        /// The handle of the owned control is being destroyed
        /// </summary>
        void OnHandleDestroyed(object sender, EventArgs e) 
        {
            // Window was destroyed, release hook.
            ReleaseHandle();
        }
                
        /// <summary>
        /// Intercept messages
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == NativeMethods.WM_NCPAINT && ThemedBorderEnabled)
            {
                IntPtr clipRegion = DrawThemedBorder(ref m);

                base.WndProc(ref m);

                if (clipRegion != IntPtr.Zero)
                {
                    NativeMethods.DeleteObject(clipRegion);
                }
            }
            else
            {
                base.WndProc(ref m);
            }
        }

        /// <summary>
        /// Draw a border with the system theme
        /// </summary>
        private IntPtr DrawThemedBorder(ref Message m)
        {
            IntPtr hDC = NativeMethods.GetWindowDC(m.HWnd);

            NativeMethods.RECT rect;
            NativeMethods.GetWindowRect(target.Handle, out rect);

            Rectangle borderRect = new Rectangle(0, 0, rect.right - rect.left - 1, rect.bottom - rect.top - 1);
            Rectangle innerRect = new Rectangle(1, 1, borderRect.Width - 2, borderRect.Height - 2);

            try
            {
                using (Graphics g = Graphics.FromHdc(hDC))
                {
                    using (Pen pen = new Pen(VisualStyleInformation.TextControlBorder))
                    {
                        g.DrawRectangle(pen, borderRect);
                    }

                    using (Pen pen = new Pen(target.BackColor))
                    {
                        g.DrawRectangle(pen, innerRect);
                    }
                }
            }
            finally
            {
                NativeMethods.ReleaseDC(m.HWnd, hDC);
            }

            borderRect = target.RectangleToScreen(borderRect);

            int regionLeft = borderRect.Left + 0;
            int regionTop = borderRect.Top + 0;
            int regionRight = borderRect.Right - 3;
            int regionBottom = borderRect.Bottom - 3;

            // Now we need to clip so the base doesn't overwrite what we just drew
            IntPtr clipRegion = NativeMethods.CreateRectRgn(regionLeft, regionTop, regionRight, regionBottom);

            if (m.WParam == (IntPtr) 1)
            {
                m.WParam = clipRegion;
            }
            else
            {
                NativeMethods.CombineRgn(m.WParam, m.WParam, clipRegion, NativeMethods.RGN_AND);
            }

            m.Result = IntPtr.Zero;

            return clipRegion;
        }
    }
}
