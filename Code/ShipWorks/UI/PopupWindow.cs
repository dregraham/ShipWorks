using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared;
using System.Reflection;
using System.Threading;
using Divelements.SandRibbon.Rendering;
using Divelements.SandRibbon;
using System.Diagnostics;
using ShipWorks.ApplicationCore.Appearance;
using System.Drawing.Drawing2D;
using ShipWorks.UI.Utility;
using Interapptive.Shared.Win32;
using ShipWorks.UI.Controls;

namespace ShipWorks.UI
{
    /// <summary>
    /// Window that acts like a menu \ combo dropdown \ tooltip, in that it does not deactivate its parent.
    /// </summary>
    public partial class PopupWindow : System.Windows.Forms.Form
    {
        // Are we done displaying?
        bool done = false;

        // True if we are sending our parent a message to keep itself shown active
        bool sendingActivateMessage = false;

        // Send leave messages when we are done.  This came about from wanting to support the SandRibbon button to stay hilighted
        // while we are within this Popup control.  We steal the leave messages (which would trigger it to lose the hot state), and then
        // give the leave messages back when we are done.
        List<NativeMethods.MSG> pendingMouseLeaves;

        // For resize dragging
        Point dragPoint = new Point(-1, -1);

        // The sizer that will be used
        PopupSizerStyle sizerStyle = PopupSizerStyle.None;

        // If the sizer is visible, where it will go
        PopupSizerLocation sizerLocation = PopupSizerLocation.Bottom;

        // If not null indicates we are displayed as a flyout menu of a ContextMenuStrip.
        ToolStripMenuItem ownerMenuItem;

        // True if we pass on the dismissive click
        bool passOnDismissClick = true;

        /// <summary>
        /// Static constructor
        /// </summary>
        static PopupWindow()
        {
            // Our check below for control name x22b8fc887c9fa272 is depenant on the obfuscated names given in this specific
            // release of SandRibbon.  When we upgrade, we need to update the name we are looking for, or using the template tree popups
            // with a 'Minimized' ribbon won't work.
            if (new RibbonTab().ProductVersion != "1.6.6.1")
            {
                throw new InvalidOperationException("SandRibbon version changed, action needed, read the above comment.");
            }
        }
        
        /// <summary>
        /// Constructor
        /// </summary>
        public PopupWindow()
        {
            InitializeComponent();

            MinimumSize = new Size(100, 100);
        }

        /// <summary>
        /// The type of sizer the control will draw
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(PopupSizerStyle.None)]
        public PopupSizerStyle SizerStyle
        {
            get 
            { 
                return sizerStyle; 
            }
            set 
            {
                if (Visible)
                {
                    throw new InvalidOperationException("Cannot change SizerStyle while window is visible.");
                }

                sizerStyle = value; 
            }
        }

