using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.ApplicationCore.Nudges
{
    public class NudgeException : Exception
    {
        public NudgeException()
        {

        }

        public NudgeException(string message)
            : base(message)
        {

        }

        public NudgeException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
