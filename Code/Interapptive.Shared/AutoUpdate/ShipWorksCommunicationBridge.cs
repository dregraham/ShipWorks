using System;
using System.IO.Pipes;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;

namespace Interapptive.Shared.AutoUpdate
{
    /// <summary>
    /// Bridge for the ShipWorks exe to communicate with the ShipWorks Escalator service
    /// </summary>
    [Component]
    public class ShipWorksCommunicationBridge : IShipWorksCommunicationBridge
    {
        public delegate void DelegateMessage(string message);
        public event DelegateMessage OnMessage;
        private readonly string instance;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipWorksCommunicationBridge(string instance, Func<Type, ILog> logFactory)
        {
            this.instance = instance;
            log = logFactory(GetType());
        }

        /// <summary>
        /// Generate a new pipe server and wait for connections
        /// </summary>
        public void StartPipeServer()
        {
            PipeSecurity pipeSecurity = new PipeSecurity();
            SecurityIdentifier sid = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
            pipeSecurity.AddAccessRule(new PipeAccessRule(sid, PipeAccessRights.ReadWrite | PipeAccessRights.CreateNewInstance, AccessControlType.Allow));

            NamedPipeServerStream pipeServer = new NamedPipeServerStream(
                instance,
                PipeDirection.In,
                10,
                PipeTransmissionMode.Byte,
                PipeOptions.Asynchronous,
                255,
                0,
                pipeSecurity);

            log.DebugFormat("Starting named pipe server {0}", instance);

            pipeServer.BeginWaitForConnection(WaitForConnectionCallBack, pipeServer);
        }

        /// <summary>
        /// Send a message
        /// </summary>
        public Result SendMessage(string message)
        {
            using (NamedPipeClientStream updaterPipe = new NamedPipeClientStream(".", instance, PipeDirection.Out))
            {
                try
                {
                    updaterPipe.Connect(1000);
                    updaterPipe.Write(Encoding.UTF8.GetBytes(message), 0, message.Length);
                }
                catch (Exception ex)
                {
                    return Result.FromError($"Could not connect to update service.{Environment.NewLine}{ex.Message}");
                }
                return Result.FromSuccess();
            }
        }

        /// <summary>
        /// Check if the communication bridge is available
        /// </summary>
        public bool IsAvailable()
        {
            using (NamedPipeClientStream updaterPipe = new NamedPipeClientStream(".", instance, PipeDirection.Out))
            {
                if (!updaterPipe.IsConnected)
                {
                    // Give it 1 second to connect
                    try
                    {
                        updaterPipe.Connect(1000);
                    }
                    catch (Exception)
                    {
                        // Connection can fail if something else is connected
                        // or if the timeout has elapsed
                        return false;
                    }
                }

                return updaterPipe.IsConnected;
            }
        }

        /// <summary>
        /// Wait for messages
        /// </summary>
        /// <param name="asyncResult"></param>
        private void WaitForConnectionCallBack(IAsyncResult asyncResult)
        {
            // Get the pipe
            NamedPipeServerStream pipeServer = (NamedPipeServerStream) asyncResult.AsyncState;

            try
            {
                // End waiting for the connection
                pipeServer.EndWaitForConnection(asyncResult);

                byte[] buffer = new byte[255];

                // Read the incoming message
                pipeServer.Read(buffer, 0, 255);

                // Convert byte buffer to string
                string stringData = Encoding.UTF8.GetString(buffer, 0, buffer.Length);

                log.InfoFormat("Received Message '{0}'", stringData);

                if (!string.IsNullOrWhiteSpace(stringData))
                {
                    // Pass message back to calling form
                    OnMessage?.Invoke(stringData.Trim());
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("An error occurred while receiving message from communication bridge. {0}", ex.Message);
                return;
            }

            // Kill original sever and create new wait server
            pipeServer.Dispose();

            StartPipeServer();
        }
    }
}
