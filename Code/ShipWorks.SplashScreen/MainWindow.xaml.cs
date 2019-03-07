using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Timers;
using System.Windows;

namespace ShipWorks.SplashScreen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string PipeName = "ShipWorksUpgradeStatus";
        private const int Timeout = 60000;
        private Timer timeoutTimer;

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            timeoutTimer = new Timer(Timeout);
            timeoutTimer.Elapsed += (o,e) => Dispatcher.Invoke(() => Close());

            StartPipeServer();
            CenterWindowOnScreen();
        }

        /// <summary>
        /// Center the window on the screen
        /// </summary>
        private void CenterWindowOnScreen()
        {
            Process shipworksProcess = Process.GetProcessesByName("shipworks")
                .FirstOrDefault(p => p.MainWindowHandle != IntPtr.Zero);

            Rect shipworksRect = new Rect();
            if (shipworksProcess != null &&
                !IsShipWorksMinimized(shipworksProcess) &&
                GetWindowRect(new HandleRef(this, shipworksProcess.MainWindowHandle), out shipworksRect))
            {
                // Center th splash screen on top of the shipworks window
                double swWidth = shipworksRect.Right - shipworksRect.Left;
                double swHeight = shipworksRect.Top - shipworksRect.Bottom;
                Left = shipworksRect.Left + ((swWidth / 2) - (Width / 2));
                Top = shipworksRect.Top - ((swHeight / 2) + (Height / 2));
            }
            else
            {
                // center the splash screen on the main window
                double screenWidth = SystemParameters.PrimaryScreenWidth;
                double screenHeight = SystemParameters.PrimaryScreenHeight;
                double windowWidth = Width;
                double windowHeight = Height;
                Left = (screenWidth / 2) - (windowWidth / 2);
                Top = (screenHeight / 2) - (windowHeight / 2);
            }
        }

        /// <summary>
        /// Checks to see if the ShipWorks process is minimized
        /// </summary>
        private static bool IsShipWorksMinimized(Process shipworksProcess)
        {
            WindowPlacement placement = new WindowPlacement();
            GetWindowPlacement(shipworksProcess.MainWindowHandle, ref placement);
            switch (placement.showCmd)
            {
                case 2:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Start the pipe server
        /// </summary>
        public void StartPipeServer()
        {
            PipeSecurity pipeSecurity = new PipeSecurity();
            pipeSecurity.AddAccessRule(new PipeAccessRule(@"Everyone", PipeAccessRights.ReadWrite, AccessControlType.Allow));
            pipeSecurity.AddAccessRule(new PipeAccessRule(WindowsIdentity.GetCurrent().Owner, PipeAccessRights.FullControl, AccessControlType.Allow));

            NamedPipeServerStream pipeServer =
                new NamedPipeServerStream(
                    PipeName,
                    PipeDirection.In,
                    10,
                    PipeTransmissionMode.Byte,
                    PipeOptions.Asynchronous,
                    255,
                    0,
                    pipeSecurity);

            timeoutTimer.Stop();
            timeoutTimer.Start();

            pipeServer.BeginWaitForConnection(
                   new AsyncCallback(WaitForConnectionCallBack), pipeServer);
        }

        /// <summary>
        /// Wait for messages
        /// </summary>
        /// <param name="asyncResult"></param>
        private void WaitForConnectionCallBack(IAsyncResult asyncResult)
        {
            // Get the pipe
            NamedPipeServerStream pipeServer = (NamedPipeServerStream)asyncResult.AsyncState;

            try
            {
                // End waiting for the connection
                pipeServer.EndWaitForConnection(asyncResult);

                byte[] buffer = new byte[255];

                // Read the incoming message
                pipeServer.Read(buffer, 0, 255);

                // Convert byte buffer to string
                string stringData = Encoding.UTF8.GetString(buffer, 0, buffer.Length).Replace("\0", string.Empty);

                if (!string.IsNullOrWhiteSpace(stringData))
                {
                    if (stringData == "Close")
                    {
                        // close the dialog
                        Dispatcher.Invoke(() => Application.Current.Shutdown());
                    }
                    else
                    {
                        Dispatcher.Invoke(() => Status.Text = stringData);
                    }

                }
            }
            catch (Exception)
            {

            }

            // Kill original sever and create new wait server
            pipeServer?.Dispose();

            StartPipeServer();
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowPlacement(IntPtr hWnd, ref WindowPlacement lpwndpl);

        [StructLayout(LayoutKind.Sequential)]
        private struct WindowPlacement
        {
            public int length;
            public int flags;
            public int showCmd;
            public System.Drawing.Point ptMinPosition;
            public System.Drawing.Point ptMaxPosition;
            public System.Drawing.Rectangle rcNormalPosition;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(HandleRef hWnd, out Rect lpRect);

        [StructLayout(LayoutKind.Sequential)]
        private struct Rect
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }
    }
}
