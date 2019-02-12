using System;
using System.IO.Pipes;
using System.Security.AccessControl;
using System.Text;
using log4net;

namespace ShipWorks.Escalator
{
    /// <summary>
    /// Bridge for the ShipWorks exe to communicate with the ShipWorks Escalator service
    /// </summary>
    public class ShipWorksCommunicationBridge
    {
        public delegate void DelegateMessage(string message);
        public event DelegateMessage OnMessage;
        private string instance;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipWorksCommunicationBridge(string instance, ILog log)
        {
            this.instance = instance;
            this.log = log;
            StartPipeServer();
        }

        /// <summary>
        /// Generate a new pipe server and wait for connections
        /// </summary>
        private void StartPipeServer()
        {
            PipeSecurity pipeSecurity = new PipeSecurity();
            pipeSecurity.AddAccessRule(new PipeAccessRule(@"Everyone", PipeAccessRights.ReadWrite, AccessControlType.Allow));

            NamedPipeServerStream pipeServer =
                new NamedPipeServerStream(
                    instance,
                    PipeDirection.In,
                    1,
                    PipeTransmissionMode.Byte,
                    PipeOptions.Asynchronous,
                    255,
                    0,
                    pipeSecurity);

            log.DebugFormat("Starting named pipe server {0}", instance);

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

                log.InfoFormat("Received Message {0}", stringData);

                // Pass message back to calling form
                OnMessage?.Invoke(stringData.Trim());
            }
            catch (Exception ex)
            {
                log.ErrorFormat("An error occurred while receiving message from communication bridge. {0}", ex.Message);
                return;
            }

            // Kill original sever and create new wait server
            pipeServer?.Dispose();

            StartPipeServer();
        }
    }
}
