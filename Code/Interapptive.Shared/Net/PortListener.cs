using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using log4net;
using Interapptive.Shared.Utility;

namespace Interapptive.Shared.Net
{
    /// <summary>
    /// HttpListener implementation for OAuth verification.
    /// </summary>
    public class PortListener : IDisposable
    {
        static readonly ILog log = LogManager.GetLogger(typeof(PortListener));

        private HttpListener listener;
        private int port = -1;

        public event EventHandler<PortListenerRequestBeginEventArgs> BeginRequest;

        /// <summary>
        /// Port on which to listen
        /// </summary>
        public int Port
        {
            get
            {
                return port;
            }
        }

        /// <summary>
        /// Creates an HttpListener so that OAuth redirects can be caught.  When a request is caught
        /// CallbackOccurred will be raised, passing the Url of the request  
        /// </summary>
        /// <param name="startPort">
        /// Pass in an optional port from which to start looking for open ports.  
        /// </param>
        /// <returns>
        /// Returns the port to which the listener found and attached.
        /// </returns>
        public int StartListening()
        {
            int startPort = 6265;
            bool successListening = false;

            if (listener != null && listener.IsListening)
            {
                throw new InvalidOperationException("Port listener is already listening.  Do not call StartListening more than once.");
            }

            string listenerAddress = string.Empty;
            
            // Go into a loop, trying to find a port that is open and that can attached
            while (!successListening)
            {
                try
                {
                    // Grab the next available port not in use
                    startPort = GetAvailableTcpPort(startPort);
                    if (startPort == -1 || startPort > 65555)
                    {
                        // If we couldn't find a port, and got above 65k, throw an error
                        string msg = string.Format(CultureInfo.InvariantCulture, "Unable to find available port >= {0}", startPort);
                        throw new NotFoundException(msg);
                    }

                    // The address will be localhost so that the listener can capture it, on the selected port
                    listenerAddress = string.Format(CultureInfo.InvariantCulture, "http://localhost:{0}/", startPort); 

                    // Setup the httplistener class
                    listener = new HttpListener();
                    listener.Prefixes.Add(listenerAddress);
                    listener.Start();
                    successListening = true;
                }
                catch (HttpListenerException hlEx)
                {
                    // We weren't able to attach to the port...maybe someone got it before we did
                    log.Info(string.Format(CultureInfo.InvariantCulture, "Exception during StartListening for port: {0}. ", startPort), hlEx);

                    if (listener != null)
                    {
                        listener.Close();
                        
                        listener = null;
                    }

                    // In case the error was caused by someone else getting our port before we could, increment by 1 and try to find the next available one.
                    startPort++;
                }
            }

            // Set our callback method
            listener.BeginGetContext(HttpListenerCallback, listener);

            // Return the port we are successfully listening on
            port = startPort;
            return startPort;
        }
 
        /// <summary>
        /// Method that is called when a call has been heard on the listen address and port
        /// Raises a call to CallbackOccurred event
        /// </summary>
        protected void HttpListenerCallback(IAsyncResult result)
        {
            if (result == null)
                throw new ArgumentNullException("result", "result is required");

            listener = (HttpListener)result.AsyncState;
            if (listener.IsListening)
            {
                // Call EndGetContext to complete the asynchronous operation.
                HttpListenerContext context = listener.EndGetContext(result);
                HttpListenerRequest request = context.Request;

                if (BeginRequest != null)
                {
                    BeginRequest(result,new PortListenerRequestBeginEventArgs(request.Url));
                }
            }
        }

        /// <summary>
        /// Helper method to find an available port on which to listen
        /// </summary>
        /// <param name="startPortNumber">While scanning through ports, this port will be the starting port number</param>
        /// <returns></returns>
        private static int GetAvailableTcpPort(int startPortNumber)
        {
            // Evaluate current system tcp connections. This is the same information provided
            // by the netstat command line application, just in .Net strongly-typed object
            // form.  We will look through the list, and if our port we would like to use
            // in our TcpClient is occupied, we will set isAvailable to false.
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();

            List<int> takenPorts = tcpConnInfoArray.Where(tcpInfo => tcpInfo.LocalEndPoint.Port >= startPortNumber).Select(tcpInfo => tcpInfo.LocalEndPoint.Port).ToList();

            while (startPortNumber < 65555)
            {
                if (takenPorts.Contains(startPortNumber))
                {
                    startPortNumber++;
                }
                else
                {
                    return startPortNumber;
                }
            }

            throw new InvalidOperationException("Unable to find an available port on which to listen.");
        }

        /// <summary>
        /// Dispose.  Will release the listener if it can.
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            // If disposing equals true, dispose all managed 
            // and unmanaged resources.
            if (disposing)
            {
                // Dispose managed resources.
                if (listener != null)
                {
                    if (listener.IsListening)
                    {
                        listener.Close();
                    }
                    listener = null;
                }
            }
        }

        /// <summary>
        /// Dispose, and supress finalize.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
