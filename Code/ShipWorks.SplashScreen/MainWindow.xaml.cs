using System;
using System.IO.Pipes;
using System.Security.AccessControl;
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
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            double windowWidth = Width;
            double windowHeight = Height;
            Left = (screenWidth / 2) - (windowWidth / 2);
            Top = (screenHeight / 2) - (windowHeight / 2);
        }

        /// <summary>
        /// Start the pipe server
        /// </summary>
        public void StartPipeServer()
        {
            PipeSecurity pipeSecurity = new PipeSecurity();
            pipeSecurity.AddAccessRule(new PipeAccessRule(@"Everyone", PipeAccessRights.ReadWrite, AccessControlType.Allow));

            NamedPipeServerStream pipeServer =
                new NamedPipeServerStream(
                    PipeName,
                    PipeDirection.In,
                    1,
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
    }
}