        /// <summary>
        /// The location of the sizer
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(PopupSizerLocation.Bottom)]
        public PopupSizerLocation SizerLocation
        {
            get
            {
                return sizerLocation;
            }
            set
            {
                if (Visible)
                {
                    throw new InvalidOperationException("Cannot change SizerStyle while window is visible.");
                }

                sizerLocation = value;
            }
        }

        /// <summary>
        /// Indicates if we pass on the click tha dismisses the popup. ComboBox's, for example, eat the dismissing click
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(true)]
        public bool PassOnDismissClick
        {
            get { return passOnDismissClick; }
            set { passOnDismissClick = value; }
        }

        /// <summary>
        /// If being displays as a context menu dropdown, this is the owner item.
        /// </summary>
        public ToolStripMenuItem OwnerMenuItem
        {
            get { return ownerMenuItem; }
            set { ownerMenuItem = value; }
        }

        /// <summary>
        /// Overridden just to change the default
        /// </summary>
        [DefaultValue(typeof(Size), "100, 100")]
        public override Size MinimumSize
        {
            get
            {
                return base.MinimumSize;
            }
            set
            {
                base.MinimumSize = value;
            }
        }

        /// <summary>
        /// If this is true, it indicates we are displayed as a flyout menu of a ContextMenuStrip.
        /// </summary>
        private bool ParticipatesInMenu
        {
            get
            {
                return ownerMenuItem != null;
            }
        }

        /// <summary>
        /// Show the window at the specified location
        /// </summary>
        [NDependIgnoreLongMethod]
        [NDependIgnoreComplexMethodAttribute]
        public DialogResult ShowPopup(Form owner, Point location)
        {
            if (owner == null)
            {
                throw new ArgumentNullException("owner");
            }

            Owner = owner;

            StartPosition = FormStartPosition.Manual;
            Location = location;

            pendingMouseLeaves = new List<NativeMethods.MSG>();

            //  Early check:  We must be on same thread as the owner
            //  so we can see its mouse and keyboard messages when we
            //  set capture to it.
            int unused;
            if (NativeMethods.GetCurrentThreadId() != NativeMethods.GetWindowThreadProcessId(owner.Handle, out unused))
            {
                // must be on same thread as parent window.         
                return DialogResult.None;
            }

            NativeMethods.ShowWindow(Handle, NativeMethods.SW_SHOWNOACTIVATE);

            // This is needed to make winforms happy and call OnLoad properly
            Visible = true;

            // When we recieve the last message that causes us to exit, we may
            // need to send it as the last thing we do to the correct control,
            // after we are destroyed.
            bool sendFinalMessageLast = false;

            //  Go into a message loop that filters all the messages
            //  it receives and routes the interesting ones to the
            //  our own window. (they are all intended for our parent window,
            //  since we have set capture on the parent.
            NativeMethods.MSG msg = new NativeMethods.MSG();
            while (NativeMethods.GetMessage(ref msg, IntPtr.Zero, 0, 0) != 0)
            {
                if (CheckDone())
                {
                    break;
                }

                if (IsMouseMessage(msg.message))
                {
                    Point screenPt = new Point(0, 0);
                    screenPt.X = (short) ((int) msg.lParam & 0xffff);
                    screenPt.Y = (short) ((int) msg.lParam >> 16);

                    // Handle all incoming mouse message.  NC messages are NC relative
                    // to the parent.  So they may actually be client for us.  And vice-versa is
                    // also possible.
                    if (IsClientMouseMessage(msg.message))
                    {
                        // For client messages, we have to translate to screen coords
                        screenPt = PointToScreen(msg.hwnd, screenPt);
                    }

                    // Its outside our area (and we are not capturing it)
                    if (!Bounds.Contains(screenPt) && msg.hwnd != Handle)
                    {
                        // If we are participating in menu, let messages to the other parts of the menu go through
                        if (ParticipatesInMenu && Control.FromHandle(msg.hwnd) is ToolStripDropDown)
                        {

                        }
                        else
                        {
                            // Close the window if any clicks occur outside
                            if (IsMouseDownMessage(msg.message))
                            {
                                Control control = Control.FromHandle(msg.hwnd);

                                // Send this final mouse down to the control it was intended for after we close... 
                                sendFinalMessageLast = true;

                                // Except... Special case for SandRibbon.  If we set sendFinalMessageLast to true while this PopupWindow is showing for a RibbonButton, and the user is clicking on the
                                // same tab that the button is in, then the button remains highlited.  I'm not exactly sure why, but I think it has something to do with the fact
                                // that the entire tab is a single window handle, with each button being windowless.
                                if (control != null)
                                {
                                    if (!passOnDismissClick)
                                    {
                                        sendFinalMessageLast = false;
                                    }
                                    else if (control.GetType().Name == "RibbonTab")
                                    {
                                        sendFinalMessageLast = false;
                                    }
                                    // This case is the control name when the Ribbon is in "Minimized" mode.  This is some kind of special host control the Ribbon uses
                                    // for the popped up ribbon tabs.
                                    else if (control.GetType().Name == "x22b8fc887c9fa272")
                                    {
                                        sendFinalMessageLast = false;
                                    }
                                }

                                done = true;
                            }
                            // Otherwise, ignore the message entirely
                            else
                            {
                                continue;
                            }
                        }
                    }
                    else
                    {
                        // Its our message.  If we are participating as a menu drop down, make sure our menu item is selected
                        if (ParticipatesInMenu)
                        {
                            ownerMenuItem.Select();
                        }
                    }

                    // We need mouse wheel support
                    if (msg.message == NativeMethods.WM_MOUSEWHEEL)
                    {
                        // Win32 native version of the screen pt
                        NativeMethods.POINT nativeScreenPt = new NativeMethods.POINT();
                        nativeScreenPt.x = screenPt.X;
                        nativeScreenPt.y = screenPt.Y;

                        // Translate to use the control under the mouse
                        IntPtr targetHwnd = NativeMethods.WindowFromPoint(nativeScreenPt);

                        msg.hwnd = targetHwnd;
                    }
                }

                // Special case.  Its not a mouse message in the sense that the above messages are since it
                // doesn't carry cursor location in its data.
                if (msg.message == NativeMethods.WM_MOUSELEAVE)
                {
                    if (this.Handle != msg.hwnd && !NativeMethods.IsChild(this.Handle, msg.hwnd))
                    {
                        pendingMouseLeaves.Add(msg);
                        continue;
                    }
                }

                // Steal keyboard messages too
                if (IsKeyboardMessage(msg.message))
                {
                    if (msg.message == NativeMethods.WM_KEYDOWN)
                    {
                        if ((int) msg.wParam == (int) Keys.Escape)
                        {
                            done = true;
                        }
                    }

                    // If one of our children has it, let them have it
                    if (!NativeMethods.IsChild(this.Handle, msg.hwnd))
                    {
                        msg.hwnd = this.Handle;
                    }
                }

                if (!done)
                {
                    if (!NativeMethods.IsDialogMessage(Handle, ref msg))
                    {
                        NativeMethods.TranslateMessage(ref msg);
                        NativeMethods.DispatchMessage(ref msg);
                    }
                }

                //  Something may have happened that caused us to stop.
                //  If so, then stop.
                if (CheckDone())
                {
                    break;
                }
            }

            // We don't dispose, b\c the state of the controls may need inspected after the loop
            Visible = false;

            // Translate None to Cancel
            if (DialogResult == DialogResult.None)
            {
                DialogResult = DialogResult.Cancel;
            }

            // See the variable declaration for a description of why we do this
            foreach (NativeMethods.MSG leaveMsg in pendingMouseLeaves)
            {
                NativeMethods.PostMessage(leaveMsg.hwnd, leaveMsg.message, leaveMsg.wParam, leaveMsg.lParam);
            }

            if (sendFinalMessageLast)
            {
                NativeMethods.PostMessage(msg.hwnd, msg.message, msg.wParam, msg.lParam);
            }

            return DialogResult;
        }

        /// <summary>
        /// Determine a location for the control that fits on the screen
        /// </summary>
        private void AdjustLocation(Point desiredLocation)
        {
            // Determine which screen we're on and how big it is.
            Screen monitor = Screen.FromPoint(desiredLocation);

            //  If too high, then slide down.
            if (desiredLocation.Y < monitor.Bounds.Top)
            {
                desiredLocation.Y = monitor.Bounds.Top;
            }

            //  If too far left, then slide right.
            if (desiredLocation.X < monitor.Bounds.Left)
            {
                desiredLocation.X = monitor.Bounds.Left;
            }

            //  If too low, then slide up.
            if (desiredLocation.Y > monitor.Bounds.Bottom - Height)
            {
                desiredLocation.Y = monitor.Bounds.Bottom - Height;
            }

            //  If too far right, then slide left.
            if (desiredLocation.X + Width > monitor.Bounds.Right)
            {
                desiredLocation.X = monitor.Bounds.Right - Width;
            }

            this.Location = desiredLocation;
        }

        /// <summary>
        /// Make sure we dont close before its allowed.
        /// </summary>
        protected override void OnClosing(CancelEventArgs e)
        {
            if (!done)
            {
                done = true;
                e.Cancel = true;
            }

            else
            {
                base.OnClosing(e);

                if (e.Cancel)
                {
                    throw new InvalidOperationException("Cannot cancel closing of a PopupDialog.");
                }
            }
        }

        /// <summary>
        /// Override WndProc as necessary
        /// </summary>
        protected override void WndProc(ref Message msg)
        {
            if (msg.Msg == NativeMethods.WM_NCACTIVATE)
            {
                WmNCActivate(ref msg);
                return;
            }

            //  Do not activate when somebody clicks on me.
            if (msg.Msg == NativeMethods.WM_MOUSEACTIVATE)
            {
                msg.Result = (IntPtr) NativeMethods.MA_NOACTIVATE;
                return;
            }

            // To support the animate api
            if (msg.Msg == NativeMethods.WM_PRINT)
            {
                if (!Visible)
                {
                    Visible = true;
                }
            }

            if (msg.Msg == NativeMethods.WM_SETCURSOR)
            {
                Point position = base.PointToClient(Cursor.Position);

                if (ResizerBounds.Contains(position))
                {
                    if (ResizerCornerBounds.Contains(position))
                    {
                        if (sizerLocation == PopupSizerLocation.Bottom)
                        {
                            Cursor.Current = Cursors.SizeNWSE;
                        }
                        else
                        {
                            Cursor.Current = Cursors.SizeNESW;
                        }
                    }
                    else
                    {
                        Cursor.Current = Cursors.SizeNS;
                    }

                    return;
                }
            }

            base.WndProc(ref msg);
        }

        /// <summary>
        /// This is what keeps the parent from looking deactivated when one of our 
        /// child controls gets focus.
        /// </summary>
        private void WmNCActivate(ref Message msg)
        {
            if (msg.WParam != IntPtr.Zero)
            {
                if (!sendingActivateMessage)
                {
                    sendingActivateMessage = true;

                    try
                    {
                        if (Owner != null)
                        {
                            IntPtr hWnd = Owner.Handle;
                            NativeMethods.SendMessage(hWnd, NativeMethods.WM_NCACTIVATE, (IntPtr) 1, IntPtr.Zero);
                            Owner.Refresh();

                            msg.WParam = (IntPtr) 1;
                        }
                    }
                    finally
                    {
                        sendingActivateMessage = false;
                    }
                }

                DefWndProc(ref msg);
            }
            else
            {
                base.WndProc(ref msg);
            }
        }

        /// <summary>
        /// Returns true if we are done processing and should end the message loop
        /// </summary>
        private bool CheckDone()
        {
            // See if a DialogResult has been set
            if (DialogResult != DialogResult.None)
            {
                done = true;
            }

            //  Something may have happened that caused us to stop.
            //  If so, then stop.
            if (done)
            {
                return true;
            }

            // See if we are still the active window
            if (!IsStillActiveWindow())
            {
                done = true;
            }

            return done;
        }

        /// <summary>
        /// Determines if we are still capturing from our parent active window.
        /// </summary>
        private bool IsStillActiveWindow()
        {
           //  If our owner stopped being the active window
           //  (e.g., the user Alt+Tab'd to another window
           //  in the meantime), then stop.
           IntPtr hwndActive = NativeMethods.GetActiveWindow();

            // See if the active window is in our chain of owners.  If it is, we're ok
            Form ownerCheck = Owner;
            while (ownerCheck != null)
            {
                if (hwndActive == ownerCheck.Handle)
                {
                    return true;
                }

                ownerCheck = ownerCheck.Owner;
            }

            // If we or one of our children is active, that's fine too
            if (hwndActive == this.Handle || NativeMethods.IsChild(hwndActive, this.Handle))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Use specific window styles
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;

                if (!DesignMode)
                {
                    cp.Style =
                        NativeMethods.WS_POPUP |
                        (cp.Style & (NativeMethods.WS_BORDER | NativeMethods.WS_DLGFRAME | NativeMethods.WS_THICKFRAME));

                    cp.Style |= NativeMethods.WS_CLIPCHILDREN;

                    cp.ExStyle =
                        NativeMethods.WS_EX_TOOLWINDOW |
                        NativeMethods.WS_EX_TOPMOST |
                        (cp.ExStyle & (NativeMethods.WS_EX_DLGMODALFRAME | NativeMethods.WS_EX_WINDOWEDGE | NativeMethods.WS_EX_DLGMODALFRAME));
                }

                return cp;
            }
        }

        /// <summary>
        /// Convert the given client point of the given window to screen point 
        /// </summary>
        private Point PointToScreen(IntPtr hwnd, Point clientPt)
        {
            NativeMethods.POINT screenPt = new NativeMethods.POINT();
            screenPt.x = clientPt.X;
            screenPt.y = clientPt.Y;

            NativeMethods.MapWindowPoints(hwnd, IntPtr.Zero, ref screenPt, 1);

            return new Point(screenPt.x, screenPt.y);
        }

        /// <summary>
        /// Determines of the given message is mouse message
        /// </summary>
        private bool IsMouseMessage(int message)
        {
            return IsClientMouseMessage(message) || IsNonClientMouseMessage(message);
        }

        /// <summary>
        /// Determines of the given message is a client message
        /// </summary>
        private bool IsClientMouseMessage(int message)
        {
            switch (message)
            {
                case NativeMethods.WM_MOUSEMOVE:
                case NativeMethods.WM_LBUTTONDOWN:
                case NativeMethods.WM_LBUTTONUP:
                case NativeMethods.WM_LBUTTONDBLCLK:
                case NativeMethods.WM_RBUTTONDOWN:
                case NativeMethods.WM_RBUTTONUP:
                case NativeMethods.WM_RBUTTONDBLCLK:
                case NativeMethods.WM_MBUTTONDOWN:
                case NativeMethods.WM_MBUTTONUP:
                case NativeMethods.WM_MBUTTONDBLCLK:
                    {
                        return true;
                    }
            }

            return false;
        }

        /// <summary>
        /// Determines if the given message targets the NC area
        /// </summary>
        private bool IsNonClientMouseMessage(int message)
        {
            switch (message)
            {
                case NativeMethods.WM_NCMOUSEMOVE:
                case NativeMethods.WM_NCLBUTTONDOWN:
                case NativeMethods.WM_NCLBUTTONUP:
                case NativeMethods.WM_NCLBUTTONDBLCLK:
                case NativeMethods.WM_NCRBUTTONDOWN:
                case NativeMethods.WM_NCRBUTTONUP:
                case NativeMethods.WM_NCRBUTTONDBLCLK:
                case NativeMethods.WM_NCMBUTTONDOWN:
                case NativeMethods.WM_NCMBUTTONUP:
                case NativeMethods.WM_NCMBUTTONDBLCLK:
                case NativeMethods.WM_MOUSEWHEEL:
                    {
                        return true;
                    }
            }

            return false;
        }

        /// <summary>
        /// Indicates if the given message represents a click of the mouse
        /// </summary>
        private bool IsMouseDownMessage(int message)
        {
            return
                message == NativeMethods.WM_LBUTTONDOWN || message == NativeMethods.WM_NCLBUTTONDOWN ||
                message == NativeMethods.WM_RBUTTONDOWN || message == NativeMethods.WM_NCRBUTTONDOWN ||
                message == NativeMethods.WM_MBUTTONDOWN || message == NativeMethods.WM_NCMBUTTONDOWN;
        }

        /// <summary>
        /// Determine if the given message targets the keyboard
        /// </summary>
        private bool IsKeyboardMessage(int message)
        {
            switch (message)
            {
                case NativeMethods.WM_KEYDOWN:
                case NativeMethods.WM_KEYUP:
                case NativeMethods.WM_CHAR:
                case NativeMethods.WM_DEADCHAR:
                case NativeMethods.WM_SYSKEYDOWN:
                case NativeMethods.WM_SYSKEYUP:
                case NativeMethods.WM_SYSCHAR:
                case NativeMethods.WM_SYSDEADCHAR:
                    {
                        return true;
                    }
            }

            return false;
        }

        /// <summary>
        /// The total bounds of the resizer control
        /// </summary>
        private Rectangle ResizerBounds
        {
            get
            {
                if (sizerStyle == PopupSizerStyle.None)
                {
                    return Rectangle.Empty;
                }

                int height = (sizerStyle == PopupSizerStyle.BothDirections) ? 12 : 8;

                Rectangle bounds = base.Bounds;
                bounds.Y = (sizerLocation == PopupSizerLocation.Bottom) ? (bounds.Bottom - height - 1) : bounds.Top + 1;
                bounds.Height = height;

                bounds.X += 1;
                bounds.Width -= 2;

                // We derive from Form, so are bounds are in Screen to start.
                return RectangleToClient(bounds);
            }
        }

        /// <summary>
        /// The bounds of the corner of the resizer where you can drag both directions.
        /// </summary>
        private Rectangle ResizerCornerBounds
        {
            get
            {
                if (sizerStyle != PopupSizerStyle.BothDirections)
                {
                    return Rectangle.Empty;
                }

                return new Rectangle(ResizerBounds.Right - ResizerBounds.Height, ResizerBounds.Y, ResizerBounds.Height, ResizerBounds.Height);
            }
        }

        /// <summary>
        /// Provides the actual space where the OptionPage goes
        /// </summary>
        public override Rectangle DisplayRectangle
        {
            get
            {
                // None
                if (sizerStyle == PopupSizerStyle.None)
                {
                    return base.DisplayRectangle;
                }

                // Bottom
                if (sizerLocation == PopupSizerLocation.Bottom)
                {
                    return new Rectangle(1, 1, Width - 2, ResizerBounds.Y - 2);
                }

                // Top
                else
                {
                    return new Rectangle(1, ResizerBounds.Bottom + 1, Width - 2, Height - ResizerBounds.Bottom - 2);
                }
            }
        }

        /// <summary>
        /// Custom painting to paint the resizer
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using (RibbonRenderer renderer = AppearanceHelper.CreateRibbonRenderer())
            {
                using (RenderingContext context = AppearanceHelper.CreateRibbonRenderingContext(e.Graphics, renderer, Font, false, renderer.ColorTable.RibbonText, renderer.ColorTable.RibbonDisabledText))
                {
                    Rectangle bounds = RectangleToClient(Bounds);

                    if (ThemeInformation.VisualStylesEnabled)
                    {
                        renderer.DrawPopupBackground(e.Graphics, bounds, false);
                    }

                    if (sizerStyle != PopupSizerStyle.None)
                    {
                        DrawResizeGripper(context, ResizerBounds, sizerStyle);
                    }

                    DrawBorder(context, bounds);
                }
            }
        }

        /// <summary>
        /// Draw the resize gripper
        /// </summary>
        private void DrawResizeGripper(RenderingContext context, Rectangle bounds, PopupSizerStyle sizerStyle)
        {
            Office2007ColorTable colorTable = context.Renderer.ColorTable;

            if (colorTable.EnableShines)
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(bounds, Color.White, colorTable.PopupResizeGripper, LinearGradientMode.Vertical))
                {
                    context.Graphics.FillRectangle(brush, bounds);
                }
            }
            else
            {
                using (SolidBrush brush = new SolidBrush(colorTable.PopupResizeGripper))
                {
                    context.Graphics.FillRectangle(brush, bounds);
                }
            }

            Color lineColor = (sizerLocation == PopupSizerLocation.Bottom) ? colorTable.PopupResizeGripper : DisplayHelper.DarkenColor(colorTable.PopupResizeGripper, .2);

            using (Pen pen = new Pen(lineColor))
            {
                int yAxis = (sizerLocation == PopupSizerLocation.Bottom) ? bounds.Y : bounds.Bottom;

                context.Graphics.DrawLine(pen, bounds.X, yAxis, bounds.Right - 1, yAxis);
            }

            if (sizerStyle == PopupSizerStyle.Vertical)
            {
                int y = (bounds.Y + (bounds.Height / 2)) - 1;

                for (int i = 0; i < 4; i++)
                {
                    int x = ((bounds.X + (bounds.Width / 2)) + (5 * i)) - 10;
                    context.Renderer.DrawGlyph(context.Graphics, new Rectangle(x, y, 0, 0), GlyphType.GripperDot, DrawState.Normal);
                }
            }
            else
            {
                context.Renderer.DrawGlyph(context.Graphics, new Rectangle(bounds.Right - 5, bounds.Y + 3, 0, 0), GlyphType.GripperDot, DrawState.Normal);
                context.Renderer.DrawGlyph(context.Graphics, new Rectangle(bounds.Right - 5, bounds.Y + 7, 0, 0), GlyphType.GripperDot, DrawState.Normal);

                if (sizerLocation == PopupSizerLocation.Bottom)
                {
                    context.Renderer.DrawGlyph(context.Graphics, new Rectangle(bounds.Right - 9, bounds.Y + 7, 0, 0), GlyphType.GripperDot, DrawState.Normal);
                }
                else
                {
                    context.Renderer.DrawGlyph(context.Graphics, new Rectangle(bounds.Right - 9, bounds.Y + 3, 0, 0), GlyphType.GripperDot, DrawState.Normal);
                }
            }
        }

        /// <summary>
        /// Draw the border around the control
        /// </summary>
        private void DrawBorder(RenderingContext context, Rectangle bounds)
        {
            Point[] points = new Point[] { 
                        new Point(bounds.X + 1, bounds.Y), 
                        new Point(bounds.Right - 2, bounds.Y), 
                        new Point(bounds.Right - 1, bounds.Y + 1), 
                        new Point(bounds.Right - 1, bounds.Bottom - 2),
                        new Point(bounds.Right - 2, bounds.Bottom - 1), 
                        new Point(bounds.X + 1, bounds.Bottom - 1), 
                        new Point(bounds.X, bounds.Bottom - 2), 
                        new Point(bounds.X, bounds.Y + 1) };

            using (Pen pen = new Pen(context.Renderer.ColorTable.PopupBorder))
            {
                context.Graphics.DrawPolygon(pen, points);
            }
        }

        /// <summary>
        /// User has clicked the mouse down
        /// </summary>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            dragPoint = new Point(e.X, e.Y);

            base.OnMouseDown(e);
        }

        /// <summary>
        /// Capture has changed
        /// </summary>
        protected override void OnMouseCaptureChanged(EventArgs e)
        {
            if (!Capture)
            {
                dragPoint = new Point(-1, -1);
            }

            base.OnMouseCaptureChanged(e);
        }

        /// <summary>
        /// Mouse is moving
        /// </summary>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (sizerStyle != PopupSizerStyle.None &&
                e.Button == MouseButtons.Left &&
                dragPoint != new Point(-1, -1) &&
                ResizerBounds.Contains(dragPoint))
            {
                int xChange = e.X - dragPoint.X;

                // Restrict how small it can drag based on the min size
                if ((Width + xChange) < MinimumSize.Width)
                {
                    xChange = MinimumSize.Width - Width;
                }

                // Cant change widths in this case
                if (sizerStyle == PopupSizerStyle.Vertical || !ResizerCornerBounds.Contains(dragPoint))
                {
                    xChange = 0;
                }

                int yChange = e.Y - dragPoint.Y;

                if (sizerLocation == PopupSizerLocation.Top)
                {
                    yChange = -yChange;
                }

                // Restrict how smaill it can drag base don the min size
                if ((Height + yChange) < MinimumSize.Height)
                {
                    yChange = MinimumSize.Height - Height;
                }

                Rectangle oldBounds = base.Bounds;
                int top = this.Top;

                if (sizerLocation == PopupSizerLocation.Top)
                {
                    top = this.Top - yChange;
                }

                // Set the new bounds
                SetBounds(Left, top, Width + xChange, Height + yChange);

                // This is in case the Size we requested isnt actually what we will get.
                xChange = Bounds.Width - oldBounds.Width;
                yChange = Bounds.Height - oldBounds.Height;

                // Drag point stays same relative to top, when dragging top
                if (sizerLocation == PopupSizerLocation.Top)
                {
                    yChange = 0;
                }

                // Upate the point we will be dragging from next time
                dragPoint.X += xChange;
                dragPoint.Y += yChange;

                Invalidate();
                Update();
            }

            base.OnMouseMove(e);
        }
    }
}
