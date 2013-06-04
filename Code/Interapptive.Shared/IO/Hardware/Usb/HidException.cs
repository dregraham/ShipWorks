using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interapptive.Shared.Usb
{
    class HidException : Exception
    {
        public HidException()
        {

        }

        public HidException(string message)
            : base(message)
        {

        }

        public HidException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
