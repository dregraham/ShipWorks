using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.ApplicationCore.Dashboard.Content
{
    /// <summary>
    /// Exception thrown when there is a problem reading the server message feed.
    /// </summary>
    class ServerMessageFeedException : Exception
    {
        public ServerMessageFeedException()
        {

        }

        public ServerMessageFeedException(string message)
            : base(message)
        {

        }

        public ServerMessageFeedException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
