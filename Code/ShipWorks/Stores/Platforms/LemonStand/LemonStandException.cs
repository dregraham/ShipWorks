using System;

namespace ShipWorks.Stores.Platforms.LemonStand
{
    public class LemonStandException : Exception
    {
         public LemonStandException()
        {

        }

        public LemonStandException(string message)
            : base(message)
        {

        }

        public LemonStandException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
