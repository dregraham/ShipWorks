using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Shell;

namespace ShipWorks.Installer.Behaviors
{
    public class WindowChromeLoadedBehavior
    {
        private Window window;

        /// <summary>
        /// Attaches this behavior to the provided window
        /// </summary>
        public void Attach(Window window)
        {
            this.window = window;
            this.window.Loaded += OnLoaded;
        }

        /// <summary>
        /// OnLoaded event handler for the attached window
        /// </summary>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (window == null)
            {
                return;
            }

            Task.Delay(5).ContinueWith(_ =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var oldWindowChrome = WindowChrome.GetWindowChrome(window);

                    if (oldWindowChrome == null)
                    {
                        return;
                    }

                    var newWindowChrome = new WindowChrome
                    {
                        CaptionHeight = oldWindowChrome.CaptionHeight,
                        CornerRadius = oldWindowChrome.CornerRadius,
                        GlassFrameThickness = new Thickness(0, 0, 0, 1),
                        NonClientFrameEdges = NonClientFrameEdges.None,
                        ResizeBorderThickness = oldWindowChrome.ResizeBorderThickness,
                        UseAeroCaptionButtons = oldWindowChrome.UseAeroCaptionButtons
                    };

                    WindowChrome.SetWindowChrome(window, newWindowChrome);
                });
            });

            var hWnd = new WindowInteropHelper(window).Handle;
            HwndSource.FromHwnd(hWnd)?.AddHook(WndProc);
        }

        /// <summary>
        /// Windows process handler for overriding the base window chrome
        /// </summary>
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case NativeMethods.WM_NCPAINT:
                    RemoveFrame();
                    handled = false;
                    break;

                case NativeMethods.WM_NCCALCSIZE:

                    handled = false;

                    var rcClientArea = (RECT) Marshal.PtrToStructure(lParam, typeof(RECT));
                    rcClientArea.Bottom += (int) (WindowChromeHelper.WindowResizeBorderThickness.Bottom / 2);
                    Marshal.StructureToPtr(rcClientArea, lParam, false);

                    var retVal = IntPtr.Zero;
                    if (wParam == new IntPtr(1))
                    {
                        retVal = new IntPtr((int) NativeMethods.WVR.REDRAW);
                    }
                    return retVal;
            }

            return IntPtr.Zero;
        }

        /// <summary>
        /// Extends the client area into the non-client frame
        /// </summary>
        private void RemoveFrame()
        {
            if (Environment.OSVersion.Version.Major >= 6 && NativeMethods.IsDwmAvailable())
            {
                if (NativeMethods.DwmIsCompositionEnabled() && SystemParameters.DropShadow)
                {
                    NativeMethods.MARGINS margins;

                    margins.bottomHeight = -1;
                    margins.leftWidth = 0;
                    margins.rightWidth = 0;
                    margins.topHeight = 0;

                    var helper = new WindowInteropHelper(window);

                    NativeMethods.DwmExtendFrameIntoClientArea(helper.Handle, ref margins);
                }
            }
        }

        /// <summary>
        /// Struct implementation of the Windows RECT
        /// </summary>
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
            public static RECT Empty;

            /// <summary>
            /// The Width of this RECT
            /// </summary>
            public int Width => Math.Abs(Right - Left);

            /// <summary>
            /// The Height of this RECT
            /// </summary>
            public int Height => (Bottom - Top);

            /// <summary>
            /// Empty Constructor
            /// </summary>
            static RECT()
            {
                Empty = new RECT();
            }

            /// <summary>
            /// Constructor from the 4 Rect lengths
            /// </summary>
            public RECT(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            /// <summary>
            /// Constructor from another RECT
            /// </summary>
            public RECT(RECT rcSrc)
            {
                Left = rcSrc.Left;
                Top = rcSrc.Top;
                Right = rcSrc.Right;
                Bottom = rcSrc.Bottom;
            }

            /// <summary>
            /// Constructor from a System.Drawing.Rectangle
            /// </summary>
            public RECT(Rectangle rectangle) : this(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom)
            {
            }

            /// <summary>
            /// Determines if this RECT is empty
            /// </summary>
            public bool IsEmpty
            {
                get
                {
                    if (Left < Right)
                    {
                        return (Top >= Bottom);
                    }
                    return true;
                }
            }

            /// <summary>
            /// Converts the RECT to a string value
            /// </summary>
            public override string ToString()
            {
                if (this == Empty)
                {
                    return "RECT {Empty}";
                }
                return string.Concat("RECT { left : ", Left, " / top : ", Top, " / right : ", Right, " / bottom : ", Bottom, " }");
            }

            /// <summary>
            /// Determines if 2 RECTS are equal to each other
            /// </summary>
            public override bool Equals(object obj)
            {
                return ((obj is Rect) && (this == ((RECT) obj)));
            }

            /// <summary>
            /// Gets the Hash code for this RECT
            /// </summary>
            public override int GetHashCode()
            {
                return ((Left.GetHashCode() + Top.GetHashCode()) + Right.GetHashCode()) + Bottom.GetHashCode();
            }

            /// <summary>
            /// == Operator override
            /// </summary>
            public static bool operator ==(RECT rect1, RECT rect2)
            {
                return ((((rect1.Left == rect2.Left) && (rect1.Top == rect2.Top)) && (rect1.Right == rect2.Right)) && (rect1.Bottom == rect2.Bottom));
            }

            /// <summary>
            /// != Operator override
            /// </summary>
            public static bool operator !=(RECT rect1, RECT rect2)
            {
                return !(rect1 == rect2);
            }
        }

    }
}
