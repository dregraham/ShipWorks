using System;

namespace ShipWorks.Stores.Platforms.Yahoo
{
    public class YahooException : Exception
    {
        public YahooException()
        {

        }

        public YahooException(string message)
            : base(message)
        {

        }

        public YahooException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
