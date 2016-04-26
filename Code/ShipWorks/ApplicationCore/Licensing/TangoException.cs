using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Exception thrown by TangoWebClient when there is a problem accessing Tango
    /// </summary>
    public class TangoException : Exception
    {
        public TangoException()
        {

        }

        public TangoException(Exception ex) :
            base(ex.Message, ex)
        {

        }

        public TangoException(string message)
            : base(message)
        {

        }

        public TangoException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
